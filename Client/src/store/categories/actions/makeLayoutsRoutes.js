import {consoleInit} from 'sun'

export default function (context) {

  let routes = [];
  for (const categoryName in context.state.all) {
    const category = context.state.all[categoryName];
    if (category.layoutName) {
      const layout = context.getters.getLayout(category.layoutName);

      routes.push(...layout.getRoutes(category));

      layout.setCategoryRoute(category);
    }
  }

  return routes;
}
