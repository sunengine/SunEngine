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
    const request1 = context.dispatch('loadMyUserInfo');
    const request2 = context.dispatch('loadAllCategories');
    const request3 = context.dispatch('loadAllMenuItems');
    await Promise.all([request1, request2, request3]);
    await context.dispatch('setAllRoutes');
  });
}
