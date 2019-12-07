export default {

  // ——— categories ————————————————————————————————————

  CategoriesAdmin: {
    title: "Админка категорий",
    addCategoryBtn: "Добавить категорию"
  },
  CategoryForm: {
    name: "Имя (eng)",
    title: "Заголовок",
    subTitle: "Подзаголовок",
    icon: "Иконка",
    header: "Шапка",
    selectParent: "Родительская категория",
    deleteConfirm: "Вы уверены что хотите удалить категорию?\nВсё содержание категории также будут удалено.",
    parent: "Родитель: ",
    hideCb: "Спрятать",
    settingsJson: "Json настройки",
    appendUrlTokenCb: "Добавлять в URL",
    appendUrlTokenInfo: "(использовать только если вы понимаете что это)",
    isMaterialsContainerCb: "Содержит материалы",
    isMaterialsSubTitleEditableCb: "Возможность редактирования подзаголовка материала",
    isMaterialsNameEditableCb: "Возможность редактирования имени (eng) материала (только для админа)",
    isCaching: "Кэшировать содержимое",
    cachingPageCount: "Кэшировать N страниц",
    noTypeLabel: "Без типа",
    layout: "Шаблон",
    validation: {
      name: {
        required: "Введите имя (eng) категории",
        minLength: "Имя (eng) должно быть не менее чем из 2 букв",
        allowedChars: "Имя (eng) должно состоять из символов `a-z`, `A-Z`, `0-9`, `-`",
        maxLength: `Имя (eng) должено состоять не более чем из ${config.DbColumnSizes.Categories_Name} символов`,
      },
      title: {
        required: "Введите заголовок категории",
        minLength: "Заголовок должен состоять не менее чем из 3 букв",
        maxLength: `Заголовок должен состоять не более чем из ${config.DbColumnSizes.Categories_Title} букв`,
      },
      subTitle: {
        maxLength: `Подзаголовок должен состоять не более чем из ${config.DbColumnSizes.Categories_SubTitle} букв`,
      },
      icon: {
        minLength: "Длинна должна быть не меньше 3 символов",
        maxLength: "Максимальная длинна должна быть не более " + config.DbColumnSizes.Categories_Icon,
      },
      parent: {
        required: "Выберите родительскую категорию"
      },
      settingsJson: {
        jsonFormatError: "@:Global.validation.validation",
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

  // ——— components ———————————————————————————————————

  ComponentForm: {
    name: "Имя (eng)",
    type: "Тип",
    isCacheData: "Кешировать",
    serverSettingsJson: "Серверные настройки JSON",
    clientSettingsJson: "Клиентские настройки JSON",
    roles: "Роли которым доступен компонент",
    validation: {
      name: {
        required: "Введите имя (eng)",
        minLength: "Длинна должна быть не меньше 3 символов",
        maxLength: "Максимальная длинна должна быть не более " + config.DbColumnSizes.Components_Name,
        allowedChars: "Имя (eng) должно состоять из символов `a-z`, `A-Z`, `0-9`, `-`, `_`"
      },
      type: {
        required: "Выберите тип",
      },
      jsonFormatError: "@:Global.validation.validation",
    }
  },
  ComponentsAdmin: {
    title: "Админка компонентов",
    addComponentBtn: "Добавить компонент",
  },
  CreateComponent: {
    title: "Задать компонент",
    createBtn: "@:Global.btn.create",
    cancelBtn: "@:Global.btn.cancel"
  },
  EditComponent: {
    title: "Изменить компонент",
    saveBtn: "@:Global.btn.save",
    cancelBtn: "@:Global.btn.cancel",
    deleteBtn: "Удалить компонент",
    deleteMsg: "Удалить компонент?",
    btnDeleteOk: "@:Global.dialog.ok",
    btnDeleteCancel: "@:Global.dialog.cancel"
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
    deleteBtn: "Удалить пункт меню",
    successNotify: "Пункт меню успешно сохранён",
    deleteMsg: "Удалить пункт меню?",
    btnDeleteOk: "@:Global.dialog.ok",
    btnDeleteCancel: "@:Global.dialog.cancel",
  },
  MenuAdminItem: {},
  MenuItemForm: {
    name: "Идентификатор (eng)",
    title: "Заголовок",
    subTitle: "Подзаголовок",
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
        jsonFormatError: "@:Global.validation.validation"
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
    title: "Установка разрешений групп",
    saveToServerBtn: "Сохранить на сервер",
    getFromServer: "Перезагрузить с сервера",
    getSuccessNotify: "Данные загружены с сервера",
    saveSuccessNotify: "Настройки групп успешно обновлены",
    textAreaLabel: "Json конфигурация"
  },
  RoleUsers: {
    users: "Пользователи",
    filter: "Найти по имени",
    noResults: "Нет результатов",
    filterLimitReached: "Выведены первые {0} результатов"
  },

  // ——— all ————————————————————————————————————

  AdminMenu: {
    adminInformation: "Информация",
    adminInformationCaption: "",
    //adminInformationCaption: "Информация о сервере",
    menuItems: "Меню",
    menuItemsCaption: "",
    //menuItemsCaption: "Редактирование меню сайта",
    categories: "Категории",
    categoriesCaption: "",
    //categoriesCaption: "Редактирование разделов сайта",
    components: "Компоненты",
    componentsCaption: "",
    skins: "Темы оформления",
    skinsCaption: "",
    //skinsCaption: "Добавление, установка, удаление",
    rolesUsers: "Группы пользователей",
    rolesUsersCaption: "",
    //rolesUsersCaption: "Пользователи по группам",
    rolesPermissions: "Разрешения групп",
    rolesPermissionsCaption: "",
    //rolesPermissionsCaption: "Добавление, редактирование, удаление групп и их прав",
    cacheSettings: "Кэширование",
    cacheSettingsCaption: "",
    configuration: "Конфигурация",
    configurationCaption: "",
    //configurationCaption: "Конфигурация",
    //cacheSettingsCaption: "Способ кэширования на сайте",
    cypherSecrets: "Ключи шифрования",
    cypherSecretsCaption: "",
    imagesCleaner: "Очистка диска",
    imagesCleanerCaption: "",
    //imagesCleanerCaption: "Удалить неиспользуемые изображения",
    deletedElements: "Удалённое",
    deletedElementsCaption: "",
    //deletedElementsCaption: "Показывать удалённые материалы и коментарии",
    version: "Версия SunEngine",
    resetCache: "Сбросить кеш",
    resetCacheCaption: "",
    //resetCacheCaption: "Сбросить весь кеш на сервере",
    resetCacheSuccess: "Кеш сброшен успешно"
  },
  AdminInformation: {
    title: "Информация",
    serverName: "Имя сервера",
    serverVersion: "Версия сервера",
    serverRepository: "Репозиторий сервера",
    sunEngineVersion: "Версия SunEngine",
    clientName: "Имя клиента",
    clientVersion: "Версия клиента",
    dotNetVersion: "Версия DotNet",
    quasarVersion: "Версия Quasar",
    vueJsVersion: "Версия VueJS",
    maintainer: "Хранитель сайта",
    maintainerContacts: "Контакты хранителя",
    description: "Описание",
    sunEngineRepository: "Репозиторий SunEngine",
    sunEngineSkinsRepository: "Репозиторий тем оформления",
    additionalData: "Дополнительная информация"
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
  ConfigurationAdmin: {
    title: "Конфигурация сайта",
    filter: "Фильтр",
    noResults: "Ничего не найдено",
    successNotify: "Значения конфигурации успешно сохранены",
    resetSuccessNotify: "Значения конфигурации перезагружены с сервера",
    resetBtn: "Перезагрузить с сервера",
    cancelBtn: "@:Global.btn.cancel",
    saveBtn: "@:Global.btn.save"
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
    title: "Очистка диска",
    info: "Потерянные изображения, которые были загружены, но не используются.",
    working: "Очистка",
    clearBtn: "Удалить потерянные изображения",
    refreshBtn: "Обновить список",
    clearCount: "Очищено изображений: ",
    emptyResult: "Потерянные изображения отсутсвуют"
  },
  SkinsAdmin: {
    title: "Темы оформления",
    mainSkins: "Основные темы",
    partialSkins: "Дополнительные темы"
  },
  MainSkinsAdmin: {
    title: "Основные темы оформления",
    current: "Текущая",
    info: "Темы оформления и документация по созданию тем - ",
    author: "Автор: ",
    contacts: "Контакты: ",
    description: "Описание: ",
    link: "Ссылка на источник.",
    version: "Версия: ",
    upload: "Загрузить тему",
    uploadSuccessNotify: "Тема успешно загружена",
    deleteSuccessNotify: "Тема успешно удалён",
    set: "Установить",
    deleteMsg: "Удалить тему?",
    btnDeleteOk: "@:Global.dialog.yes",
    btnDeleteCancel: "@:Global.dialog.cancel"
  },
  PartialSkinsAdmin: {
    title: "Дополнительные темы оформления",
    onBtn: "Включить",
    offBtn: "Выключить",
    deleteMsg: "Удалить тему?",
    btnDeleteOk: "@:Global.dialog.yes",
    btnDeleteCancel: "@:Global.dialog.cancel",
    upload: "Загрузить дополнительную тему",
  }
}
