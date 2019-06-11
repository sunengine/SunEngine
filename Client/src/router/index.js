import Vue from 'vue'
import VueRouter from 'vue-router'

import {getTokens} from 'sun'
import {checkTokensUpdated} from 'sun'
import {setRouter} from 'sun'

import {consoleRequestStart, consoleGreyEnd, consoleTokens} from 'sun'



Vue.use(VueRouter);

/*
 * If not building with SSR mode, you can
 * directly export the Router instantiation
 */


export default function ({store, ssrContext}) {

  const router = new VueRouter({
    scrollBehavior: () => ({y: 0}),
    routes: [],

    // Leave these as is and change from quasar.conf.js instead!
    // quasar.conf.js -> build -> vueRouterMode
    // quasar.conf.js -> build -> publicPath
    mode: process.env.VUE_ROUTER_MODE,
    base: process.env.VUE_ROUTER_BASE
  });

  router.beforeEach(async (to, from, next) => {
    if (config.Log.MoveTo)
      console.info("%cMove to page%c" + config.SiteUrl.substring(config.SiteSchema.length) + to.path, consoleRequestStart, consoleGreyEnd, to);

    if (checkTokensUpdated()) {
      console.info('%cReload user credentials', consoleTokens);

      store.commit('clearAllUserRelatedData');

      const tokens = getTokens();
      store.state.auth.longToken = tokens?.longToken;

      if(store.state.auth.longToken)
        await store.dispatch('loadMyUserInfo');

      await store.dispatch('loadAllCategories');
      await store.dispatch('loadAllMenuItems');
      await store.dispatch('setAllRoutes');
    }


    next();

  });

  setRouter(router);

  return router;
}



