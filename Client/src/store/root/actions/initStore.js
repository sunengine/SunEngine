import {consoleTokens} from 'sun'
import {hasLongToken} from 'sun'
import {consoleInit, removeTokens} from 'sun'


export default async function (context) {

  console.info("%cStart init store", consoleInit);

  if(hasLongToken())
    await context.dispatch('loadMyUserInfo').catch(() => {
      console.error('%cTokens removed', consoleTokens);
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
