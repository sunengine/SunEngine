import {router, removeTokens} from 'sun'

export default async function logout(context) {
  await context.dispatch('request', {url: '/Auth/Logout'}).finally(
    () => {
      removeTokens();
      router.push({name: "Home"});
    }
  );
}
