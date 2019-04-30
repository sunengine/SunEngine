import admin from './admin';
import components from './components';
import errors from './errors.js';

export default {
  ...components,
  Admin: {...admin},
  Errors: {...errors},
}
