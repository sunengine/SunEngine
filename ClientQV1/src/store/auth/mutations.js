import imagePath from "services/imagePath.js";
import Vue from "vue";
import {setToken} from "services/token";
import {store} from "store";
import {removeToken} from "services/token";

export function makeLogin(state, data) {

  Object.assign(state, data);

  if (data.isPermanentLogin) {
    setToken(data.tokens);
  }

}

export function makeLogout(state) {
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
