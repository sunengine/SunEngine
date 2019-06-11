import {store} from 'sun';

export default function clearAllUserRelatedData(state) {
  state.longToken = null;
  state.isPermanentLogin = null;
  state.user = null;
  state.roles = ['Unregistered'];
  store.state.categories.root = null;
  store.state.categories.all = null;
  store.state.menu.namedMenuItems = null;
}
