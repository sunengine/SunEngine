import state from 'store/root/state'
import initStore from 'store/root/actions/initStore'
import setAllRoutes from 'store/root/actions/setAllRoutes'


import request from 'store/root/actions/request'


export default {
  state,
  actions: {
    initStore,
    request,
    setAllRoutes
  }
}
