export adminModule from './admin'
export authModule from './auth'
export categoriesModule from './categories'
export menuModule from './menu'
export rootModule from './root'



export var store;

export function setStore(store1) {
  if(!store)
    store = store1;
}
