import registerLayouts from './registerLayouts'
import request from './request'
import initStore from './initStore'
import authModule from './auth'
import categoriesModule from './categories'


export var store;

export function setStore(store1) {
  if(!store)
    store = store1;
}

export {
  authModule,
  initStore,
  categoriesModule,
  registerLayouts,
  request
}
