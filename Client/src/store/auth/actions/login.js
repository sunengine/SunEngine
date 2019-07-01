
export default async function (context, userData) {

  await context.dispatch('request',
    {
      url: "/Auth/Login",
      data: {
        nameOrEmail: userData.nameOrEmail,
        password: userData.password
      }
    }).then(async () => {

    await context.dispatch('loadMyUserInfo');

    await context.dispatch('loadAllCategories');

    await context.dispatch('setAllRoutes');

    await context.dispatch('loadAllMenuItems');

  });
}
