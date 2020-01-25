export default {
	ChangeLink: {
		title: "Изменить ссылку на профиль",
		successNotify: "Ссылка отредактирована",
		link: "Токен ссылки",
		hintLink: "Ссылка на профиль:",
		linkValidationInfo:
			"Токен должен состоять не менее чем из 3 символов 'a-z', '-', '0-9'. И содержать хотя бы одну букву.",
		saveBtn: "@:Global.btn.save",
		validation: {
			minLength: "Длинна токена должна быть не менее 3",
			allowedChars: "Допустимы только строчные буквы английского алфавита и цифры",
			numberNotAllow: "Необходимо что бы в токен ссылки входили буквы",
			linkInDb: "Этот токен уже занят"
		}
	},
	ChangeName: {
		title: "Изменить имя пользователя",
		successNotify: "Имя изменено",
		nameValidationInfo:
			"Имя может состоять из букв, цифр, пробела и символа '-', длинны не менее 3.",
		saveBtn: "@:Global.btn.save",
		name: "Имя",
		password: "Пароль",
		validation: {
			password: {
				required: "Введите пароль"
			},
			name: {
				required: "Введите имя",
				minLength: "Длинна имени должна быть не меньше 3",
				allowedChars: "Возможно использование только допустимых символов",
				nameInDb: "Это имя уже занято"
			}
		}
	},
	EditInformation: {
		title: "Изменить информацию о себе",
		label: "Информация о вас на странице вашего профиля.",
		successNotify: "Информация успешно сохранена",
		save: "@:Global.btn.save"
	},
	LoadPhoto: {
		title: "Изменить фотографию пользователя",
		maxPhotoSize: "Максимальный размер изображения:",
		fileSizeLimit: "Вы выбрали файл слишком большого размера",
		resetBtn: "Сбросить фотографию",
		uploadNewPhotoBtn: "Загрузить фотографию",
		avatarDeletedSuccessNotify: "Аватар успешно удалён",
		avatarChangedSuccessNotify: "Аватар успешно обновлён"
	},
	MyBanList: {
		title: "Чёрный список пользователей",
		subTitle: "Эти пользователи не могут писать вам личные сообщения",
		voidResult: "Список пуст"
	},
	Sessions: {
		title: "Сессии авторизации",
		deviceInfo: "Устройство и браузер",
		current: "Текущая",
		updateDate: "Дата обновления",
		successNotify: "Сессии успешно удалены",
		logout: "Выйти"
	},
	SettingsMenu: {
		goToProfile: "Просмотреть профиль",
		changeEmail: "Изменить email",
		changePassword: "Изменить пароль",
		changeLink: "Изменить ссылку на профиль",
		changeName: "Изменить имя",
		changeYourInformation: "Изменить информацию о вас",
		changePhoto: "Изменить фотографию",
		sessions: "Сессии авторизации",
		banedUsersList: "Чёрный список пользователей"
	},
	SettingsPage: {
		title: "Личный кабинет"
	},
	SettingsPanel: {
		title: "@:SettingsPage.title"
	}
};
