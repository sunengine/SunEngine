export default {
	Global: {
	    link : "link",
		dialog: {
			cancel: "Cancel",
			yes: "Yes",
			ok: "Ok"
		},
		btn: {
			create: "Create",
			save: "Save",
			cancel: "Cancel",
			delete: "Delete"
		},
		validation: {
			userName: {
				required: "Enter user name",
				minLength: "Minimal user name length is {minLength}",
				maxLength: "Maximal user name length is {maxLength}",
				nameInDb: "This name is already used",
				allowedUserNameCharacters: "User name can  '{allowedUserNameCharacters}'"
			},
			email: {
				required: "Enter email",
				emailSig: "Invalid email",
				maxLength: "Maximal email length is {maxLength}"
			},
			password: {
				passwordOld: "Required enter old password",
				required: "Enter password",
				requiredLength: "Minimal password length - {requiredLength}",
				requiredUniqueChars:
					"Password must contain minimum {requiredUniqueChars} different chars",
				requireDigit: "Password must contain digit",
				requireLowercase: "Password must contain lowercase letter",
				requireUppercase: "Password must contain uppercase letter",
				requireNonAlphanumeric:
					"Password must contain non letter and digit symbol like '@','^','+'..."
			},
			emailSig: "Incorrect email",
			jsonFormatError: "Json validation error",
			fileSizeLimit:
				"File '{fileName}' is to big. MAx file size is {maxSize} megabytes."
		},
		errorNotify: "Error",
		submitting: "Sending...",
		successNotify: "Operation success",
		apiError: "Api error",
		refresh: "Refresh"
	},
	App: {
		loading: "Loading...",
		canNotConnectApi: "Can`t connect to API."
	},
	MyEditor: {
		uploadImages: "Upload image"
	},
	LayoutNames: {
		Articles: "Articles",
		Articles1: "Articles with 1 level subcategories",
		Articles2: "Articles with 2 level subcategories",
		Blog: "Blog",
		Forum0: "Forum thread",
		Forum1: "Forum with 1 level threads",
		Forum2: "Forum with 2 level threads"
	}
};
