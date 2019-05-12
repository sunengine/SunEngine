import accountRoutes from './accountRoutes'
import adminRoutes from './adminRoutes'
import authRoutes from './authRoutes'
import mainRoutes from './mainRoutes'
import makeRoutesFromLayouts from './makeRoutesFromLayouts'
import miscRoutes from './miscRoutes'
import personalRoutes from './personalRoutes'
import pageNotFoundRoute from './pageNotFoundRoute'
import {router} from './index'

export * from './makeSections'

export {
  router,
  accountRoutes,
  adminRoutes,
  authRoutes,
  mainRoutes,
  makeRoutesFromLayouts,
  miscRoutes,
  personalRoutes,
  pageNotFoundRoute
}
