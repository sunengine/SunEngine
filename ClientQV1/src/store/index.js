import Vue from 'vue'
import Vuex from 'vuex'

/*import auth from './auth'
import categories from './categories'
import extensions from './extensions'*/

Vue.use(Vuex)

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation
 */

export default function (/* { ssrContext } */) {
  const Store = new Vuex.Store({
    modules: {
      //auth,
      //categories,
      //extensions
    }
  });

  return Store
}
