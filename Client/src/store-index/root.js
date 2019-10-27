import rootState from 'store/root/rootState'
import initStore from 'store/root/actions/initStore'
import setAllRoutes from 'store/root/actions/setAllRoutes'
import {isInitialized,initializeError} from 'store/root/getters'


export default {
  state: rootState,
  getters: {
    isInitialized,
    initializeError
  },
  actions: {
    initStore,
    setAllRoutes
  }
}
