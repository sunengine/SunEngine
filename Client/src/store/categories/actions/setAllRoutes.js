import VueRouter from 'vue-router'

import {consoleInit, router} from 'sun'
import {routes} from 'sun'
import {pageNotFoundRoute} from 'sun'
import {routeHasAccess} from 'sun'


export default async function (context) {

  const routesPlus = await context.dispatch('makeLayoutsRoutes');

  const allRoutes = [...routes, ...routesPlus, ...pageNotFoundRoute];

  const userRoutes = allRoutes.filter(x => routeHasAccess(x));

  const tmpRouter = new VueRouter({
    mode: 'history',
    routes: userRoutes
  });

  router.matcher = tmpRouter.matcher;
  router.addRoutes([]);

  console.info("%cRoutes registered", consoleInit, config.Log.InitExtended ? userRoutes : '');

}
