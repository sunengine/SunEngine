import state from 'store/auth/state'
import loadMyUserInfo from 'store/auth/actions/loadMyUserInfo'
import initUserFromLocalStorage from 'store/auth/actions/initUserFromLocalStorage'
import login from 'store/auth/actions/login'
import logout from 'store/auth/actions/logout'
import clearAllUserRelatedData from 'store/auth/mutations/clearAllUserRelatedData'
import setUserData from 'store/auth/mutations/setUserData'
import setUserInfo from 'store/auth/mutations/setUserInfo'



export default {
  state,
  actions: {
    loadMyUserInfo,
    initUserFromLocalStorage,
    login,
    logout
  },
  mutations: {
    clearAllUserRelatedData,
    setUserData,
    setUserInfo,
  },
}
