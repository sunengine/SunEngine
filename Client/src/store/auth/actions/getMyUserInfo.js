
export default async function getMyUserInfo(context) {
  await context.dispatch('request', {
    url: '/Personal/GetMyUserInfo',
  }).then(response => {
    context.commit('setUserInfo', response.data);
  });
}
