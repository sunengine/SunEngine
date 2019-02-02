import Vue from 'vue'
import Vuex from 'vuex'

import auth from './auth'
import categories from './categories'
import options from './options'
import extensions from './extensions'

import {makeUserDataFromToken} from "services/auth";
import {getToken} from "services/token";

import request from "services/request";


Vue.use(Vuex)

/*
 * If not building with SSR isReadMode, you can
 * directly export the Store instantiation
 */

export var store;

export default function (/* { ssrContext } */) {
  const Store = new Vuex.Store({
    state: {
      isInitialized: false,
      initializeError: false
    },
    actions: {
      async request(context, data) {
        return await request(data.url, data.data, data.sendAsJson, context.state.auth.tokens?.shortToken);
      },
      async init() {
        try {
          console.log("StartInit");

          await getAllCategories(this);

          await getMyUserInfo(this);

          initExtensions(this);

          this.state.isInitialized = true;
        } catch(x) {
          console.error("error",x);
          this.state.initializeError = true;
        }
      }
    },
    modules: {
      auth,
      categories,
      options,
      extensions
    }
  });

  store = Store;

  initUser(store);

  return Store
}

function initUser(store) {
  var token = getToken();
  if (token) {
    var userData = makeUserDataFromToken(token);
    store.commit('makeLogin', userData);
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

function initExtensions(store) {
  store.dispatch('getAndSetAllExtensions');
}
