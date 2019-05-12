import accountRoutes from './accountRoutes'
import adminRoutes from './adminRoutes'
import authRoutes from './authRoutes'
import makeRoutesFromLayouts from './makeRoutesFromLayouts'
import miscRoutes from './miscRoutes'
import personalRoutes from './personalRoutes'
import pageNotFoundRoute from './pageNotFoundRoute'


export var router;

export * from './makeSections'

export {
  accountRoutes,
  adminRoutes,
  authRoutes,
  makeRoutesFromLayouts,
  miscRoutes,
  personalRoutes,
  pageNotFoundRoute
}
