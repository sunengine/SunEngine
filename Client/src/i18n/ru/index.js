import admin from './admin';
import main from './main';
import site from 'site/i18n/ru.js';


export default {
  ...main,
  admin: {...admin},
  ...site
}
