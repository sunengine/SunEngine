export default {

  Global: {
    dialog: {
      cancel: "Отмена",
      yes: "Да",
      ok: "Да"
    },
    btn: {
      save: "Сохранить",
      cancel: "Отмена"
    },
    validation: {
      emailSig: "Неправильная сигнатура email",
    },
    errorNotify: "Ошибка",
    successNotify: "Операция выполнена успешно",
    submitting: "Отправляю данные..."
  },

  App: {
    loading: "Загрузка...",
    canNotConnectApi: "Невозможно соединиться с API."
  },
  Captcha: {
    newMessageBtn: "Выдать новое изображение",
    waitMessage: "Что бы сгенерировать новый токен, нужно немного подождать, попробуйте через некоторое время",
    enterToken: "Введите текст с картинки",
    required: "@:Captcha.enterToken",
  },


  CreateEditComment: {
    required: "Введите сообщение",
    htmlTextSizeOrHasImage: "Минимальная длинна текста - 5",
    spamProtectionMessage: "Нельзя так часто отправлять сообщения. Подождите немного.",
    addSuccessNotify: "Комментарий добавлен",
    editSuccessNotify: "Комментарий обновлён",
  },
  CreateMaterial: {
    title: "Добавить материал",
    sendBtn: "Отправить",
    successNotify: "Материал успешно добавлен",
  },
  EditMaterial: {
    title: "Редактировать материал",
    saveBtn: "Сохранить",
    successNotify: "Материал успешно сохранён",
  },
  MaterialForm: {
    name: "Имя (eng)",
    title: "Заголовок",
    description: "Короткое описание",
    tags: "Метки",
    selectCategory: "Выберите раздел",
    category: "Раздел: {0}",
    validation: {
      name: {
        allowedChars: "Имя должно содержать только английские буквы цифры и символ '-'",
        numberNotAllowed: "Имя материала не может быть числом",
        minLength: "Минимальная длинна имени - 3",
        maxLength: `Максимальная длинна имени - ${config.DbColumnSizes.Materials_Name}`,
      },
      title: {
        required: "Введите заголовок",
        minLength: "Минимальная длинна заголовка - 3",
        maxLength: `Максимальная длинна заголовка - ${config.DbColumnSizes.Materials_Title}`,
      },
      text: {
        required: "Введите текст",
        htmlTextSizeOrHasImage: "Минимальная длинна текста - 5",
      },
      description: {
        maxLength: "Максимально допустимая длинна " + config.DbColumnSizes.Materials_Description
      },
      category: {
        required: "Выберите раздел"
      }
    }
  },
  CreateEditMaterial: {
    titleField: "Заголовок",
    addTitle: "Добавить материал",
    editTitle: "Редактировать текст: {0}",
    description: "Короткое описание",
    tags: "Метки",
    sendBtn: "Отправить",
    selectCategory: "Выберите раздел",
    category: "Раздел: {0}",
    successNotify: "Материал успешно добавлен",
    spamProtectionNotify: "Нельзя так часто создавать материалы. Необходимо подождать.",
    validation: {
      title: {
        required: "Введите заголовок",
        minLength: "Минимальная длинна заголовка - 3",
        maxLength: `Максимальная длинна заголовка - ${config.DbColumnSizes.Materials_Title}`,
      },
      text: {
        required: "Введите текст",
        htmlTextSizeOrHasImage: "Минимальная длинна текста - 5",
      },
      description: {
        maxLength: "Максимально допустимая длинна " + config.DbColumnSizes.Materials_Description
      },
      category: {
        required: "Выберите раздел"
      }
    }
  },
  MyEditor: {
    uploadImages: "Добавить изображения"
  },
  Material: {
    category: "раздел:",
    edit: "Редактировать",
    tags: "Метки:"
  },
  ReadComment: {
    edit: "редактировать",
    deleteDialogMessage: "Удалить сообщение?",
    moveToTrashSuccess: "Комментарий удалён"
  },


  SettingsMenu: {
    goToProfile: "Просмотреть профиль",
    changeEmail: "Изменить email",
    changePassword: "Изменить пароль",
    changeLink: "Изменить link",
    changeName: "Изменить имя",
    changeYourInformation: "Изменить информацию о вас",
    changePhoto: "Изменить фотографию",
    banedUsersList: "Забаненые пользователи"
  },
  SettingsPage: {
    title: "Личный кабинет"
  },
  SettingsPanel: {
    title: "@:SettingsPage.title"
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
  ChangeLink: {
    title: "Редактировать Link пользователя",
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


  ForumPanel: {
    newTopics: "Новые сообщения",
    sections: "Разделы"
  },
  Topic: {
    lastFrom: "Последнее от"
  },

  Thread: {
    newTopicBtn: "Новая тема",
    topic: "Тема",
    last: "Последнее"
  },
  NewTopics: {
    titleStart: "Новые темы",
    newTopicBtn: "@:Thread.newTopicBtn",
    topic: "@:Thread.topic",
    last: "@:Thread.last"
  },
  Profile: {
    roles: "Группы",
    sendPrivateMessageBtn: "Написать пользователю",
    banBtn: "Забанить",
    unBanBtn: "Разбанить",
    banNotify: "Пользователь {0} теперь не может вам писать",
    unBanNotify: "Пользователь {0} теперь может вам писать"
  },
  SendPrivateMessage: {
    title: "Написать личное сообщение",
    titleStart: "Написать",
    sendBtn: "Отправить",
    successNotify: "Сообщение успешно отправлено пользователю {0}",
    sendErrorNotify: "Сообщение не отправлено. Ошибка на сервере.",
    sendSpamProtectionNotify: "Нельзя так часто отправлять личные сообщения. Необходимо подождать.",
  },
  MyBanList: {
    title: "Забаненые пользователи"
  },
  LoadPhoto: {
    title: "Изменить фотографию пользователя",
    resetBtn: "Сбросить фотографию",
    uploadNewPhotoBtn: "Выбрать фотографию",
    avatarDeletedSuccessNotify: "Аватар успешно удалён",
    avatarChangedSuccessNotify: "Аватар успешно обновлён"
  },
  EditInformation: {
    title: "Редактировать информацию о себе",
    label: "Информация о вас на странице вашего профиля.",
    successNotify: "Информация успешно сохранена",
    save: "@:Global.btn.save"
  },

  DeletedComment: {
    label: "Сообщение удалено"
  },
  Error404: {
    title: "Ошибка 404",
    info: "Извините, страница не найдена...",
    goBackBtn: "Вернуться назад"
  },
};
