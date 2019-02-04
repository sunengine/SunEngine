import Vue from 'vue'
import Vuex from 'vuex'

import auth from './auth'
import categories from './categories'
import options from './options'
import extensions from './extensions'

import {makeUserDataFromToken} from "services/auth";
import {getToken, setToken} from "services/token";

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

        return await request(data.url, data.data, data.sendAsJson, context.state.auth.tokens)
          .then(async data => {
            if(data.headers.tokensexpire) {
              store.commit('makeLogout');

              if(data.url !== "/Categories/GetAllCategoriesAndAccesses" )
                context.dispatch('getAllCategories');
            }
            else if (data.headers.tokens) {
              const tokensJson = JSON.parse(data.headers.tokens);
              setToken(tokensJson);
              const userData = makeUserDataFromToken(tokensJson);
              Object.assign(context.state.auth, userData);

            }

            return data;
          });

      },
      async init() {
        try {
          console.log("StartInit");

          await getAllCategories(this);

          await getMyUserInfo(this);

          await initExtensions(this);

          this.state.isInitialized = true;

        } catch (x) {
          console.error("error", x);
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
  const tokens = getToken();
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
