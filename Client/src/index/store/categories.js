import state from 'store/categories/state'
import loadAllCategories from 'store/categories/actions/loadAllCategories'
import makeLayoutsRoutes from 'store/categories/actions/makeLayoutsRoutes'
import setAllRoutes from 'store/categories/actions/setAllRoutes'
import getCategory from 'store/categories/getters/getCategory'
import getLayout from 'store/categories/getters/getLayout'
import prepareAllCategories from 'store/categories/mutations/prepareAllCategories'
import registerLayout from 'store/categories/mutations/registerLayout'




export default {
  state,
  actions: {
    loadAllCategories,
    makeLayoutsRoutes,
    setAllRoutes
  },
  getters: {
    getCategory,
    getLayout
  },
  mutations: {
    prepareAllCategories,
    registerLayout
  }
};


