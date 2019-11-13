import {store} from 'sun'

export default function (route) {
  console.log("" + store.state.auth.roles);
  const rez = !route.meta?.roles || store.state.auth.roles.some(x => route.meta.roles.some(y => x === y));
  return rez;
}

