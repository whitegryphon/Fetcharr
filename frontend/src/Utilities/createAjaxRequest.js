import $ from 'jquery';

const absUrlRegex = /^(https?:)?\/\//i;
const apiRoot = window.Fetcharr.apiRoot;

function isRelative(ajaxOptions) {
  return !absUrlRegex.test(ajaxOptions.url);
}

function addRootUrl(ajaxOptions) {
  ajaxOptions.url = apiRoot + ajaxOptions.url;
}

function addApiKey(ajaxOptions) {
  ajaxOptions.headers = ajaxOptions.headers || {};
  ajaxOptions.headers['X-Api-Key'] = window.Fetcharr.apiKey;
}

function addUIHeader(ajaxOptions) {
  ajaxOptions.headers = ajaxOptions.headers || {};
  ajaxOptions.headers['X-Fetcharr-Client'] = true;
}

function addContentType(ajaxOptions) {
  if (
    ajaxOptions.contentType == null &&
    ajaxOptions.dataType === 'json' &&
    (ajaxOptions.method === 'PUT' || ajaxOptions.method === 'POST' || ajaxOptions.method === 'DELETE')) {
    ajaxOptions.contentType = 'application/json';
  }
}

export default function createAjaxRequest(originalAjaxOptions) {
  const requestXHR = new window.XMLHttpRequest();
  let aborted = false;
  let complete = false;

  function abortRequest() {
    if (!complete) {
      aborted = true;
      requestXHR.abort();
    }
  }

  const ajaxOptions = { dataType: 'json', ...originalAjaxOptions };

  if (isRelative(ajaxOptions)) {
    addRootUrl(ajaxOptions);
    addApiKey(ajaxOptions);
    addUIHeader(ajaxOptions);
    addContentType(ajaxOptions);
  }

  const request = $.ajax({
    xhr: () => requestXHR,
    ...ajaxOptions
  }).then(null, (xhr, textStatus, errorThrown) => {
    xhr.aborted = aborted;

    return $.Deferred().reject(xhr, textStatus, errorThrown).promise();
  }).always(() => {
    complete = true;
  });

  return {
    request,
    abortRequest
  };
}
