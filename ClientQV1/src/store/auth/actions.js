import {loginRequest,makeUserDataFromToken} from "services/auth";
import {removeToken} from "services/token"
import {routeCheckAccess} from "services/routeAccess"
import {router} from 'router'

export async function doLogin(context, userData) {

  return await loginRequest(userData.nameOrEmail, userData.password)
    .then(async tokenData => {

      const data = makeUserDataFromToken(tokenData);

      data.permanent = !userData.notMyComputer;

      context.commit('makeLogin', data);

      let x1 = context.dispatch('getAllCategories');
      let x2 = context.dispatch('getMyUserInfo');
      await Promise.all( [x1,x2]);

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
    context.commit('setUserInfo',response.data);
  }).catch(error => {
   console.log("error",error);
  });

}



