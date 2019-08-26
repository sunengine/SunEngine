export default {

  // ——— categories ————————————————————————————————————

  CategoriesAdmin: {
    title: "Админка категорий",
    addCategoryBtn: "Добавить категорию"
  },
  CategoryForm: {
    name: "Имя категории (eng)",
    title: "Заголовок",
    subTitle: "Короткое описание",
    icon: "Иконка",
    header: "Шапка категории",
    selectParent: "Родительская категория",
    deleteConfirm: "Вы уверены что хотите удалить категорию?\nВсё содержание категории также будут удалено.",
    parent: "Родитель: ",
    hideCb: "Спрятать",
    settingsJson: "Json настройки",
    appendUrlTokenCb: "Добавлять в URL",
    appendUrlTokenInfo: "(использовать только если вы понимаете что это)",
    isMaterialsContainerCb: "Содержит материалы",
    materialsSubTitleInputType: "Установка подписи заголовка материала",
    materialsPreviewGeneratorName: "Алгоритм генерации превью к материалам",
    isMaterialsNameEditableCb: "Возможность редактирования имени (eng) материала (только для админа)",
    isCaching: "Кэшировать содержимое",
    cachingPageCount: "Кэшировать N страниц",
    noTypeLabel: "Без типа",
    layout: "Шаблон",
    validation: {
      name: {
        required: "Введите имя (eng) категории",
        minLength: "Имя (eng) должно быть не менее чем из 2 букв",
        allowedChars: "Имя (eng) должно состоять из символов `a-z`, `A-Z`, `0-9`, `-`"
      },
      title: {
        required: "Введите заголовок категории",
        minLength: "Заголовок должен состоять не менее чем из 3 букв"
      },
      icon: {
        minLength: "Длинна должна быть не меньше 3 символов",
        maxLength: "Максимальная длинна должна быть не более " + config.DbColumnSizes.Categories_Icon,
      },
      parent: {
        required: "Выберите родительскую категорию"
      },
      settingsJson: {
        jsonFormatError: "Неверный формат Json",
      }
    }
  },
  CategoryItem: {
    rootCategory: "Корневая категория"
  },
  CreateCategory: {
    title: "Добавить категорию",
    createBtn: "Создать",
    cancelBtn: "@:Global.btn.cancel",
    successNotify: "Категория добавлена."
  },
  EditCategory: {
    title: "Редактировать категорию",
    deleteBtn: "Удалить категорию",
    saveBtn: "@:Global.btn.save",
    cancelBtn: "@:Global.btn.cancel",
    deletedNotify: "Категория успешно удалена.",
    deleteConfirm: "Вы уверены, что хотите удалить категорию?\nВсе данные категории так же будут удалены.",
    deleteDialogBtnOk: "Удалить",
    deleteDialogBtnCancel: "Отмена",
    successNotify: "Категория обновлена."
  },

  // ——— menuItems ————————————————————————————————————

  CreateMenuItem: {
    title: "Создать пункт меню",
    createBtn: "@:Global.btn.create",
    cancelBtn: "@:Global.btn.cancel",
    successNotify: "Пункт меню успешно создан"
  },
  EditMenuItem: {
    title: "Редактировать пункт меню",
    saveBtn: "@:Global.btn.save",
    cancelBtn: "@:Global.btn.cancel",
    successNotify: "Пункт меню успешно сохранён"
  },
  MenuAdminItem: {

  },
  MenuItemForm: {
    name: "Идентификатор (eng)",
    title: "Заголовок",
    subTitle: "Подпись заголовка",
    parent: "Родительский элемент",
    rootElement: "Корневой элемент",
    url: "Ссылка, внутренняя или внешняя",
    exact: "Подсвечивать пункт меню только при точном совпадении адреса (exact)",
    roles: "Роли которые могут видеть пункт меню",
    cssClass: "Css class",
    icon: "Иконка",
    settingsJson: "Кастомные настройки (Json)",
    isHidden: "Спрятать",
    local: "Локальная ссылка",
    external: "Внешняя ссылка",
    urlError: "Введена некорректная ссылка",
    validation: {
      name: {
        minLength: "Длинна должна быть не меньше 3 символов",
        maxLength: "Максимальная длинна должна быть не более " + config.DbColumnSizes.MenuItems_Name,
        allowedChars: "Вы ввели недопустимые символы, разрешено использование только [a-zA-Z0-9_-] символов"
      },
      title: {
        required: "Необходимо ввести заголовок",
        minLength: "Длинна должна быть не меньше 3 символов",
        maxLength: "Максимальная длинна должна быть не более " + config.DbColumnSizes.Categories_Title,
      },
      subTitle: {
        minLength: "Длинна должна быть не меньше 3 символов",
        maxLength: "Максимальная длинна должна быть не более " + config.DbColumnSizes.MenuItems_SubTitle,
      },
      cssClass: {
        minLength: "Длинна должна быть не меньше 3 символов",
        maxLength: "Максимальная длинна должна быть не более " + config.DbColumnSizes.MenuItems_CssClass,
      },
      icon: {
        minLength: "Длинна должна быть не меньше 3 символов",
        maxLength: "Максимальная длинна должна быть не более " + config.DbColumnSizes.MenuItems_Icon,
      },
      settingsJson: {
        jsonFormatError: "Неверный формат Json",
      }
    }
  },
  MenuItemsAdmin: {
    title: "Редактирование меню",
    addMenuItemBtn: "Добавить пункт меню",
    deleteMsg: "Удалить пункт меню?",
    btnDeleteOk: "@:Global.dialog.ok",
    btnDeleteCancel: "@:Global.dialog.cancel"
  },

  // ——— roles ————————————————————————————————————

  ProfileRoles: {
    roles: "Группы пользователя:",
    addRoleBtn: "Добавить группу",
    removeRoleBtn: "Удалить группу",
    addRoleConfirmTitle: "",
    addRoleConfirmMessage: "Добавить в группу '{0}'?",
    addRoleConfirmOkBtn: "Добавить",
    addRoleConfirmCancelBtn: "@:Global.dialog.cancel",
    removeRoleConfirmTitle: "",
    removeRoleConfirmMessage: "Удалить из группы '{0}'?",
    removeRoleConfirmOkBtn: "Удалить",
    removeRoleConfirmCancelBtn: "@:Global.dialog.cancel"
  },
  RolesPage: {
    title: "Пользователи в группах",
    roles: "Группы",
  },
  RolesPermissions: {
    title: "Загрузка Json прав для групп",
    backupWarning: "Перед загрузкой необходимо сделать backup базы.",
    saveToServerBtn: "Сохранить на сервер",
    getFromServer: "Загрузить с сервера",
    getSuccessNotify: "Данные загружены с сервера",
    saveSuccessNotify: "Настройки групп успешно обновлены",
    textAreaLabel: "Json файл конфигурации прав групп"
  },
  RoleUsers: {
    users: "Пользователи",
    filter: "Найти по имени",
    noResults: "Нет результатов",
    filterLimitReached: "Выведены первые {0} результатов"
  },

  // ——— all ————————————————————————————————————

  AdminMenu: {
    menuItems: "Меню",
    menuItemsCaption: "",
    //menuItemsCaption: "Редактирование меню сайта",
    categories: "Категории",
    categoriesCaption: "",
    //categoriesCaption: "Редактирование разделов сайта",
    rolesUsers: "Группы",
    rolesUsersCaption: "",
    //rolesUsersCaption: "Пользователи по группам",
    rolesPermissions: "Права групп",
    rolesPermissionsCaption: "",
    //rolesPermissionsCaption: "Добавление, редактирование, удаление групп и их прав",
    cacheSettings: "Кэширование",
    cacheSettingsCaption: "",
    //cacheSettingsCaption: "Способ кэширования на сайте",
    cypherSecrets: "Ключи шифрования",
    cypherSecretsCaption:"",
    imagesCleaner: "Очистка",
    imagesCleanerCaption: "",
    //imagesCleanerCaption: "Удалить неиспользуемые изображения",
    deletedElements: "Удалённое",
    deletedElementsCaption: "",
    //deletedElementsCaption: "Показывать удалённые материалы и коментарии",
    version: "Версия SunEngine"
  },
  AdminPage: {
    title: "@:AdminPanel.title"
  },
  AdminPanel: {
    title: "Админка"
  },
  CacheSettings: {
    title: "Настройки кэширования",
    cachePolicy: "Политика кэширования",
    alwaysPolicy: "Всегда кэшировать",
    neverPolicy: "Никогда не кэшировать",
    customPolicy: "Настриваемая политика",
    cacheLifetime: "Время хранения записи",
    saveChangesBtn: "Сохранить настройки",
    withoutInvalidationTime: "Без ограничения по времени",
    successNotify: "Политика кэширования изменена",
    error: "Произошла ошибка",
    validation: {
      invalidateCacheTime: {
        required: "Поле должно быть заполнено",
        invalidValue: "Значение не может быть ниже 0",
      }
    }
  },
  CypherSecrets: {
    title: "Сбросить ключи шифрования"
  },
  DeletedElements: {
    title: "Удалённые элементы",
    showDeleted: "Показать удалённые элементы",
    info1: "Если галка установлена, во всех категориях будут показываться стёртые материалы.",
    info2: "Если в URL любого раздела добавить '?deleted=1' - стёртые материалы будут показаны в рамках этой ссылки.",
    info3: "Функция не предусмотрена для разделов, где выводятся сразу несколько категорий, например 'новые сообщения' на форуме."
  },
  ImagesCleaner: {
    title: "Очистка изображений",
    info: "Потерянные изображения, которые не используются на сайте",
    clearBtn: "Очистить",
    refreshBtn : "Обновить",
    clearCount: "Очищено изображений: ",
    emptyResult: "Каталог пуст"
  }
}
