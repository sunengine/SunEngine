import Vue from 'vue'
import VueRouter from 'vue-router'

import {routes} from 'sun'
import {routeHasAccess} from'sun'


Vue.use(VueRouter);

/*
 * If not building with SSR mode, you can
 * directly export the Router instantiation
 */

export var router;

export default function ({ store, ssrContext }) {

  router = new VueRouter({
    scrollBehavior: () => ({y: 0}),
    routes,

    // Leave these as is and change from quasar.conf.js instead!
    // quasar.conf.js -> build -> vueRouterMode
    // quasar.conf.js -> build -> publicPath
    mode: process.env.VUE_ROUTER_MODE,
    base: process.env.VUE_ROUTER_BASE
  });

  router.beforeEach((to, from, next) => {
    if (!routeHasAccess(to)) {
      router.push({name: 'Home'});
      return;
    }

    next();
  });

  return router;
}



import accountRoutes from './accountRoutes'
import adminRoutes from './adminRoutes'
import authRoutes from './authRoutes'
import makeRoutesFromLayouts from './makeRoutesFromLayouts'
import miscRoutes from './miscRoutes'
import personalRoutes from './personalRoutes'
import pageNotFoundRoute from './pageNotFoundRoute'

export * from './makeSections'

export {
  accountRoutes,
  adminRoutes,
  authRoutes,
  makeRoutesFromLayouts,
  miscRoutes,
  personalRoutes,
  pageNotFoundRoute
}
