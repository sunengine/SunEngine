import {makeUserDataFromTokens} from 'sun'

export default async function doLogin(context, userData) {

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
    let request1 = context.dispatch('getAllCategories');
    let request2 = context.dispatch('getMyUserInfo');
    await Promise.all([request1, request2]);
  });
}
