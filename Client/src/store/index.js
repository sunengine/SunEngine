import Vue from 'vue'
import Vuex from 'vuex'

import {authModule} from 'sun'
import {categoriesModule} from 'sun'
import {request} from 'sun'
import {initStore} from 'sun'


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
      initStore
    },
    modules: {
      auth: authModule,
      categories: categoriesModule
    }
  });

  return store;
}





