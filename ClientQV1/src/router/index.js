import Vue from 'vue'
import VueRouter from 'vue-router'


import auth from './auth'
import account from './account'
import misc from './misc'
import personal from './personal';
import admin from './admin';
import site from './site'
import ssr from './ssr';


Vue.use(VueRouter);

/*
 * If not building with SSR mode, you can
 * directly export the Router instantiation
 */

export var router;

export default function (/* { store, ssrContext } */) {
  router = new VueRouter({
    scrollBehavior: () => ({ y: 0 }),
    routes: [...auth,...account,...misc,...personal,...admin,...site,...ssr],


    // Leave these as is and change from quasar.conf.js instead!
    // quasar.conf.js -> build -> vueRouterMode
    // quasar.conf.js -> build -> publicPath
    mode: process.env.VUE_ROUTER_MODE,
    base: process.env.VUE_ROUTER_BASE
  });



  return router;
}
