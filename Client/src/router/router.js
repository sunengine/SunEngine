export accountRoutes from './accountRoutes'
export adminRoutes from './adminRoutes'
export authRoutes from './authRoutes'
export miscRoutes from './miscRoutes'
export personalRoutes from './personalRoutes'
export pageNotFoundRoute from './pageNotFoundRoute'
export * from './makeSections'


export var router;

export function setRouter(router1) {
  if(!router)
    router = router1;
}



