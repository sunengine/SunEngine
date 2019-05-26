export registerLayouts from './registerLayouts'
export request from './request'
export initStore from './initStore'
export initUser from './initUser'
export adminModule from './admin'
export authModule from './auth'
export categoriesModule from './categories'
export menuModule from './menu'



export var store;

export function setStore(store1) {
  if(!store)
    store = store1;
}
