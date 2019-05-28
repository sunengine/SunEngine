import VueRouter from 'vue-router'

import {consoleInit, router} from 'sun'
import {routes} from 'sun'
import {pageNotFoundRoute} from 'sun'



export default async function (context) {

  const routesPlus = await context.dispatch('makeLayoutsRoutes');

  const allRoutes = [...routes, ...routesPlus, ...pageNotFoundRoute];

  const tmpRouter = new VueRouter({
    mode: 'history',
    routes: allRoutes
  });

  router.matcher = tmpRouter.matcher;

  console.info("%cRoutes registered", consoleInit, config.Log.InitExtended ? allRoutes : '');

}
