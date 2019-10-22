import rootState from 'store/root/rootState'
import initStore from 'store/root/actions/initStore'
import setAllRoutes from 'store/root/actions/setAllRoutes'


export default {
  state: rootState,
  actions: {
    initStore,
    setAllRoutes
  }
}
