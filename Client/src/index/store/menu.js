import state from 'store/menu/state'
import loadAllMenuItems from 'store/menu/actions/loadAllMenuItems'
import getMenu from 'store/menu/getters/getMenu'
import prepareAllMenuItems from 'store/menu/mutations/prepareAllMenuItems'


export default {
  state,
  actions: {
    loadAllMenuItems
  },
  getters: {
    getMenu
  },
  mutations: {
    prepareAllMenuItems
  }
}
