export default {
	Login: {
		title: "Войти",
		nameOrEmail: "Имя или email",
		password: "Пароль",
		entering: "Заходим...",
		enterBtn: "Войти",
		forgotPassword: "Забыли пароль?",
		successNotify: "Вы зашли",
		validation: {
			nameOrEmail: {
				required: "Введите имя или email"
			},
			password: {
				required: "Введите пароль"
			}
		}
	},
	LoginRegisterMenu: {
		enter: "Войти",
		register: "Зарегистрироваться"
	},
	Register: {
		title: "Зарегистрироваться",
		userName: "Имя пользователя",
		email: "Email",
		password: "Пароль",
		password2: "Подтвердите пароль",
		registerBtn: "@:Register.title",
		registering: "Регистрируемся...",
		emailSent: "Сообщение с ссылкой для регистрации отправлено на email",
		validation: {
			userName: {
				required: "Введите имя пользователя",
				minLength: "Имя пользователя должно быть не менее чем из 3 букв",
				maxLength: `Имя пользователя должно состоять не более чем из ${config.DbColumnSizes.Users_UserName} символов`,
				nameInDb: "Имя занято"
			},
			email: {
				required: "Введите email",
				emailSig: "@:Global.validation.emailSig",
				maxLength: `Email должен состоять не более чем из ${config.DbColumnSizes.Users_Email} символов`
			},
			password: {
				required: "Введите пароль",
				minLength: `Пароль должен состоять не менее чем из ${config.PasswordValidation.MinLength} символов`,
				minDifferentChars: `В пароле должно быть не менее ${config.PasswordValidation.MinDifferentChars} разных символов`
			},
			password2: {
				equals: "Пароли должны совпадать"
			}
		}
	},
	RegisterEmailResult: {
		title: "Подтверждение почты",
		success: "Ваша почта успешно подтверждена.",
		error: "Подтверждение почты. Что-то пошло не так.",
		enter: "Войти"
	},
	UserMenu: {
		profile: "Профиль",
		yourAccount: "Личный кабинет",
		adminPanel: "Админка",
		exit: "Выйти",
		logoutNotify: "Вы вышли"
	}
};
