import {router} from 'sun'

export default async function logout(context) {
  router.push({name: "Home"});
  return await context.dispatch('request', {url: '/Auth/Logout'});
}
