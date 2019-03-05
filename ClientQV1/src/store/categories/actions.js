export async function getAllCategories(context) {
  console.log("GetAllCategories 0");
  await context.dispatch("request", {url: "/Categories/GetAllCategoriesAndAccesses"})
    .then(response => {
      console.info("GetAllCategories");
      context.commit('setCategories', response.data);
    }).catch(error => {
      console.log("error", error);
    });
}
