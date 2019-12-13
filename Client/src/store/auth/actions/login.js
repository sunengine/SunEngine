import {request} from 'sun'
import {Api} from 'sun'

export default function (context, userData) {

  return request(
    Api.Auth.Login,
    {
      nameOrEmail: userData.nameOrEmail,
      password: userData.password,
      skipLock: userData?.skipLock
    }
  ).then(async () => {
    await context.dispatch('loadMyUserInfo');
    await context.dispatch('loadAllCategories');
    await context.dispatch('setAllRoutes');
    await context.dispatch('loadAllMenuItems');
  });
}
