import {request} from 'sun'
import {Api} from 'sun'

export default function (context, data) {

  return request(
    Api.Personal.GetMyUserInfo,
    {
      skipLock: data?.skipLock
    }).then(response => {
    context.commit('setUserInfo', response.data);
  });
}
