export accountRoutes from 'router/accountRoutes'
export adminRoutes from 'router/adminRoutes'
export authRoutes from 'router/authRoutes'
export miscRoutes from 'router/miscRoutes'
export personalRoutes from 'router/personalRoutes'
export pageNotFoundRoute from 'router/pageNotFoundRoute'
export * from 'router/makeSections'


export var router;

export function setRouter(router1) {
  if(!router)
    router = router1;
}



