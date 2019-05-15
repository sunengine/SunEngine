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

  const whiteSpace = "    ";
  console.info("%cRoutes registered:", consoleInit,"\n\n"+whiteSpace + routes.map(x => `${fill(x.name)} ${x.path}`).join("\n"+whiteSpace));

  return routes;

  function fill(str) {
    let dist = 35 - str.length;
    if(dist <= 5)
      dist = 5;
    return str + ' '.repeat(dist);
  }
}
