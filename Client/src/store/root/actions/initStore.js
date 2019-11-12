import {consoleTokens} from 'sun'
import {hasLongToken} from 'sun'
import {consoleInit} from 'sun'
import {removeTokens} from 'sun'
import {InitializeState} from 'sun'


export default async function (context) {

  console.info("%cStart init store", consoleInit);

  context.state.initializeState = InitializeState.Running;

  if (hasLongToken())
    await context.dispatch('loadMyUserInfo');

  try {
    await context.dispatch('loadAllCategories');

    await context.dispatch('registerAllLayouts');

    await context.dispatch('registerAllComponentsTypes');

    await context.dispatch('loadAllComponents');

    await context.dispatch('setAllRoutes');

    await context.dispatch('loadAllMenuItems');

    context.state.initializeState = InitializeState.Done;

  } catch (error) {

    console.error(error);

    context.state.initializeState = InitializeState.Error;
  }
}
