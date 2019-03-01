import Vue from 'vue'
import VueRouter from 'vue-router'

import routes from './routes'

import routesCore from './routesCore'
/*import routesSite from './routesSite'
import routesPersonal from './routesPersonal'
import routesAdmin from './routesAdmin'*/

Vue.use(VueRouter);

/*
 * If not building with SSR mode, you can
 * directly export the Router instantiation
 */

export var router;

export default function (/* { store, ssrContext } */) {
  router = new VueRouter({
    scrollBehavior: () => ({ y: 0 }),
    routes: [...routes],


    // Leave these as is and change from quasar.conf.js instead!
    // quasar.conf.js -> build -> vueRouterMode
    // quasar.conf.js -> build -> publicPath
    mode: process.env.VUE_ROUTER_MODE,
    base: process.env.VUE_ROUTER_BASE
  });

  return router;
}
