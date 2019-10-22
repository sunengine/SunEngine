export default function (context) {

  let routes = [];
  for (const componentName in context.state.allComponents) {
    const component = context.state.allComponents[componentName];
    const componentType = context.getters.getComponentType(component.type);
    routes.push(...componentType.getRoutes(component));
  }

  return routes;
}
