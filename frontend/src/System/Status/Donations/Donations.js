import React, { Component } from 'react';
import FieldSet from 'Components/FieldSet';
import Link from 'Components/Link/Link';
import translate from 'Utilities/String/translate';
import styles from '../styles.css';

class Donations extends Component {

  //
  // Render

  render() {
    return (
      <FieldSet legend={translate('Donations')}>
        <div className={styles.logoContainer} title="Radarr">
          <Link to="https://radarr.video/donate">
            <img
              className={styles.logo}
              src={`${window.Fetcharr.urlBase}/Content/Images/Icons/logo-radarr.png`}
            />
          </Link>
        </div>
        <div className={styles.logoContainer} title="Lidarr">
          <Link to="https://lidarr.audio/donate">
            <img
              className={styles.logo}
              src={`${window.Fetcharr.urlBase}/Content/Images/Icons/logo-lidarr.png`}
            />
          </Link>
        </div>
        <div className={styles.logoContainer} title="Readarr">
          <Link to="https://readarr.com/donate">
            <img
              className={styles.logo}
              src={`${window.Fetcharr.urlBase}/Content/Images/Icons/logo-readarr.png`}
            />
          </Link>
        </div>
        <div className={styles.logoContainer} title="Fetcharr">
          <Link to="https://fetcharr.com/donate">
            <img
              className={styles.logo}
              src={`${window.Fetcharr.urlBase}/Content/Images/Icons/logo-fetcharr.png`}
            />
          </Link>
        </div>
        <div className={styles.logoContainer} title="Sonarr">
          <Link to="https://opencollective.com/sonarr">
            <img
              className={styles.logo}
              src={`${window.Fetcharr.urlBase}/Content/Images/Icons/logo-sonarr.png`}
            />
          </Link>
        </div>
      </FieldSet>
    );
  }
}

Donations.propTypes = {

};

export default Donations;
