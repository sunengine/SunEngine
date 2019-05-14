export default class Category {
  getRoute() {
    return this.route;
  }

  getMaterialRoute(idOrName, hash) {
    if(!this.route)
      return;

    let route = Object.assign({}, this.getRoute());

    route.name += "-mat";
    route.params.idOrName = idOrName.toString();

    if (hash)
      route.hash = hash;

    return route;
  }
}
