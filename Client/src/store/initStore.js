import {consoleInit} from 'sun'
import {makeRoutesFromLayouts, pageNotFoundRoute, router} from 'sun'
import {registerLayouts} from 'sun'
import {getTokens, makeUserDataFromTokens} from 'sun'
import {store} from 'sun'



export default async function init() {

  console.info("%cStartInit", consoleInit);

  await initUser(this);

  try {
    await this.dispatch('loadAllCategories');

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


async function initUser(store) {
  const tokens = getTokens();

  store.state.auth.tokens = tokens;

  if (tokens) {
    const userData = makeUserDataFromTokens(tokens);
    userData.isPermanentLogin = true;

    store.commit('setUserData', userData);

    console.info('%cUser restored from localStorage', consoleInit, userData);

    await this.dispatch('getMyUserInfo').catch(() => {
    });
  }
}
