export default {
	ChangeLink: {
		title: "Edit user profile link",
		successNotify: "Token edited",
		link: "Link token",
		hintLink: "Profile link:",
		linkValidationInfo:
			"Token must consist of at least 3 characters 'a-z', '-', '0-9'. And contain at least one letter.",
		saveBtn: "@:Global.btn.save",
		validation: {
			minLength: "Token must be at least 3 characters",
			allowedChars: "Allowed only lower english letters and numbers",
			numberNotAllow: "Token must be contain letters",
			linkInDb: "This token already used"
		}
	},
	ChangeName: {
		title: "Change login",
		successNotify: "Login changed",
		nameValidationInfo:
			"Login can contain letters, numbers, space and '-' character, and must be at least 3 characters long.",
		saveBtn: "@:Global.btn.save",
		name: "Login",
		password: "Password",
		validation: {
			password: {
				required: "Enter password"
			},
			name: {
				required: "Enter login",
				minLength: "Login must be at least 3 characters",
				allowedChars: "Only allowed characters are allowed.",
				nameInDb: "Login already used"
			}
		}
	},
	EditInformation: {
		title: "Edit information",
		label: "Information about you on your profile page.",
		successNotify: "Information successfully saved",
		save: "@:Global.btn.save"
	},
	LoadPhoto: {
		title: "Change user`s avatar",
		maxPhotoSize: "Max photo size:",
		fileSizeLimit: "File size is too big",
		resetBtn: "Reset user`s avatar",
		uploadNewPhotoBtn: "Upload avatar",
		avatarDeletedSuccessNotify: "Avatar was deleted",
		avatarChangedSuccessNotify: "Avatar was changed"
	},
	MyBanList: {
		title: "Banned users",
		subTitle: "This users can not send you private messages",
		voidResult: "No users found in black list"
	},
	Sessions: {
		title: "Authorization sessions",
		deviceInfo: "Device and browser info",
		current: "Current",
		updateDate: "Update date",
		successNotify: "Sessions successfully deleted",
		logout: "Exit"
	},
	SettingsMenu: {
		goToProfile: "View profile",
		changeEmail: "Change email",
		changePassword: "Change password",
		changeLink: "Change link",
		changeName: "Change login",
		changeYourInformation: "Change information about you",
		changePhoto: "Change avatar",
		sessions: "Authorization sessions",
		banedUsersList: "Black list users"
	},
	SettingsPage: {
		title: "Account"
	},
	SettingsPanel: {
		title: "@:SettingsPage.title"
	}
};
