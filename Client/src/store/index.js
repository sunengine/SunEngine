import Vue from 'vue'
import Vuex from 'vuex'

import {authModule as auth} from 'sun'
import {categoriesModule as categories} from 'sun'
import {menuModule as menu} from 'sun'
import {adminModule as admin} from 'sun'
import {rootModule} from 'sun'



Vue.use(Vuex);

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation
 */


var store;


export default function (/* { ssrContext } */) {

  store = new Vuex.Store({
    ...rootModule,
    modules: {
      admin,
      auth,
      categories,
      menu
    }
  });

  store.dispatch("initUserFromLocalStorage");

  return store;
}


export {store};



