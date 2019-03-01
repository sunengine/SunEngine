import Vue from 'vue'
import Vuex from 'vuex'

import auth from './auth'
import request from "./request";
import categories from "store/categories";
import {getTokens, makeUserDataFromToken} from "services/tokens";
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

        console.log("StartInit");

        try {
          await getAllCategories(this);

          await getMyUserInfo(this);

          //await initExtensions(this);

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

  initUser(store);

  return store;
}

function initUser(store) {
  const tokens = getTokens();

  store.state.auth.tokens = tokens;

  if (tokens) {
    const userData = makeUserDataFromToken(tokens);
    store.commit('makeLogin', userData);

    console.log('UserRestored');
  }
}

async function getAllCategories(store) {
  await store.dispatch('getAllCategories');
}

async function getMyUserInfo(store) {
  if (!store.state.auth.user)
    return;

  await store.dispatch('getMyUserInfo');
}

async function initExtensions(store) {
  await store.dispatch('getAndSetAllExtensions');
}
