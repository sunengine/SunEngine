export default {
  CategoryForm: {
    name: "Имя категории (eng)",
    title: "Заголовок",
    shortDescription: "Короткое описание",
    header: "Шапка категории",
    selectParent: "Выберите родительскую категорию",
    sectionType: "Тип категории",
    deleteConfirm: "Вы уверены что хотите удалить категорию?\nВсё содержание категории также будут удалено.",
    parent: "Родитель: ",
    hideCb: "Спрятать",
    appendUrlTokenCb: "Добавлять в URL",
    appendUrlTokenInfo: "(использовать только если вы понимаете что это)",
    isMaterialsContainerCb: "Содержит материалы",
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
      }
    }
  },
  CreateCategory: {
    title: "Добавить категорию",
    createBtn: "Создать",
    cancelBtn: "@:Global.btn.cancel",
    successNotify: "Категория добавлена.\nНе забудьте перегрузить сайт для обновления."
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
    successNotify: "Категория обновлена.\nНе забудьте перегрузить сайт для обновления."
  },
  AdminMenu: {
    categoriesAdmin: "Категории",
    rolesPermissions: "Настройка прав",
    rolesUsers: "Группы пользователей",
    cacheSettings: "Настройки кэширования",
    imagesCleaner: "Очистка изображений"
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
  CategoriesAdmin: {
    title: "Админка категорий",
    addCategoryBtn: "Добавить категорию"
  },
  CategoryItem: {
    rootCategory: "Корневая категория"
  },
  ImagesCleaner: {
    title: "Очистка изображений",
    clearBtn: "Очистить",
    refreshBtn : "Обновить",
    clearCount: "Очищено изображений: "
  },
  ProfileRoles: {
    roles: "Группы пользователя:",
    addRoleBtn: "Добавить группу",
    removeRoleBtn: "Удалить группу",
    addRoleConfirm: "Добавить в группу '{0}'?",
    addRoleConfirmOkBtn: "Добавить",
    removeRoleConfirm: "Удалить из группы '{0}'?",
    removeRoleConfirmOkBtn: "Удалить",
  },
  RolesPage: {
    title: "Группы пользователей",
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
  }
}
