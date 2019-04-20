import admin from './admin';
import main from './main';
import errors from './errors.js';
import site from 'site/i18n/ru.js';


export default {
  ...main,
  Admin: {...admin},
  Errors: {...errors},
  ...site
}
