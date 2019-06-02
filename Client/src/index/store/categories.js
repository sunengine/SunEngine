import state from 'store/categories/state'
import loadAllCategories from 'store/categories/actions/loadAllCategories'
import getCategory from 'store/categories/getters/getCategory'
import prepareAllCategories from 'store/categories/mutations/prepareAllCategories'




export default {
  state,
  actions: {
    loadAllCategories
  },
  getters: {
    getCategory
  },
  mutations: {
    prepareAllCategories
  }
};


