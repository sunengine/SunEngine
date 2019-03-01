import {routeCheckAccess} from "services/routeAccess"
import {router} from 'router'
import {parseJwt, makeUserDataFromTokens} from 'services/tokens'


export async function doLogin(context, userData) {

  await context.dispatch('request',
    {
      url: "/Account/Login",
      data: {
        nameOrEmail: userData.nameOrEmail,
        password: userData.password
      }
    }).then(async () => {

    const data = makeUserDataFromTokens(context.tokens);

    data.isPermanentLogin = !userData.notMyComputer;

    context.commit('makeLogin', data);

    const x1 = context.dispatch('getAllCategories');
    const x2 = context.dispatch('getMyUserInfo');
    await Promise.all([x1, x2]);

  });
}


export async function doLogout(context) {

  context.commit('makeLogout');

  await context.dispatch('getAllCategories');
}


export async function getMyUserInfo(context) {
  await context.dispatch('request', {
    url: '/Personal/GetMyUserInfo',
  }).then(response => {
    context.commit('setUserInfo', response.data);
  }).catch(error => {
    console.log("error", error);
  });

}



