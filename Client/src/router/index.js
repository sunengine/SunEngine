import Vue from 'vue'
import VueRouter from 'vue-router'

import routesCore from './routesCore'
import routesSite from './routesSite'
import routesPersonal from './routesPersonal'
import routesAdmin from './routesAdmin'



import {store} from 'store'
import {routeHasAccess} from "plugins/routeAccess"

Vue.use(VueRouter)

/*
 * If not building with SSR isReadMode, you can
 * directly export the Router instantiation
 */

export var router;

export default function (/* { store, ssrContext } */) {
  const Router = new VueRouter({
    scrollBehavior: () => ({y: 0}),
    routes: [...routesCore,...routesPersonal,...routesAdmin, ...routesSite],

    // Leave these as is and change from quasar.conf.js instead!
    // quasar.conf.js -> build -> vueRouterMode
    // quasar.conf.js -> build -> publicPath
    mode: 'history',
    isReadMode: process.env.VUE_ROUTER_MODE,
    base: process.env.VUE_ROUTER_BASE
  });

  router = Router;

  router.beforeEach((to, from, next) => {
    if(!routeHasAccess(to)) {
      router.push({name: 'Home'});
      return;
    }
    router.$prevRoute = from;
    next();
  });

  router.$goBack = function () {
    if (router.$prevRoute && !router.$prevRoute?.meta?.notReturnable) {
      router.push(router.$prevRoute.fullPath);
    }
    else {
      router.push({name: 'Home'});
    }
  };

  return Router;
}



