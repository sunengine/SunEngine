export default {
  ChangeLink: {
    title: "Изменить link пользователя",
    successNotify: "Link отредактирован",
    link: "Link",
    linkValidationInfo: "Link должен состоять не менее чем из 3 символов 'a-z', 'A-Z', '-', '0-9'. И содержать хотя бы одну букву.",
    saveBtn: "@:Global.btn.save",
    validation: {
      minLength: "Длинна link должна быть не менее 3",
      allowedChars: "Допустимы только буквы английского алфавита и цифры",
      numberNotAllow: "Необходимо что бы в link входили буквы",
      linkInDb: "Этот link уже занят"
    }
  },
  ChangeName: {
    title: "Изменить имя пользователя",
    successNotify: "Имя изменено",
    nameValidationInfo: "Имя может состоять из букв, цифр, пробела и символа '-', длинны не менее 3.",
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
    resetBtn: "Сбросить фотографию",
    uploadNewPhotoBtn: "Выбрать фотографию",
    avatarDeletedSuccessNotify: "Аватар успешно удалён",
    avatarChangedSuccessNotify: "Аватар успешно обновлён"
  },
  MyBanList: {
    title: "Забаненые пользователи"
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
    changeLink: "Изменить link",
    changeName: "Изменить имя",
    changeYourInformation: "Изменить информацию о вас",
    changePhoto: "Изменить фотографию",
    sessions: "Сессии авторизации",
    banedUsersList: "Забаненые пользователи"
  },
  SettingsPage: {
    title: "Личный кабинет"
  },
  SettingsPanel: {
    title: "@:SettingsPage.title"
  }
}
