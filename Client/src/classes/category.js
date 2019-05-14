export default class Category {
  getRoute() {
    return this.route;
  }

  getMaterialRoute(idOrName, hash) {
    let route = Object.assign({}, this.route);
    route.name += "-mat";
    route.params.idOrName = idOrName;
    if (hash) route.hash = hash;
    return route;
  }
}
