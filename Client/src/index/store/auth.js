import state from 'store/auth/state'
import loadMyUserInfo from 'store/auth/actions/loadMyUserInfo'
import login from 'store/auth/actions/login'
import logout from 'store/auth/actions/logout'
import clearAllUserRelatedData from 'store/auth/mutations/clearAllUserRelatedData'
import setUserInfo from 'store/auth/mutations/setUserInfo'



export default {
  state,
  actions: {
    loadMyUserInfo,
    login,
    logout
  },
  mutations: {
    clearAllUserRelatedData,
    setUserInfo,
  },
}
