import {consoleInit} from 'sun'


export default async function (context) {

  console.info("%cStart init store", consoleInit);

  if (context.state.auth.tokens)
    await context.dispatch('getMyUserInfo');

  try {
    await context.dispatch('loadAllCategories');

    await context.dispatch('loadAllMenuItems');

    await context.dispatch('registerLayouts');

    await context.dispatch('setAllRoutes');

    context.state.isInitialized = true;

  } catch {

    context.state.initializeError = true;
  }
}
