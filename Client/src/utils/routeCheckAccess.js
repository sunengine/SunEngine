import {router} from 'sun'
import {routeHasAccess} from 'sun'

export default function(route) {
  if(!routeHasAccess(route)) {
    router.push({name: 'Home'});
    return false;
  }
  return true;
}
