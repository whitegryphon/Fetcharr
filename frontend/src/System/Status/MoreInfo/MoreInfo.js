import React, { Component } from 'react';
import DescriptionList from 'Components/DescriptionList/DescriptionList';
import DescriptionListItemDescription from 'Components/DescriptionList/DescriptionListItemDescription';
import DescriptionListItemTitle from 'Components/DescriptionList/DescriptionListItemTitle';
import FieldSet from 'Components/FieldSet';
import Link from 'Components/Link/Link';
import translate from 'Utilities/String/translate';

class MoreInfo extends Component {

  //
  // Render

  render() {
    return (
      <FieldSet legend={translate('MoreInfo')}>
        <DescriptionList>
          <DescriptionListItemTitle>{translate('HomePage')}</DescriptionListItemTitle>
          <DescriptionListItemDescription>
            <Link to="https://fetcharr.com/">fetcharr.com</Link>
          </DescriptionListItemDescription>

          <DescriptionListItemTitle>{translate('Wiki')}</DescriptionListItemTitle>
          <DescriptionListItemDescription>
            <Link to="https://wiki.servarr.com/fetcharr">wiki.servarr.com/fetcharr</Link>
          </DescriptionListItemDescription>

          <DescriptionListItemTitle>{translate('Reddit')}</DescriptionListItemTitle>
          <DescriptionListItemDescription>
            <Link to="https://reddit.com/r/fetcharr">r/fetcharr</Link>
          </DescriptionListItemDescription>

          <DescriptionListItemTitle>{translate('Discord')}</DescriptionListItemTitle>
          <DescriptionListItemDescription>
            <Link to="https://fetcharr.com/discord">fetcharr.com/discord</Link>
          </DescriptionListItemDescription>

          <DescriptionListItemTitle>{translate('Source')}</DescriptionListItemTitle>
          <DescriptionListItemDescription>
            <Link to="https://github.com/Fetcharr/Fetcharr/">github.com/Fetcharr/Fetcharr</Link>
          </DescriptionListItemDescription>

          <DescriptionListItemTitle>{translate('FeatureRequests')}</DescriptionListItemTitle>
          <DescriptionListItemDescription>
            <Link to="https://github.com/Fetcharr/Fetcharr/issues">github.com/Fetcharr/Fetcharr/issues</Link>
          </DescriptionListItemDescription>

        </DescriptionList>
      </FieldSet>
    );
  }
}

MoreInfo.propTypes = {

};

export default MoreInfo;
