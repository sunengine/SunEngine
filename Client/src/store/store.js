export registerLayouts from './registerLayouts'
export request from './request'
export initStore from './initStore'
export authModule from './auth'
export categoriesModule from './categories'
export menuModule from './menu'



export var store;

export function setStore(store1) {
  if(!store)
    store = store1;
}
