import Vue from 'vue'
import Vuex from 'vuex'

import auth from './auth'
import request, {setSessionTokens} from "../../../Client/src/services/request";
import categories from "store/categories";
import {getToken} from "services/token";
import {makeUserDataFromToken} from "services/auth";
//import extensions from './extensions'

Vue.use(Vuex)

/*
 * If not building with SSR mode, you can
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
              await store.dispatch('doLogout');
            }
            return rez;
          }).catch(async rez => {
            if (rez.response.headers.tokensexpire) {
              await store.dispatch('doLogout');
            }
            throw rez;
          })
      },
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
