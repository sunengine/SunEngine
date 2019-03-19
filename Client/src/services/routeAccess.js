import {store} from "store";
import {router} from "router"

export var routeHasAccess = function(route) {
  return !route.meta.roles ||
    store.state.auth.roles.some(x => route.meta.roles.some(y => x === y))
};

export var routeCheckAccess = function(route) {
  if (!routeHasAccess(route)) {
    router.push({name: 'Home'});
  }
};
