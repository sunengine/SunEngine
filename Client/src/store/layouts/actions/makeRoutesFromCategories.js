
export default function (context) {

  let routes = [];
  for (const categoryName in context.rootState.categories.all) {
    const category = context.rootState.categories.all[categoryName];
    if (category.layoutName) {
      const layout = context.getters.getLayout(category.layoutName);

      routes.push(...layout.getRoutes(category));

      layout.setCategoryRoute(category);
    }
  }

  return routes;
}
