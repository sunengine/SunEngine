import {getAllCategories} from 'sun'
import {getCategory} from 'sun'
import {getLayout} from 'sun'
import {registerLayout} from 'sun'
import {prepareAllCategories} from 'sun'
import {categoriesState} from 'sun'

export default {
  //namespaced: true,
  state: categoriesState,
  getters : {
    getCategory,
    getLayout
  },
  mutations: {
    prepareAllCategories,
    registerLayout
  },
  actions: {
    getAllCategories
  }
}

