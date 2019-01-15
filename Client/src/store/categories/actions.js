export async function getAllCategories(context) {
  await context.dispatch("request", {url: "/Categories/GetAllCategoriesAndAccesses"}).then(response => {
    console.log("GetAllCategories");
    context.commit('setCategories', response.data);
  })

}
