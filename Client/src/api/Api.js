export default {
	Blog: {
		GetPosts: "/Blog/GetPosts",
		GetPostsFromMultiCategories: "/Blog/GetPostsFromMultiCategories"
	},
	Profile: {
		BanUser: "/Profile/BanUser",
		UnBanUser: "/Profile/UnBanUser",
		GetProfile: "/Profile/GetProfile",
		SendPrivateMessage: "/Profile/SendPrivateMessage"
	},
	Personal: {
		SetMyLink: "/Personal/SetMyLink",
		CheckLinkInDb: "/Personal/CheckLinkInDb",
		CheckNameInDb: "/Personal/CheckNameInDb",
		SetMyName: "/Personal/SetMyName",
		SetMyProfileInformation: "/Personal/SetMyProfileInformation",
		GetMyProfileInformation: "/Personal/GetMyProfileInformation",
		RemoveMyAvatar: "/Personal/RemoveMyAvatar",
		GetMyBanList: "/Personal/GetMyBanList",
		GetMySessions: "/Personal/GetMySessions",
		RemoveMySessions: "/Personal/RemoveMySessions",
		GetMyUserInfo: "/Personal/GetMyUserInfo"
	},
	UploadImages: {
		UploadImage: "/UploadImages/UploadImage",
		UploadUserPhoto: "/UploadImages/UploadUserPhoto"
	},
	Materials: {
		Get: "/Materials/Get",
		Update: "/Materials/Update",
		Create: "/Materials/Create",
		Restore: "/Materials/Restore",
		Delete: "/Materials/Delete"
	},
	Comments: {
		Create: "/Comments/Create",
		GetMaterialComments: "/Comments/GetMaterialComments",
		MoveToTrash: "/Comments/MoveToTrash",
		Update: "/Comments/Update",
		Get: "/Comments/Get"
	},
	Forum: {
		GetThread: "/Forum/GetThread",
		GetNewTopics: "/Forum/GetNewTopics"
	},
	Auth: {
		Login: "/Auth/Login",
		Logout: "/Auth/Logout",
		Register: "/Auth/Register",
		CheckUserNameInDb: "/Auth/CheckUserNameInDb"
	},
	Captcha: {
		GetCaptchaKey: "/Captcha/GetCaptchaKey",
		CaptchaImage: "/Captcha/CaptchaImage"
	},
	Articles: {
		GetArticles: "/Articles/GetArticles",
		GetArticlesFromMultiCategories: "/Articles/GetArticlesFromMultiCategories"
	},
	Activities: {
		GetActivities: "/Activities/GetActivities"
	},
	Account: {
		ChangeEmail: "/Account/ChangeEmail",
		ChangePassword: "/Account/ChangePassword",
		ResetPasswordSetNew: "/Account/ResetPasswordSetNew",
		ResetPasswordSendEmail: "/Account/ResetPasswordSendEmail"
	},
	Menu: {
		GetAllMenuItems: "/Menu/GetAllMenuItems"
	},
	Components: {
		GetAllComponents: "/Components/GetAllComponents"
	},
	Categories: {
		GetAllCategoriesAndAccesses: "/Categories/GetAllCategoriesAndAccesses"
	},
	Pulse: {
		Version: "/Pulse/Version",
		PulseException: "/Pulse/PulseException"
	},
	Configuration: {
		GetDynamicConfig: "/Configuration/GetDynamicConfig"
	}
};
