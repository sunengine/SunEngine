export default {
	// ——— categories ————————————————————————————————————

	CategoriesAdmin: {
		title: "Админка категорий",
		addCategoryBtn: "Добавить категорию",
		showInfo: "Информация о категориях"
	},
	CategoryForm: {
		name: "Именной идентификатор (eng)",
		title: "Заголовок",
		token: "Токен URL",
		appendTokenToSubCatsPath: "Добавлять токен в дочерние категории",
		showInBreadcrumbs: "Показать в строке пути (breadcrumbs)",
		subTitle: "Подзаголовок",
		icon: "Иконка",
		header: "Шапка",
		selectParent: "Родительская категория",
		deleteConfirm:
			"Вы уверены что хотите удалить категорию?\nВсё содержание категории также будут удалено.",
		parent: "Родитель: ",
		hideCb: "Спрятать",
		settingsJson: "Json настройки",
		appendUrlTokenCb: "Добавлять в URL",
		appendUrlTokenInfo: "(использовать только если вы понимаете что это)",
		isMaterialsContainerCb: "Содержит материалы",
		isMaterialsSubTitleEditableCb:
			"Возможность редактирования подзаголовка материала",
		isMaterialsNameEditableCb:
			"Возможность редактирования имени (eng) материала (только для админа)",
		isCaching: "Кэшировать содержимое",
		cachingPageCount: "Кэшировать N страниц",
		noTypeLabel: "Без типа",
		layout: "Шаблон",
		validation: {
			name: {
				required: "Введите имя (eng) категории",
				minLength: "Имя (eng) должно быть не менее чем из 2 букв",
				allowedChars:
					"Имя (eng) должно состоять из символов `a-z`, `A-Z`, `0-9`, `-`",
				maxLength: `Имя (eng) должено состоять не более чем из ${config.DbColumnSizes.Categories_Name} символов`
			},
			token: {
				allowedChars:
					"Токен URL должен состоять из символов `a-z`, `A-Z`, `0-9`, `-`",
				maxLength: `Токен URL должен состоять не более чем из ${config.DbColumnSizes.Categories_Token} символов`
			},
			title: {
				required: "Введите заголовок категории",
				minLength: "Заголовок должен состоять не менее чем из 3 букв",
				maxLength: `Заголовок должен состоять не более чем из ${config.DbColumnSizes.Categories_Title} букв`
			},
			subTitle: {
				maxLength: `Подзаголовок должен состоять не более чем из ${config.DbColumnSizes.Categories_SubTitle} букв`
			},
			icon: {
				minLength: "Длинна должна быть не меньше 3 символов",
				maxLength:
					"Максимальная длинна должна быть не более " +
					config.DbColumnSizes.Categories_Icon
			},
			parent: {
				required: "Выберите родительскую категорию"
			},
			settingsJson: {
				jsonFormatError: "@:Global.validation.jsonFormatError"
			}
		}
	},
	CategoryItem: {
		rootCategory: "Корневая категория",
		moveUpBtnTooltip: "Переместить вверх",
		moveDownBtnTooltip: "Переместить вниз",
		editBtnTooltip: "Редактировать",
		createSubCategoryBtnTooltip: "Создать подкатегорию",
		moveToBtnTooltip: "Перейти к категории",
		showCategoryBtnTooltip: "Просмотреть категорию в админке"
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
		deleteConfirm:
			"Вы уверены, что хотите удалить категорию?\nВсе данные категории так же будут удалены.",
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
				maxLength:
					"Максимальная длинна должна быть не более " +
					config.DbColumnSizes.Components_Name,
				allowedChars:
					"Имя (eng) должно состоять из символов `a-z`, `A-Z`, `0-9`, `-`, `_`"
			},
			type: {
				required: "Выберите тип"
			},
			jsonFormatError: "@:Global.validation.validation"
		}
	},
	ComponentsAdmin: {
		title: "Админка компонентов",
		addComponentBtn: "Добавить компонент"
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
		btnDeleteCancel: "@:Global.dialog.cancel"
	},
	MenuAdminItem: {
		moveUpBtnTooltip: "Переместить вверх",
		moveDownBtnTooltip: "Переместить вниз",
		editBtnTooltip: "Редактировать",
		changeIsHiddenBtnTooltip: "Спрятать / Показать",
		addSubMenuItemBtnTooltip: "Добавить дочерний элемент",
		goToBtnTooltip: "Перейти по ссылке",
		deleteBtnTooltip: "Удалить"
	},
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
				maxLength:
					"Максимальная длинна должна быть не более " +
					config.DbColumnSizes.MenuItems_Name,
				allowedChars:
					"Вы ввели недопустимые символы, разрешено использование только [a-zA-Z0-9_-] символов"
			},
			title: {
				required: "Необходимо ввести заголовок",
				minLength: "Длинна должна быть не меньше 3 символов",
				maxLength:
					"Максимальная длинна должна быть не более " +
					config.DbColumnSizes.Categories_Title
			},
			subTitle: {
				minLength: "Длинна должна быть не меньше 3 символов",
				maxLength:
					"Максимальная длинна должна быть не более " +
					config.DbColumnSizes.MenuItems_SubTitle
			},
			cssClass: {
				minLength: "Длинна должна быть не меньше 3 символов",
				maxLength:
					"Максимальная длинна должна быть не более " +
					config.DbColumnSizes.MenuItems_CssClass
			},
			icon: {
				minLength: "Длинна должна быть не меньше 3 символов",
				maxLength:
					"Максимальная длинна должна быть не более " +
					config.DbColumnSizes.MenuItems_Icon
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
		addRoleConfirmMessage: "Добавить в группу '{roleName}'?",
		addRoleConfirmOkBtn: "Добавить",
		addRoleConfirmCancelBtn: "@:Global.dialog.cancel",
		removeRoleConfirmTitle: "",
		removeRoleConfirmMessage: "Удалить из группы '{roleName}'?",
		removeRoleConfirmOkBtn: "Удалить",
		removeRoleConfirmCancelBtn: "@:Global.dialog.cancel"
	},
	RolesPage: {
		title: "Пользователи в группах",
		roles: "Группы"
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
		filter: "Фильтр",
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
		configuration: "Конфигурация",
		configurationCaption: "",
		//configurationCaption: "Конфигурация",
		systemTools: "Утилиты",
		systemToolsCaption: "",
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
	ConfigurationAdmin: {
		title: "Конфигурация сайта",
		filter: "Фильтр",
		noResults: "Ничего не найдено",
		successNotify: "Значения конфигурации успешно сохранены",
		resetSuccessNotify: "Значения конфигурации перезагружены с сервера",
		resetBtn: "Сбросить",
		resetBtnTooltip: "Сбросить введённые сейчас данные и перегрузить с сервера",
		cancelBtn: "@:Global.btn.cancel",
		saveBtn: "@:Global.btn.save",
		groupTitles: {
			Editor: "Редактор",
			Global: "Основной",
			Dev: "Отладка",
			Cache: "Кеш",
			Images: "Изображения",
			Sanitizer: "Очистка HTML",
			Email: "Почта",
			Scheduler: "Расписание вызова функций",
			Materials: "Материалы",
			Comments: "Комментарии",
			Forum: "Форум",
			Articles: "Статьи",
			Captcha: "Капча. Проверка что пользователь.",
			Blog: "Блог",
			Jwe: "Безопасность токенов Jwe",
			Skins: "Темы оформления"
		},
		groupSubTitles: {
			Editor:
				"Примеры тулбара на сайте Quasar - https://quasar.dev/vue-components/editor",
			Sanitizer:
				"При публикации все html тексты (материалы, комментарии и др.) очищаются от вредоносных скриптов и всего что не входит в разрешённые списки."
		},
		items: {
			"Global:Locale": "Язык интерфейса",
			"Global:SiteName": "Имя сайта",
			"Global:PageTitleTemplate":
				"Шаблон заголовка (title) страниц сайта в браузере",
			"Global:SiteTitle": "Заголовок сайта",
			"Global:SiteSubTitle": "Подзаголовок сайта",
			"Global:OpenExternalLinksAtNewTab": "Открывать внешние ссылки в новом окне",
			"Global:IconsSet": "Иконки",
			"Dev:ShowExceptions": "Показывать исключения в логах",
			"Dev:LogInitExtended": "Логировать инициализацию расширено",
			"Dev:LogMoveTo": "Логировать перемещения по сайту",
			"Dev:LogRequests": "Логировать запросы",
			"Dev:VueAppInWindow": "Доступ к Vue app из консоли",
			"Dev:VueDevTools": "Разрешить VueDevTools",
			"Images:AllowGifUpload": 'Разрешить загрузку "gif" изображений',
			"Images:AllowSvgUpload": 'Разрешить загрузку "svg" изображений',
			"Images:AvatarSizePixels": "Сторона квадрата аватары в пикселях",
			"Images:ImageRequestSizeLimitBytes":
				"Максимальный размер изображения в байтах",
			"Images:MaxImageHeight": "Максимальная высота изображения до сжатия в px",
			"Images:MaxImageWidth": "Максимальная ширина изображения до сжатия в px",
			"Images:PhotoMaxHeightPixels":
				"Высота фотографии пользователя после сжатия px",
			"Images:PhotoMaxWidthPixels":
				"Ширина фотографии пользователя после сжатия px",
			"Images:ResizeMaxHeightPixels": "Высота изображения после сжатия px",
			"Images:ResizeMaxWidthPixels": "Ширина изображения после сжатия px",
			"Sanitizer:AllowedAttributes": "Разрешённые аттрибуты html",
			"Sanitizer:AllowedClasses": "Разрешённые классы html",
			"Sanitizer:AllowedCssProperties": "Разрешённые свойства css",
			"Sanitizer:AllowedImageDomains": "Разрешённые домены для изображений",
			"Sanitizer:AllowedTags": "Разрешённые теги html",
			"Sanitizer:AllowedVideoDomains": "Разрешённые домены для видео",
			"Sanitizer:AllowedSchemes": "Разрешённые схемы URL",
			"Email:EmailFromAddress": "Почтовый адрес от",
			"Email:EmailFromName": "Сообщение от имени",
			"Email:Host": "Хост",
			"Email:Login": "Логин",
			"Email:Password": "Пароль",
			"Email:Port": "Порт",
			"Email:UseSSL": "Использовать SSL",
			"Captcha:CaptchaTimeoutSeconds": "Интервал между пробами в секундах",
			"Editor:MaterialToolbar": "Тулбар написания материала",
			"Editor:CommentToolbar": "Тулбар написания комментария",
			"Editor:UserInformationToolbar": "Тулбар редактирования личной информации",
			"Editor:SendPrivateMessageToolbar": "Тулбар написания приватных сообщений",
			"Scheduler:ExpiredRegistrationUsersClearDays":
				"Интервал очистки не подтвердивших регистрацию пользователей в днях",
			"Scheduler:JwtBlackListServiceClearMinutes":
				"Интервал очистки чёрного списка Jwe в минутах",
			"Scheduler:LogJobs": "Логировать задачи на сервере",
			"Scheduler:LongSessionsClearDays": "Интервал очистки истёкших сессий в днях",
			"Scheduler:SpamProtectionCacheClearMinutes":
				"Интервал очистки кеша защиты от спама в минутах",
			"Scheduler:UploadVisitsToDataBaseMinutes":
				"Интервал заливки кеша посещений материала на сервер в минутах",
			"Materials:CommentsPageSize": "Количество комментариев на странице",
			"Materials:SubTitleLength": "Длинна подзаголовка",
			"Materials:TimeToOwnDeleteInMinutes":
				"Время в течении которого можно удалять свои сообщения в минутах",
			"Materials:TimeToOwnEditInMinutes":
				"Время в течении которого можно редактировать свои сообщения в минутах",
			"Materials:TimeToOwnMoveInMinutes":
				"Время в течении которого можно перемещать свои сообщения в минутах",
			"Comments:TimeToOwnDeleteInMinutes":
				"Время в течении которого можно удалять свои комментарии в минутах",
			"Comments:TimeToOwnEditInMinutes":
				"Время в течении которого можно редактировать свои комментарии в минутах",
			"Blog:PostsPageSize": "Размер страницы постов",
			"Blog:PreviewLength": "Длинна превьюшки в символах",
			"Articles:CategoryPageSize": "Количество статей на странице",
			"Forum:NewTopicsMaxPages":
				"Максимальное количетсво страниц на вкладке новых тем",
			"Forum:NewTopicsPageSize": "Количество тем на вкладке новых тем",
			"Forum:ThreadMaterialsPageSize": "Количество тем на вкладке раздела",
			"Jwe:Issuer": "Эмитент (Issuer)",
			"Jwe:LongTokenLiveTimeDays": "Длительность сессии в днях",
			"Jwe:ShortTokenLiveTimeMinutes":
				"Длительность жизни access токена в минутах",
			"Skins:CurrentSkinName": "Основная тема",
			"Skins:PartialSkinsNames": "Дополнительные темы",
			"Skins:MaxArchiveSizeKb": "Максимальный размер файла архива в кб",
			"Skins:MaxExtractArchiveSizeKb":
				"Максимальный размер архива после разархивации в кб",
			"Cache:CurrentCachePolicy": "Политика кэширования",
			"Cache:InvalidateCacheTime": "Время хранения кэш-записи"
		},
		tooltips: {
			"Global:PageTitleTemplate":
				"Для подстановки использовать {pageTitle} и {siteName}.",
			"Global:OpenExternalLinksAtNewTab": "В материалах, комментариях, постах",
			"Images:MaxImageHeight": "Проверка при заливки изображения на сервер.",
			"Images:MaxImageWidth": "Проверка при заливки изображения на сервер.",
			"Sanitizer:AllowedSchemes": "Например: mailto,skype",
			"Cache:InvalidateCacheTime":
				"Этот параметр используется не всеми политиками. Значения ниже 0 интерпретируются как необходимость постоянного хранения записей.",
			"Dev:LogInitExtended": "В консоле браузера.",
			"Dev:LogMoveTo": "В консоле браузера.",
			"Dev:LogRequests": "В консоле браузера.",
			"Dev:VueAppInWindow": "В консоле браузера.",
			"Dev:VueDevTools":
				"После изменения настроек необходимо сбросить кеш и перезагрузить страницу (ctrl + f5) с закрытым DevTools",
			"Skins:CurrentSkinName": "Устанавливается через админку тем.",
			"Skins:PartialSkinsNames":
				"Устанавливается через админку тем. Список через запятую.",
			"Skins:MaxArchiveSizeKb": "Проверка темы на при заливки на сервер.",
			"Skins:MaxExtractArchiveSizeKb": "Проверка темы на при заливки на сервер."
		}
	},
	CypherSecrets: {
		title: "Сбросить ключи шифрования"
	},
	DeletedElements: {
		title: "Удалённые элементы",
		showDeleted: "Показать удалённые элементы на страницах",
		info1:
			"Если галка установлена, во всех категориях будут показываться стёртые материалы.",
		info2:
			"Если в URL любого раздела добавить '?deleted=1' - стёртые материалы будут показаны в рамках этой ссылки.",
		info3:
			"Функция не предусмотрена для разделов, где выводятся сразу несколько категорий, например 'новые сообщения' на форуме.",
		btnDeleteAllMarkedComments: "Очистить базу от удаленных материалов",
		deleteSuccess:
			"Успешно очищено \nМатериалов: {materialsCount}\nКомментариев: {commentsCount}"
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
		partialSkins: "Дополнительные темы",
		customCss: "Произвольный CSS"
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
		upload: "Загрузить основную тему",
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
		upload: "Загрузить дополнительную тему"
	},
	CustomCssAdmin: {
		title: "Произвольный CSS",
		cssInput: "Произвольный CSS",
		saveBtn: "@:Global.btn.save",
		clearBtn: "Очистить",
		refreshBtn: "Перегрузить с сервера",
		reloadSuccessNotify: "Данные перезагружены с сервера",
		successNotify: "Данные успешно сохранены"
	}
};
