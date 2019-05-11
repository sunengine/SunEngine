import {categoriesState} from 'sun'
import {getCategory} from 'sun'
import {setCategories} from 'sun'
import {getAllCategories} from 'sun'
import {getLayout} from 'sun'
import {registerLayout} from 'sun'

export default {
  //namespaced: true,
  state: categoriesState,
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
