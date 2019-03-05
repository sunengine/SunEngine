import imagePath from "services/imagePath.js";
import Vue from "vue";
import {setTokens} from "services/tokens";
import {store} from "store";


export function setUserData(state, data) {

  Object.assign(state, data);

  if (data.isPermanentLogin) {
    setTokens(data.tokens);
  }

}

export function clearAllUserRelatedData(state) {
  state.tokens = null;
  state.isPermanentLogin = null;
  state.user = null;
  state.userInfo = null;
  state.userGroup = 'Unregistered';
  state.userGroups = ['Unregistered'];
  store.state.categories.root = null;
  store.state.categories.all = null;
}

export function setUserInfo(state, data) {
  const userInfo = {
    photo: imagePath(data.photo),
    avatar: imagePath(data.avatar),
    link: data.link
  };

  state.userInfo = userInfo;
}
