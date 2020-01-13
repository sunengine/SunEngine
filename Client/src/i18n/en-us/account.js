export default {
	ChangeEmail: {
		title: "Edit user email",
		successNotify:
			"Сообщение с ссылкой для подтверждения email отправлено по почте",
		password: "Current password",
		newEmail: "New email",
		saveBtn: "@:Global.btn.save",
		validation: {
			password: {
				required: "Required password"
			},
			email: {
				required: "Required email",
				emailSig: "Enter correct email"
			}
		}
	},
	ChangeEmailResult: {
		title: "@:RegisterEmailResult.title",
		success: "@:RegisterEmailResult.success",
		error: "@:RegisterEmailResult.error"
	},
	ChangePassword: {
		title: "Change password",
		successNotify: "Password changed.",
		changeBtn: "Change password",
		passwordOld: "Current password",
		password: "New password",
		password2: "Repeat new password",
		validation: {
			passwordOld: {
				required: "Required enter old password"
			},
			password: {
				required: "@:Register.validation.password.required",
				minLength: "@:Register.validation.password.minLength",
				minDifferentChars: "@:Register.validation.password.minDifferentChars"
			},
			password2: {
				equals: "@:Register.validation.password2.equals"
			}
		}
	},
	ResetPassword: {
		title: "Reset password",
		email: "Enter email",
		resetPasswordBtn: "Reset password",
		success: "Message with a link to reset password sent to your Email.",
		validation: {
			email: {
				required: "@:ResetPassword.email",
				emailSig: "@:Global.validation.emailSig"
			}
		}
	},
	ResetPasswordFailed: {
		title: "Reset password",
		message: "Something went wrong."
	},
	ResetPasswordSetNew: {
		title: "Set password",
		successMessage: "Password changed.",
		enter: "Sign In",
		saveBtn: "Change password",
		password: "New password",
		password2: "Repeat password",
		validation: {
			password: {
				required: "@:Register.validation.password.required",
				minLength: "@:Register.validation.password.minLength",
				minDifferentChars: "@:Register.validation.password.minDifferentChars"
			},
			password2: {
				equals: "@:Register.validation.password2.equals"
			}
		}
	}
};
