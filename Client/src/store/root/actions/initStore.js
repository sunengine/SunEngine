import {consoleTokens} from 'sun'
import {hasLongToken} from 'sun'
import {consoleInit} from 'sun'
import {removeTokens} from 'sun'


export default async function (context) {

  console.info("%cStart init store", consoleInit);

  context.state.isInitialized = false;
  context.state.initializeError = false;

  if (hasLongToken()) {
    await context.dispatch('loadMyUserInfo')
      .catch(() => {
        console.error('%cTokens removed', consoleTokens);
        removeTokens();
      });
  }


  try {
    await context.dispatch('loadAllCategories');

    await context.dispatch('registerAllLayouts');

    await context.dispatch('registerAllComponentsTypes');

    await context.dispatch('loadAllComponents');

    await context.dispatch('setAllRoutes');

    await context.dispatch('loadAllMenuItems');

    context.state.isInitialized = true;
    context.state.initializeError = false;

  } catch (error) {

    console.error(error);

    context.state.initializeError = true;
    context.state.isInitialized = false;
  }
}
