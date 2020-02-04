import authState from "store/auth/authState";
import loadMyUserInfo from "store/auth/actions/loadMyUserInfo";
import login from "store/auth/actions/login";
import logout from "store/auth/actions/logout";
import isAdmin from "store/auth/getters/isAdmin";
import clearAllUserRelatedData from "store/auth/mutations/clearAllUserRelatedData";
import setUserInfo from "store/auth/mutations/setUserInfo";

export default {
	state: authState,
	actions: {
		loadMyUserInfo,
		login,
		logout
	},
	getters: {
		isAdmin
	},
	mutations: {
		clearAllUserRelatedData,
		setUserInfo
	}
};
