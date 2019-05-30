import {consoleInit} from 'sun'


export default async function(context) {

  console.info("%cStart init store", consoleInit);

  try {


    const request1 = context.dispatch('loadAllCategories');

    const request2 = context.dispatch('loadAllMenuItems');

    const requests = [request1, request2];

    if (context.state.auth.tokens) {
      const request3 = context.dispatch('loadMyUserInfo');
      requests.push(request3);
    }

    await Promise.all(requests);

    await context.dispatch('registerLayouts');

    await context.dispatch('setAllRoutes');

    context.state.isInitialized = true;

  } catch {

    context.state.initializeError = true;
  }
}
