import state from './state'
import getCategory from './getCategory'
import setCategories from './setCategoriesMutation'
import getAllCategories from './getAllCategoriesAction'
import getLayout from "./getLayout"
import registerLayout from './registerLayoutMutation'

export default {
  //namespaced: true,
  state: state,
  getters : {
    getCategory,
    getLayout
  },
  mutations: {
    setCategories,
    registerLayout
  },
  actions: {
    getAllCategories
  }
}
