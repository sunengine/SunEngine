import {setTokens} from 'sun';

export default async function (context, userData) {

  context.state.isPermanentLogin = !userData.notMyComputer;

  await context.dispatch('request',
    {
      url: "/Auth/Login",
      data: {
        nameOrEmail: userData.nameOrEmail,
        password: userData.password
      }
    }).then(async () => {

    await context.dispatch('loadMyUserInfo');

    await context.dispatch('loadAllCategories');

    await context.dispatch('setAllRoutes');

    await context.dispatch('loadAllMenuItems');

  }).catch((error) => {

    context.state.isPermanentLogin = null;

    throw error;

  });

}
