import {consoleInit} from 'sun'
import {makeRoutesFromLayouts, pageNotFoundRoute, router} from 'sun'
import {registerLayouts} from 'sun'
import {getTokens, makeUserDataFromTokens} from 'sun'
import {store} from 'sun'


export default async function () {

  console.info("%cStartInit", consoleInit);

  if (store.state.auth.tokens)
    await store.dispatch('getMyUserInfo');

  try {
    await this.dispatch('loadAllCategories');

    await this.dispatch('loadAllMenuItems');


    registerLayouts(store);

    const routes = makeRoutesFromLayouts(store);
    const router1 = router;
    router1.addRoutes(routes);
    router1.addRoutes(pageNotFoundRoute);

    this.state.isInitialized = true;
  } catch (x) {
    console.error("error", x);
    this.state.initializeError = true;
  }
}


