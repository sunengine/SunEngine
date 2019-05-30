import {makeUserDataFromTokens} from 'sun'

export default async function(context, userData) {

  await context.dispatch('request',
    {
      url: "/Auth/Login",
      data: {
        nameOrEmail: userData.nameOrEmail,
        password: userData.password
      }
    }).then(async () => {

    const data = makeUserDataFromTokens(context.state.tokens);

    data.isPermanentLogin = !userData.notMyComputer;

    context.commit('setUserData', data);

    await Promise.all([
      context.dispatch('loadAllCategories'),
      context.dispatch('loadMyUserInfo')
    ]);

    await context.dispatch('setAllRoutes');

    await context.dispatch('loadAllMenuItems');
  });
}
