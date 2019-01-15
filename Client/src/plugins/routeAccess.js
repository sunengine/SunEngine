import {store} from "store";
import {router} from "router"

export var routeHasAccess = function(route) {
  if (route.meta.roles) {
    if (!store.state.auth.user
      || !store.state.auth.user.userGroups.some(x => route.meta.roles.some(y => x == y))) {

      return false;
    }
  }

  return true;
}

export var routeCheckAccess = function(route) {
  if (!routeHasAccess(route)) {
    router.push({name: 'Home'});
  }
}
