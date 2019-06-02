import {consoleInit} from 'sun'


export default async function (context) {

  console.info("%cStart init store", consoleInit);

  try {
    const requests = [
      context.dispatch('loadAllCategories'),
      context.state.auth.tokens ? context.dispatch('loadMyUserInfo'): undefined];

    await Promise.all(requests);

    await context.dispatch('registerLayouts');

    await context.dispatch('setAllRoutes');

    await context.dispatch('loadAllMenuItems');

    context.state.isInitialized = true;

  } catch(error) {

    console.error(error);

    context.state.initializeError = true;
  }
}
