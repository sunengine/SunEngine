import storeState from 'store/root/state'
import initStore from 'store/root/actions/initStore'
import registerLayouts from 'store/root/actions/registerLayouts'
import request from 'store/root/actions/request'


export default {
  state: storeState,
  actions: {
    initStore,
    registerLayouts,
    request
  }
}
