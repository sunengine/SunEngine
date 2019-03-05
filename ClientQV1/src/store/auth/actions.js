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

    const data = makeUserDataFromTokens(context.state.tokens);

    data.isPermanentLogin = !userData.notMyComputer;

    context.commit('setUserData', data);
    await context.dispatch('getAllCategories');
    await context.dispatch('getMyUserInfo');
    //await Promise.all([x1, x2]); TODO revive
  });
}


export async function logout(context) {
  return await context.dispatch('request', {url: '/Account/Logout'});
}


/*export async function resetToUnregistered(context) {
  context.commit('clearAllUserRelatedData');
  await context.dispatch('getAllCategories');
}*/


export async function getMyUserInfo(context) {
  await context.dispatch('request', {
    url: '/Personal/GetMyUserInfo',
  }).then(response => {
    context.commit('setUserInfo', response.data);
  }).catch(error => {
    console.log("error", error);
  });

}



