import * as signalR from '@microsoft/signalr/dist/browser/signalr.js';
import PropTypes from 'prop-types';
import { Component } from 'react';
import { connect } from 'react-redux';
import { createSelector } from 'reselect';
import { setAppValue, setVersion } from 'Store/Actions/appActions';
import { removeItem, update, updateItem } from 'Store/Actions/baseActions';
import { fetchCommands, finishCommand, updateCommand } from 'Store/Actions/commandActions';
import { fetchIndexers } from 'Store/Actions/indexerActions';
import { fetchIndexerStatus } from 'Store/Actions/indexerStatusActions';
import { fetchHealth } from 'Store/Actions/systemActions';
import { fetchTagDetails, fetchTags } from 'Store/Actions/tagActions';
import { repopulatePage } from 'Utilities/pagePopulator';
import titleCase from 'Utilities/String/titleCase';

function getHandlerName(name) {
  name = titleCase(name);
  name = name.replace('/', '');

  return `handle${name}`;
}

function createMapStateToProps() {
  return createSelector(
    (state) => state.app.isReconnecting,
    (state) => state.app.isDisconnected,
    (isReconnecting, isDisconnected) => {
      return {
        isReconnecting,
        isDisconnected
      };
    }
  );
}

const mapDispatchToProps = {
  dispatchFetchCommands: fetchCommands,
  dispatchUpdateCommand: updateCommand,
  dispatchFinishCommand: finishCommand,
  dispatchSetAppValue: setAppValue,
  dispatchSetVersion: setVersion,
  dispatchUpdate: update,
  dispatchUpdateItem: updateItem,
  dispatchRemoveItem: removeItem,
  dispatchFetchHealth: fetchHealth,
  dispatchFetchIndexers: fetchIndexers,
  dispatchFetchIndexerStatus: fetchIndexerStatus,
  dispatchFetchTags: fetchTags,
  dispatchFetchTagDetails: fetchTagDetails
};

function Logger(minimumLogLevel) {
  this.minimumLogLevel = minimumLogLevel;
}

Logger.prototype.cleanse = function(message) {
  const apikey = new RegExp(`access_token=${encodeURIComponent(window.Fetcharr.apiKey)}`, 'g');
  return message.replace(apikey, 'access_token=(removed)');
};

Logger.prototype.log = function(logLevel, message) {
  // see https://github.com/aspnet/AspNetCore/blob/21c9e2cc954c10719878839cd3f766aca5f57b34/src/SignalR/clients/ts/signalr/src/Utils.ts#L147
  if (logLevel >= this.minimumLogLevel) {
    switch (logLevel) {
      case signalR.LogLevel.Critical:
      case signalR.LogLevel.Error:
        console.error(`[signalR] ${signalR.LogLevel[logLevel]}: ${this.cleanse(message)}`);
        break;
      case signalR.LogLevel.Warning:
        console.warn(`[signalR] ${signalR.LogLevel[logLevel]}: ${this.cleanse(message)}`);
        break;
      case signalR.LogLevel.Information:
        console.info(`[signalR] ${signalR.LogLevel[logLevel]}: ${this.cleanse(message)}`);
        break;
      default:
        // console.debug only goes to attached debuggers in Node, so we use console.log for Trace and Debug
        console.log(`[signalR] ${signalR.LogLevel[logLevel]}: ${this.cleanse(message)}`);
        break;
    }
  }
};

class SignalRConnector extends Component {

  //
  // Lifecycle

  constructor(props, context) {
    super(props, context);

    this.connection = null;
  }

  componentDidMount() {
    console.log('[signalR] starting');

    const url = `${window.Fetcharr.urlBase}/signalr/messages`;

    this.connection = new signalR.HubConnectionBuilder()
      .configureLogging(new Logger(signalR.LogLevel.Information))
      .withUrl(`${url}?access_token=${encodeURIComponent(window.Fetcharr.apiKey)}`)
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (retryContext) => {
          if (retryContext.elapsedMilliseconds > 180000) {
            this.props.dispatchSetAppValue({ isDisconnected: true });
          }
          return Math.min(retryContext.previousRetryCount, 10) * 1000;
        }
      })
      .build();

    this.connection.onreconnecting(this.onReconnecting);
    this.connection.onreconnected(this.onReconnected);
    this.connection.onclose(this.onClose);

    this.connection.on('receiveMessage', this.onReceiveMessage);

    this.connection.start().then(this.onStart, this.onStartFail);
  }

  componentWillUnmount() {
    this.connection.stop();
    this.connection = null;
  }

  //
  // Control
  handleMessage = (message) => {
    const {
      name,
      body
    } = message;

    const handler = this[getHandlerName(name)];

    if (handler) {
      handler(body);
      return;
    }

    console.error(`signalR: Unable to find handler for ${name}`);
  };

  handleCommand = (body) => {
    if (body.action === 'sync') {
      this.props.dispatchFetchCommands();
      return;
    }

    const resource = body.resource;
    const status = resource.status;

    // Both sucessful and failed commands need to be
    // completed, otherwise they spin until they timeout.

    if (status === 'completed' || status === 'failed') {
      this.props.dispatchFinishCommand(resource);
    } else {
      this.props.dispatchUpdateCommand(resource);
    }
  };

  handleHealth = () => {
    this.props.dispatchFetchHealth();
  };

  handleIndexerstatus = () => {
    this.props.dispatchFetchIndexerStatus();
  };

  handleIndexer = (body) => {
    const action = body.action;
    const section = 'indexers';

    if (action === 'updated') {
      this.props.dispatchUpdateItem({ section, ...body.resource });
    } else if (action === 'deleted') {
      this.props.dispatchRemoveItem({ section, id: body.resource.id });
    }
  };

  handleVersion = (body) => {
    const version = body.version;

    this.props.dispatchSetVersion({ version });
  };

  handleSystemTask = () => {
    this.props.dispatchFetchCommands();
  };

  handleTag = (body) => {
    if (body.action === 'sync') {
      this.props.dispatchFetchTags();
      this.props.dispatchFetchTagDetails();
      return;
    }
  };

  //
  // Listeners

  onStartFail = (error) => {
    console.error('[signalR] failed to connect');
    console.error(error);

    this.props.dispatchSetAppValue({
      isConnected: false,
      isReconnecting: false,
      isDisconnected: false,
      isRestarting: false
    });
  };

  onStart = () => {
    console.debug('[signalR] connected');

    this.props.dispatchSetAppValue({
      isConnected: true,
      isReconnecting: false,
      isDisconnected: false,
      isRestarting: false
    });
  };

  onReconnecting = () => {
    this.props.dispatchSetAppValue({ isReconnecting: true });
  };

  onReconnected = () => {

    const {
      dispatchFetchCommands,
      dispatchFetchIndexers,
      dispatchSetAppValue
    } = this.props;

    dispatchSetAppValue({
      isConnected: true,
      isReconnecting: false,
      isDisconnected: false,
      isRestarting: false
    });

    // Repopulate the page (if a repopulator is set) to ensure things
    // are in sync after reconnecting.
    dispatchFetchIndexers();
    dispatchFetchCommands();
    repopulatePage();
  };

  onClose = () => {
    console.debug('[signalR] connection closed');
  };

  onReceiveMessage = (message) => {
    console.debug('[signalR] received', message.name, message.body);

    this.handleMessage(message);
  };

  //
  // Render

  render() {
    return null;
  }
}

SignalRConnector.propTypes = {
  isReconnecting: PropTypes.bool.isRequired,
  isDisconnected: PropTypes.bool.isRequired,
  dispatchFetchCommands: PropTypes.func.isRequired,
  dispatchUpdateCommand: PropTypes.func.isRequired,
  dispatchFinishCommand: PropTypes.func.isRequired,
  dispatchSetAppValue: PropTypes.func.isRequired,
  dispatchSetVersion: PropTypes.func.isRequired,
  dispatchUpdate: PropTypes.func.isRequired,
  dispatchUpdateItem: PropTypes.func.isRequired,
  dispatchRemoveItem: PropTypes.func.isRequired,
  dispatchFetchHealth: PropTypes.func.isRequired,
  dispatchFetchIndexers: PropTypes.func.isRequired,
  dispatchFetchIndexerStatus: PropTypes.func.isRequired,
  dispatchFetchTags: PropTypes.func.isRequired,
  dispatchFetchTagDetails: PropTypes.func.isRequired
};

export default connect(createMapStateToProps, mapDispatchToProps)(SignalRConnector);
