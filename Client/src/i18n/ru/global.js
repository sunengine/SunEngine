export default {
	Global: {
		dialog: {
			cancel: "Отмена",
			yes: "Да",
			ok: "Да"
		},
		btn: {
			create: "Создать",
			save: "Сохранить",
			cancel: "Отмена",
			delete: "Удалить",
			reset: "Сбросить"
		},
		validation: {
			userName: {
				required: "Необходимо ввести имя пользователя",
				minLength: "Минимальная длинна имени пользователя - {minLength}",
				maxLength: "Максимальная длинна имени пользователя - {maxLength}",
				nameInDb: "Это имя уже занято",
				allowedUserNameCharacters: "Имя пользователя может состоять только из символов '{allowedUserNameCharacters}'"
			},
			email: {
				required: "Необходимо ввести email",
				emailSig: "Введите валидный email",
				maxLength: "Максимальная длинна email - {maxLength}"
			},
			password: {
				passwordsNotEquals: "Пароли должны совпадать",
				required: "Необходимо ввести пароль",
				requiredLength: "Минимальная длинна пароля - {requiredLength}",
				requiredUniqueChars:
					"Необходимо что бы пароль состоял не менее чем из {requiredUniqueChars} разных символов",
				requireDigit: "В пароль должно входить хотя бы одно число",
				requireLowercase:
					"В пароль должна входить хотя бы одна строчная буква (нижний регистр)",
				requireUppercase: "В пароль должна входить хотя бы одна заглавная буква",
				requireNonAlphanumeric:
					"В пароль должен входить хотя бы один не буквенный и не числовой символ, например '@','^','+'..."
			},
			emailSig: "Неправильная сигнатура email",
			jsonFormatError: "Ошибка валидации Json",
			fileSizeLimit:
				"Файл '{fileName}' слишком большого размера. Максимально допустимый размер файла {maxSize} мегабайта."
		},
		units: {
			megabytes: "мегабайт"
		},
		errorNotify: "Ошибка",
		successNotify: "Операция выполнена успешно",
		submitting: "Отправляю данные...",
		apiError: "Ошибка Api",
		refresh: "Перегрузить"
	},
	App: {
		loading: "Загрузка...",
		canNotConnectApi: "Невозможно соединиться с API."
	},

	MyEditor: {
		uploadImages: "Добавить изображения"
	},
	LayoutNames: {
		Articles: "Статьи в 1 раздел",
		Articles1: "Статьи с подразделами 1 уровень",
		Articles2: "Статьи с подразделами 2 уровня",
		Blog: "Блог",
		Forum0: "Форум в 1 поток",
		Forum1: "Форум с подразделами в 1 уровень",
		Forum2: "Форум с подразделами в 2 уровня"
	}
};
