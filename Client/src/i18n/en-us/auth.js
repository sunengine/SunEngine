export default {
	Login: {
		title: "Sign In",
		nameOrEmail: "Login or email",
		password: "Password",
		entering: "Sign In...",
		enterBtn: "Sign In",
		forgotPassword: "Forget password?",
		enterSuccess: "You logged in",
		validation: {
			nameOrEmail: {
				required: "Enter login or email"
			},
			password: {
				required: "Enter password"
			}
		}
	},
	LoginRegisterMenu: {
		enter: "Sign In",
		register: "Sign Up"
	},
	Register: {
		title: "Sign Up",
		userName: "Login",
		email: "Email",
		password: "Password",
		password2: "Repeat password",
		registerBtn: "@:Register.title",
		registering: "Sing Up...",
		emailSent:
			"An email has been sent to the address you supplied. Important! Please check your email for a message confirming your registration.",
		validation: {
			userName: {
				required: "Enter login",
				minLength: "Login must be at least 3 letters long",
				maxLength: `Login must be at most not ${config.DbColumnSizes.Users_UserName} characters`,
				nameInDb: "User name is already taken"
			},
			email: {
				required: "Enter email",
				emailSig: "@:Global.validation.emailSig",
				maxLength: `Email must be at most not ${config.DbColumnSizes.Users_Email} characters`
			},
			password: {
				required: "Enter password",
				minLength: `Password must be at least ${config.PasswordValidation.MinLength} characters`,
				minDifferentChars: `Password must contain at least ${config.PasswordValidation.MinDifferentChars} different characters`
			},
			password2: {
				equals: "Passwords must match"
			}
		}
	},
	RegisterEmailResult: {
		title: "Confirm Email",
		success: "Your email was confirmed.",
		error: "Something went wrong.",
		enter: "Sign In"
	},
	UserMenu: {
		profile: "Profile",
		yourAccount: "Account",
		adminPanel: "Admin Panel",
		exit: "Sign Out",
		logoutNotify: "You did logout"
	}
};
