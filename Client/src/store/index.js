import Vue from 'vue'
import Vuex from 'vuex'

import {authModule as auth} from 'sun'
import {categoriesModule as categories} from 'sun'
import {componentsModule as components} from 'sun'
import {menuModule as menu} from 'sun'
import {adminModule as admin} from 'sun'
import {layoutsModule as layouts} from 'sun'
import {rootModule} from 'sun'
import {setStore} from 'sun'
import {initLongTokenFromLocalStorage} from  'sun'



Vue.use(Vuex);

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation
 */


export default function (/* { ssrContext } */) {

  const store = new Vuex.Store({
    ...rootModule,
    modules: {
      admin,
      auth,
      categories,
      components,
      layouts,
      menu
    }
  });

  setStore(store);

  initLongTokenFromLocalStorage(store);

  return store;
}




