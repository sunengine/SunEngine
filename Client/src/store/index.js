import Vue from 'vue'
import Vuex from 'vuex'

import auth from './auth'
import categories from './categories'
import extensions from './extensions'

import {makeUserDataFromToken} from "services/auth";
import {getToken, setToken} from "services/token";

import request from "services/request";
import {routeCheckAccess} from "../plugins/routeAccess";
import {router} from "../router";

import {setSessionTokens} from "services/request"


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
        return request(data.url, data.data, data.sendAsJson)
          .then(async rez => {
            if (rez.headers.tokensexpire) {
              store.commit('makeLogout');
              await getAllCategories(this);
            }
            return rez;
          }).catch(rez => {

            if (rez.response.headers.tokensexpire) {
              store.commit('makeLogout');
            }
            throw rez;
          })
      },
      async init() {

        console.log("StartInit");

        try {
          await getAllCategories(this);

          await getMyUserInfo(this);

          await initExtensions(this);

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
      extensions
    }
  });

  store = Store;

  initUser(store);

  return Store
}

function initUser(store) {
  const tokens = getToken();
  setSessionTokens(tokens);

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
