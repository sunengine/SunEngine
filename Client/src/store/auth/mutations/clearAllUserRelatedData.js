import {store} from "../../index";

export default function clearAllUserRelatedData(state) {
  state.tokens = null;
  state.isPermanentLogin = null;
  state.user = null;
  state.userInfo = null;
  state.roles = ['Unregistered'];
  store.state.categories.root = null;
  store.state.categories.all = null;
}
