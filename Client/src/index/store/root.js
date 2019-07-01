import state from 'store/root/state'
import initStore from 'store/root/actions/initStore'
import request from 'store/root/actions/request'


export default {
  state,
  actions: {
    initStore,
    request
  }
}
