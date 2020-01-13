export default {
	ChangeEmail: {
		title: "Изменить email пользователя",
		successNotify:
			"Сообщение с ссылкой для подтверждения email отправлено по почте",
		password: "Ваш пароль",
		newEmail: "Новый email",
		saveBtn: "@:Global.btn.save",
		validation: {
			password: {
				required: "Необходимо ввести пароль"
			},
			email: {
				required: "Необходимо ввести email",
				emailSig: "Введите валидный email"
			}
		}
	},
	ChangeEmailResult: {
		title: "@:RegisterEmailResult.title",
		success: "@:RegisterEmailResult.success",
		error: "@:RegisterEmailResult.error"
	},
	ChangePassword: {
		title: "Изменить пароль",
		successNotify: "Пароль изменён.",
		changeBtn: "Изменить пароль",
		passwordOld: "Старый пароль",
		password: "Новый пароль",
		password2: "Подтвердите новый пароль",
		validation: {
			passwordOld: {
				required: "Необходимо ввести старый пароль"
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
		title: "Сброс пароля",
		email: "Введите email",
		resetPasswordBtn: "Сбросить пароль",
		success: "Сообщение с ссылкой для сброса пароля отправлено на email",
		validation: {
			email: {
				required: "@:ResetPassword.email",
				emailSig: "@:Global.validation.emailSig"
			}
		}
	},
	ResetPasswordFailed: {
		title: "Сброс пароля",
		message: "Что-то пошло не так. Возможно истекло время сброса."
	},
	ResetPasswordSetNew: {
		title: "Установить пароль",
		successMessage: "Пароль изменён.",
		enter: "Войти",
		saveBtn: "Изменить пароль",
		password: "Новый пароль",
		password2: "Подтвердите пароль",
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
