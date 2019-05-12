import registerLayouts from './registerLayouts'
import request from './request'
import {store} from './index'
import categories from './categories/categories'

export * from './auth'
import auth from './auth/auth'

export {
  store,
  auth,
  categories,
  registerLayouts,
  request
}
