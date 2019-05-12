import {store} from 'sun'
import {router} from 'sun'

export function routeHasAccess(route) {
  return !route.meta.roles ||
    store.state.auth.roles.some(x => route.meta.roles.some(y => x === y))
}

export function routeCheckAccess(route) {
  if (!routeHasAccess(route)) {
    router.push({name: 'Home'});
  }
}
