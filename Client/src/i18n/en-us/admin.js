export default {
	// ——— categories ————————————————————————————————————

	CategoriesAdmin: {
		title: "Categories admin",
		addCategoryBtn: "Add category",
		showInfo: "Information about categories"
	},
	CategoryForm: {
		name: "Id name (eng)",
		title: "Title",
		subTitle: "Subtitle",
		token: "URL token",
		appendTokenToSubCatsPath: "Append token to sub categories path's",
		showInBreadcrumbs: "Show in breadcrumbs",
		icon: "Icon",
		header: "Header",
		selectParent: "Parent category",
		deleteConfirm: "You want to remove the category?",
		parent: "Parent: ",
		hideCb: "Hide",
		settingsJson: "Json settings",
		appendUrlTokenCb: "Add to URL",
		appendUrlTokenInfo: "(use only if you understand what it is)",
		isMaterialsContainerCb: "Contains materials",
		isMaterialsSubTitleEditableCb: "Possibility to edit material subtitle",
		isMaterialsNameEditableCb:
			"Possibility to edit material name (eng), only for admin",
		isCaching: "Caching",
		cachingPageCount: "Cache N pages",
		noTypeLabel: "Without type",
		layout: "Layout",
		validation: {
			name: {
				required: "Enter category name (eng)",
				minLength: "Name (eng) must be at least 2 letters",
				maxLength: `Имя (eng) must contain max ${config.DbColumnSizes.Categories_Name} chars`,
				allowedChars:
					"The name (eng) must consist of the characters `a-z`, `A-Z`, `0-9`, `-`"
			},
			token: {
				allowedChars:
					"URL token must consist of the characters `a-z`, `A-Z`, `0-9`, `-`",
				maxLength: `URL token must contain max ${config.DbColumnSizes.Categories_Token} chars`
			},
			title: {
				required: "Enter category title",
				minLength: "Category title must contain at least 3 letters",
				maxLength: `Title must contain max ${config.DbColumnSizes.Categories_Title} letter`
			},
			subTitle: {
				maxLength: `Subtitle must contain max ${config.DbColumnSizes.Categories_SubTitle} letter`
			},
			icon: {
				minLength: "Minimal icon length - 3",
				maxLength: "Maximum icon length - " + config.DbColumnSizes.Categories_Icon
			},
			parent: {
				required: "Select parent category"
			},
			settingsJson: {
				jsonFormatError: "@:Global.validation.jsonFormatError"
			}
		}
	},
	CategoryItem: {
		rootCategory: "Root category"
	},
	CreateCategory: {
		title: "Create category",
		createBtn: "Create",
		cancelBtn: "@:Global.btn.cancel",
		successNotify: "Category was added."
	},
	EditCategory: {
		title: "Edit category",
		deleteBtn: "Remove category",
		saveBtn: "@:Global.btn.save",
		cancelBtn: "@:Global.btn.cancel",
		deletedNotify: "Category successfully removed.",
		deleteConfirm: "You want to remove the category?",
		deleteDialogBtnOk: "Remove",
		deleteDialogBtnCancel: "Cancel",
		successNotify: "Category was updated."
	},

	// ——— components ———————————————————————————————————

	ComponentForm: {
		name: "Name (eng)",
		type: "Type",
		isCacheData: "Cache data",
		serverSettingsJson: "Server settings JSON",
		clientSettingsJson: "Client settings JSON",
		roles: "Roles to access",
		validation: {
			name: {
				required: "Enter name (eng)",
				minLength: "Minimum component name length is 3",
				maxLength:
					"Maximum component name length is " +
					config.DbColumnSizes.Components_Name +
					" chars",
				allowedChars:
					"The name (eng) must consist of the characters `a-z`, `A-Z`, `0-9`, `-`, `_`"
			},
			type: {
				required: "Select type"
			},
			jsonFormatError: "@:Global.validation.validation"
		}
	},
	ComponentsAdmin: {
		title: "Components admin",
		addComponentBtn: "Add component"
	},
	CreateComponent: {
		title: "Create component",
		createBtn: "@:Global.btn.create",
		cancelBtn: "@:Global.btn.cancel"
	},
	EditComponent: {
		title: "Update component",
		saveBtn: "@:Global.btn.save",
		cancelBtn: "@:Global.btn.cancel",
		deleteBtn: "@:Global.btn.delete",
		deleteMsg: "Remove component?",
		btnDeleteOk: "@:Global.dialog.ok",
		btnDeleteCancel: "@:Global.dialog.cancel"
	},

	// ——— menuItems ————————————————————————————————————

	CreateMenuItem: {
		title: "Create menu item",
		createBtn: "@:Global.btn.create",
		cancelBtn: "@:Global.btn.cancel",
		successNotify: "Menu item successfully created"
	},
	EditMenuItem: {
		title: "Edit menu item",
		saveBtn: "@:Global.btn.save",
		cancelBtn: "@:Global.btn.cancel",
		deleteBtn: "Delete menu item",
		successNotify: "Menu item successfully edited",
		deleteMsg: "Delete menu item?",
		btnDeleteOk: "@:Global.dialog.ok",
		btnDeleteCancel: "@:Global.dialog.cancel"
	},
	MenuAdminItem: {
		moveUpBtnTooltip: "Move up",
		moveDownBtnTooltip: "Move down",
		editBtnTooltip: "Edit",
		changeIsHiddenBtnTooltip: "Show / Hide",
		addSubMenuItemBtnTooltip: "Add sub item",
		goToBtnTooltip: "Go to link",
		deleteBtnTooltip: "Delete menu item"
	},
	MenuItemForm: {
		name: "Name (eng)",
		title: "Title",
		subTitle: "Subtitle",
		parent: "Parent element",
		rootElement: "Root element",
		url: "Link internal or external",
		exact: "Highlight menu item only for exact match",
		roles: "Roles can see menu item",
		cssClass: "Css class",
		icon: "Icon",
		settingsJson: "Custom settings (Json)",
		isHidden: "Hide",
		local: "Local link",
		external: "External link",
		urlError: "Error in link",
		validation: {
			name: {
				minLength: "Minimal name length - 3",
				maxLength: "Maximum name length - " + config.DbColumnSizes.MenuItems_Name,
				allowedChars: "Only [a-zA-Z0-9_-] symbols allowed"
			},
			title: {
				required: "Title required",
				minLength: "Minimal title length - 3",
				maxLength: "Maximum title length - " + config.DbColumnSizes.Categories_Title
			},
			subTitle: {
				minLength: "Minimal subtitle length - 3",
				maxLength:
					"Maximum subtitle length - " + config.DbColumnSizes.MenuItems_SubTitle
			},
			cssClass: {
				minLength: "Minimal css class length - 3",
				maxLength:
					"Maximum css class length - " + config.DbColumnSizes.MenuItems_CssClass
			},
			icon: {
				minLength: "Minimal icon length - 3",
				maxLength: "Maximum icon length - " + config.DbColumnSizes.MenuItems_Icon
			},
			settingsJson: {
				jsonFormatError: "@:Global.validation.jsonFormatError"
			}
		}
	},
	MenuItemsAdmin: {
		title: "Menu editor",
		addMenuItemBtn: "Add menu item",
		deleteMsg: "Delete menu item?",
		btnDeleteOk: "@:Global.dialog.ok",
		btnDeleteCancel: "@:Global.dialog.cancel"
	},

	// ——— roles ————————————————————————————————————

	ProfileRoles: {
		roles: "User groups:",
		addRoleBtn: "Add to group",
		removeRoleBtn: "Remove from group",
		addRoleConfirmTitle: "",
		addRoleConfirmMessage: "Add to group '{roleName}'?",
		addRoleConfirmOkBtn: "Yes",
		addRoleConfirmCancelBtn: "@:Global.dialog.cancel",
		removeRoleConfirmTitle: "",
		removeRoleConfirmMessage: "Remove from group '{roleName}'?",
		removeRoleConfirmOkBtn: "Remove",
		removeRoleConfirmCancelBtn: "@:Global.dialog.cancel"
	},
	RolesPage: {
		title: "Groups",
		roles: "Groups"
	},
	RolesPermissions: {
		title: "Upload group permissions (json)",
		backupWarning: "Before uploading, you need to make a database backup.",
		saveToServerBtn: "Save",
		getFromServer: "Reload from server",
		getSuccessNotify: "Download completed successfully",
		saveSuccessNotify: "Group settings were updated successfully",
		textAreaLabel: "Json file of roles permissions"
	},
	RoleUsers: {
		users: "Users",
		filter: "Filter",
		noResults: "Not found",
		filterLimitReached: "First {0} results are derived"
	},

	// ——— all ————————————————————————————————————

	AdminMenu: {
		adminInformation: "Information",
		adminInformationCaption: "",
		menuItems: "Menu",
		menuItemsCaption: "",
		categories: "Categories",
		categoriesCaption: "",
		components: "Components",
		componentsCaption: "",
		skins: "Skins",
		skinsCaption: "",
		rolesUsers: "Groups",
		rolesUsersCaption: "",
		rolesPermissions: "Permission",
		rolesPermissionsCaption: "",
		configuration: "Configuration",
		configurationCaption: "",
		cypherSecrets: "Cypher keys",
		cypherSecretsCaption: "",
		imagesCleaner: "Disk cleaner",
		imagesCleanerCaption: "",
		deletedElements: "Deleted elements",
		deletedElementsCaption: "",
		version: "SunEngine version",
		resetCache: "Reset cache",
		resetCacheCaption: "",
		resetCacheSuccess: "Cache succesfully reseted"
	},
	AdminPage: {
		title: "@:AdminPanel.title"
	},
	AdminInformation: {
		title: "Information",
		serverName: "Server name",
		serverVersion: "Server version",
		serverRepository: "Server repository",
		sunEngineVersion: "SunEngine version",
		clientName: "Client name",
		clientVersion: "Client version",
		dotNetVersion: "DotNet version",
		quasarVersion: "Quasar version",
		vueJsVersion: "VueJS version",
		maintainer: "Site maintainer",
		maintainerContacts: "Maintainer contacts",
		description: "Description",
		sunEngineRepository: "SunEngine repository",
		sunEngineSkinsRepository: "SunEngine skins repository",
		additionalData: "Additional data"
	},
	AdminPanel: {
		title: "Admin panel"
	},
	ConfigurationAdmin: {
		title: "Site configuration",
		filter: "Filter",
		noResults: "Nothing found",
		successNotify: "Configuration values successfully saved",
		resetSuccessNotify: "Configuration values reloaded from server",
		resetBtn: "Reset",
		resetBtnTooltip: "Reset input and reload from server",
		cancelBtn: "@:Global.btn.cancel",
		saveBtn: "@:Global.btn.save",
		groupTitles: {
			Editor: "Editor",
			Global: "Global",
			Dev: "Development",
			Cache: "Cache",
			Images: "Images",
			Sanitizer: "Sanitize HTML",
			Email: "Email",
			Scheduler: "Functions scheduler",
			Materials: "Materials",
			Comments: "Comments",
			Forum: "Forum",
			Articles: "Articles",
			Captcha: "Captcha. Check if real user.",
			Blog: "Blog",
			Jwe: "Jwe security",
			Skins: "Skins"
		},
		groupSubTitles: {
			Editor:
				"Toolbar examples on Quasar site - https://quasar.dev/vue-components/editor",
			Sanitizer:
				"When publish all html texts (materials, comments and others) were cleaning against harmful scripts and all html elements except allowed in lists."
		},
		items: {
			"Global:Locale": "Interface language",
			"Global:SiteName": "Site name",
			"Global:SiteTitle": "Site title",
			"Global:SiteSubTitle": "Site subtitle",
			"Global:IconsSet": "Icon set",
			"Dev:ShowExceptions": "Show exceptions",
			"Images:AllowGifUpload": 'Allow "gif" images upload',
			"Images:AllowSvgUpload": 'Allow "svg" images upload',
			"Images:AvatarSizePixels": "Avatar size in pixels (square avatar)",
			"Images:ImageRequestSizeLimitBytes": "Maximum image size in bytes",
			"Images:MaxImageHeight":
				"Check: maximum image height before compression in px",
			"Images:MaxImageWidth":
				"Check: maximum image width before compression in px",
			"Images:PhotoMaxHeightPixels":
				"The height of the user's photo after px compression",
			"Images:PhotoMaxWidthPixels":
				"Width of the user's photo after px compression",
			"Images:ResizeMaxHeightPixels": "Image height after px compression",
			"Images:ResizeMaxWidthPixels": "Image width after px compression",
			"Sanitizer:AllowedAttributes": "Allowed html attributes",
			"Sanitizer:AllowedClasses": "Allowed html classes",
			"Sanitizer:AllowedCssProperties": "Allowed css properties",
			"Sanitizer:AllowedImageDomains": "Allowed images domains",
			"Sanitizer:AllowedTags": "Allowed html tags",
			"Sanitizer:AllowedVideoDomains": "Allowed video domains",
			"Email:EmailFromAddress": "Email address From",
			"Email:EmailFromName": "Message from",
			"Email:Host": "Host",
			"Email:Login": "Login",
			"Email:Password": "Password",
			"Email:Port": "Port",
			"Captcha:CaptchaTimeoutSeconds": "Captcha tries interval in seconds",
			"Editor:MaterialToolbar": "Material editor toolbar",
			"Editor:CommentToolbar": "Comment editor toolbar",
			"Editor:UserInformationToolbar": "Personal information editor toolbar",
			"Editor:SendPrivateMessageToolbar": "Send private message toolbar",
			"Scheduler:ExpiredRegistrationUsersClearDays":
				"Interval for cleaning up users who did not confirm registration in days",
			"Scheduler:JwtBlackListServiceClearMinutes":
				"Jwe blacklist cleanup interval in minutes",
			"Scheduler:LogJobs": "Log tasks on the server",
			"Scheduler:LongSessionsClearDays":
				"Interval to clear expired sessions in days",
			"Scheduler:SpamProtectionCacheClearMinutes":
				"Interval for clearing the anti-spam cache in minutes",
			"Scheduler:UploadVisitsToDataBaseMinutes":
				"Interval to upload visits cache to database",
			"Materials:CommentsPageSize": "Number of comments per page",
			"Materials:SubTitleLength": "Subtitle length",
			"Materials:TimeToOwnDeleteInMinutes":
				"The time during which you can delete your messages in minutes",
			"Materials:TimeToOwnEditInMinutes":
				"The time during which you can edit your messages in minutes",
			"Materials:TimeToOwnMoveInMinutes":
				"The time during which you can move your messages in minutes",
			"Comments:TimeToOwnDeleteInMinutes":
				"Time during which you can delete your comments in minutes",
			"Comments:TimeToOwnEditInMinutes":
				"Time during which you can edit your comments in minutes",
			"Blog:PostsPageSize": "Post page size",
			"Blog:PreviewLength": "Long thumbnails in symbols",
			"Articles:CategoryPageSize": "Number of articles on page",
			"Forum:NewTopicsMaxPages": "Maximum number of pages in the new topics tab",
			"Forum:NewTopicsPageSize": "The number of topics in the new topics tab",
			"Jwe:Issuer": "",
			"Jwe:LongTokenLiveTimeDays":
				"Duration of the session in days refresh token life",
			"Jwe:ShortTokenLiveTimeMinutes": "Access token lifetime",
			"Skins:CurrentSkinName": "Main theme",
			"Skins:PartialSkinsNames": "Additional themes",
			"Skins:MaxArchiveSizeKb": "Check: maximum archive size in kb",
			"Skins:MaxExtractArchiveSizeKb":
				"Check: maximum archive size after unzipping in kb",
			"Cache:CurrentCachePolicy": "Cache policy",
			"Cache:InvalidateCacheTime": "Retention time of cache entry"
		},
		tooltips: {
			"Global:PageTitleTemplate":
				"Use tokens {pageTitle} and {siteName} for replace parts.",
			"Global:OpenExternalLinksAtNewTab": "In meterials, comments and posts.",
			"Images:MaxImageHeight": "Check when upload to server.",
			"Images:MaxImageWidth": "Check when upload to server.",
			"Sanitizer:AllowedSchemes": "Example: mailto,skype",
			"Dev:LogInitExtended": "In browser console",
			"Dev:LogMoveTo": "In browser console",
			"Dev:LogRequests": "In browser console",
			"Dev:VueAppInWindow": "In browser console",
			"Dev:VueDevTools":
				"After change needed to clean site cache and reload page (ctrl + f5) with closed DevTools",
			"Skins:CurrentSkinName": "Set from skins admin.",
			"Skins:PartialSkinsNames": "Set from skins admin. Comma separated list.",
			"Skins:MaxArchiveSizeKb": "Skin check when uploading on server.",
			"Skins:MaxExtractArchiveSizeKb": "Skin check when uploading on server.",
			"Cache:InvalidateCacheTime":
				"This option is not used by all policies. Values of 0 or lower are interpreted as requiring permanent storage of cache entry."
		}
	},
	CypherSecrets: {
		title: "Reset cypher keys"
	},
	DeletedElements: {
		title: "Deleted elements",
		showDeleted: "Show deleted elements on pages",
		info1: "If checked, deleted materials will shows on any single category.",
		info2:
			"If append '?deleted=1' to any single category URL deleted material will shows.",
		info3:
			"This function will not work on multi categories sections like 'new topics' on forum.",
		btnDeleteAllMarkedComments: "Clean database from deleted materials",
		deleteSuccess:
			"Success clean \nMaterials: {materialsCount}\nComments: {commentsCount}"
	},
	ImagesCleaner: {
		title: "Disk cleaner",
		info: "Lost images not using on site",
		working: "Clearing",
		clearBtn: "Delete lost images",
		refreshBtn: "Refresh lost images list",
		clearCount: "Cleared images: ",
		emptyResult: "Lost images not found"
	},
	SkinsAdmin: {
		title: "Skins admin",
		mainSkins: "Main skins",
		partialSkins: "Partial skins",
		customCss: "Custom CSS"
	},
	MainSkinsAdmin: {
		title: "Skins admin",
		current: "Current",
		info: "Skins collection and documentation to create own skin - ",
		author: "Author: ",
		contacts: "Contacts: ",
		description: "Description: ",
		link: "Link to source.",
		version: "Version: ",
		upload: "Upload main skin",
		uploadSuccessNotify: "Skin uploaded successfully",
		deleteSuccessNotify: "Skin deleted successfully",
		set: "Set",
		deleteMsg: "Delete skin?",
		btnDeleteOk: "@:Global.dialog.yes",
		btnDeleteCancel: "@:Global.dialog.cancel"
	},
	PartialSkinsAdmin: {
		title: "Partial skins",
		onBtn: "Enable",
		offBtn: "Disable",
		deleteMsg: "Delete partial skin?",
		btnDeleteOk: "@:Global.dialog.yes",
		btnDeleteCancel: "@:Global.dialog.cancel",
		upload: "Upload partial skin"
	},
	CustomCssAdmin: {
		title: "Custom CSS",
		cssInput: "Custom CSS",
		saveBtn: "@:Global.btn.save",
		clearBtn: "Clear",
		refreshBtn: "Reload from server",
		reloadSuccessNotify: "Css reloaded from server",
		successNotify: "Css successfully updated"
	}
};
