import Vue from 'vue'
import Vuex from 'vuex'

import auth from './auth'
import request from "./request";
import categories from "store/categories";
import {getTokens, makeUserDataFromTokens} from "services/tokens";
import {consoleInitStyle} from "services/consoleStyles";
//import extensions from './extensions'

Vue.use(Vuex);

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation
 */

export var store;

export default function (/* { ssrContext } */) {
  store = new Vuex.Store({
    state: {
      isInitialized: false,
      initializeError: false
    },
    actions: {
      request,
      async init() {

        console.info("%cStartInit", consoleInitStyle);

        try {
          initUser(this);

          this.state.auth.user && await this.dispatch('getMyUserInfo');

          !this.state.categories.all && await this.dispatch('getAllCategories');

          //await this.dispatch('getAndSetAllExtensions');

          this.state.isInitialized = true;
        } catch (x) {
          console.error("error", x);
          this.state.initializeError = true;
        }
      },
    },
    modules: {
      auth,
      categories,
      //extensions
    }
  });

  return store;
}

function initUser(store) {
  const tokens = getTokens();

  store.state.auth.tokens = tokens;

  if (tokens) {
    const userData = makeUserDataFromTokens(tokens);
    store.commit('setUserData', userData);

    console.info('User restored from localStorage', userData);
  }

}

