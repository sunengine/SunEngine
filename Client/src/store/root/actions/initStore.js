import {removeTokens} from 'sun'
import {hasLongToken} from 'sun'
import {consoleInit} from 'sun'


export default async function (context) {

  console.info("%cStart init store", consoleInit);

  if(hasLongToken())
    await context.dispatch('loadMyUserInfo').catch(() => {
      console.error("removeTokens");
      removeTokens();
    });

  try {
    await context.dispatch('loadAllCategories');

    await context.dispatch('registerLayouts');

    await context.dispatch('setAllRoutes');

    await context.dispatch('loadAllMenuItems');

    context.state.isInitialized = true;

  } catch(error) {

    console.error(error);

    context.state.initializeError = true;
  }
}
