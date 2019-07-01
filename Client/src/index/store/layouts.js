import state from 'store/layouts/state'
import makeRoutesFromCategories from 'store/layouts/actions/makeRoutesFromCategories'
import registerLayouts from 'store/layouts/actions/registerLayouts'
import setAllRoutes from 'store/layouts/actions/setAllRoutes'
import getLayout from 'store/layouts/getters/getLayout'
import registerLayout from 'store/layouts/mutations/registerLayout'


export default {
  state,
  actions: {
    makeRoutesFromCategories,
    registerLayouts,
    setAllRoutes
  },
  getters: {
    getLayout
  },
  mutations: {
    registerLayout
  }
}
