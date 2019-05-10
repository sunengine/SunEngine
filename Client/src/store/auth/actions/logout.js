
export default async function logout(context) {
  return await context.dispatch('request', {url: '/Auth/Logout'});
}
