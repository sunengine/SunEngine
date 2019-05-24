import {consoleInit} from 'sun'

export default function (store) {

  let routes = [];
  const allCategories = store.state.categories.all;
  for (const categoryName in allCategories) {
    const category = allCategories[categoryName];
    if (category.layoutName) {
      const layout = store.getters.getLayout(category.layoutName);

      routes.push(...layout.getRoutes(category));

      layout.setCategoryRoute(category);
    }
  }

  console.info("%cRoutes registered", consoleInit, config.Log.InitExtended ? routes : '');

  return routes;
}
