import registerLayouts from './registerLayouts'
import request from './request'
import {store} from './index'
import auth from './auth/auth'
import categories from './categories/categories'

export * from './auth'
export * from './categories'

export {
  store,
  auth,
  categories,
  registerLayouts,
  request
}
