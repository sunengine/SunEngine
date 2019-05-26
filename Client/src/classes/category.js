import { extend } from 'quasar'


export default class Category {
  getRoute() {
    return this.route;
  }

  getMaterialRoute(idOrName, hash) {

    if(!this.route)
      return;

    let rezRoute = extend(true, {}, this.getRoute());

    rezRoute.name += "-mat";
    if(!rezRoute.params)
      rezRoute.params = {};

    rezRoute.params.idOrName = idOrName.toString();

    if (hash)
      rezRoute.hash = hash;

    return rezRoute;
  }
}
