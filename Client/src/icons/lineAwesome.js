const global = {
	delete: "la la-trash",
	save: "la la-save",
	cancel: "la la-times",
	refresh: "la la-sync-alt",
	goTo: "la la-arrow-right",
	search: "las la-search",
	eye: "la la-eye",
	eyeSlash: "la la-eye-slash",
	upload: "la la-upload",
	add: "la la-plus",
	clock: "la la-clock",
	user: "la la-user",
	send: "la la-arrow-circle-right",
	comment: "la la-comment-alt",
	category: "la la-box",
	categories: "la la-boxes",
	key: "la la-key",
	envelope: "la la-envelope",
	wrench: "la la-wrench",
	tooltip: "la la-question-circle",
    fullscreen:  'las la-compress',
    fullscreenExit:  'las la-arrows-alt'
};

export default {
	name: "line-awesome",
	sunName: "LineAwesome",
	pickUrl: "https://icons8.com/line-awesome",
	global: global,
	Activity: {
		category: global.category,
		comment: global.comment,
		material: "la la-file-alt",
		publishDate: global.clock
	},
	AdminInformation: {
		support: "la la-heart"
	},
	AdminMenu: {
		AdminInformation: "la la-info-circle",
		CategoriesAdmin: "la la-boxes",
		SectionsAdmin: "la la-stream",
		ConfigurationAdmin: "la la-sliders-h",
		CypherSecrets: global.key,
		DeletedElements: global.delete,
		ImagesCleaner: "la la-broom",
		MenuItemsAdmin: "la la-bars",
		resetCache: global.refresh,
		RolesPage: "la la-user-friends",
		RolesPermissions: "la la-user-lock",
		SkinsAdmin: "la la-palette",
		systemTools: "la la-tools"
	},
	Article: {
		comments: global.comment,
		delete: global.delete,
		publishDate: global.clock,
		user: global.user
	},
	ArticlesMultiCatPage: {
		add: global.add
	},
	ArticlesPage: {
		add: global.add
	},
	BlogMultiCatPage: {
		add: global.add
	},
	BlogPage: {
		add: global.add
	},
	Captcha: {
		hand: "la la-hand-point-right",
		refresh: global.refresh
	},
	CategoriesAdmin: {
		addCategoryBtn: global.add
	},
	CategoryForm: {
		category: global.category,
		icons: "la la-icons",
		noIcon: "la la-stop",
		layout: "la la-table"
	},
	CategoryItem: {
		down: "la la-angle-down",
		edit: global.wrench,
		eyeSlash: global.eyeSlash,
		goTo: global.goTo,
		material: "la la-file-alt",
		plus: global.add,
		up: "la la-angle-up"
	},
	CategoriesInput: {
		category: global.category,
		categories: global.categories
	},
	ChangeEmail: {
		envelope: global.envelope,
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		key: global.key,
		save: global.save
	},
	ChangeEmailResult: {
		failed: "la la-exclamation-circle",
		success: "la la-check-circle"
	},
	ChangeLink: {
		link: "la la-link",
		save: global.save
	},
	ChangeName: {
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		key: global.key,
		save: global.save,
		user: global.user
	},
	ChangePassword: {
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		key: global.key,
		save: global.save
	},
	Comment: {
		delete: global.delete,
		publishDate: global.clock
	},
	SectionForm: {
		section: "la la-stream"
	},
	SectionsAdmin: {
		add: global.add,
		component: "la la-cube",
		edit: global.wrench,
		goTo: global.goTo
	},
	ConfigItem: {
		pretty: "la la-broom"
	},
	ConfigurationAdmin: {
		cancel: global.cancel,
		question: global.tooltip,
		reset: global.refresh,
		save: global.save,
		search: global.search
	},
	CreateCategory: {
		cancel: global.cancel,
		create: global.save
	},
	CreateComment: {
		send: global.send
	},
	CreateSection: {
		add: global.save,
		cancel: global.cancel
	},
	CreateMaterial: {
		cancel: global.cancel,
		save: global.save
	},
	CreateMenuItem: {
		cancel: global.cancel,
		create: global.save
	},
	CustomCssAdmin: {
		clean: "la la-broom",
		reset: global.refresh,
		save: global.save
	},
	CustomJavaScriptAdmin: {
		reset: global.refresh,
		save: global.save
	},
	CypherSecrets: {
		key: global.key
	},
	DeletedElements: {
		trashBtn: global.delete
	},
	EditCategory: {
		cancel: global.cancel,
		delete: global.delete,
		save: global.save
	},
	EditComment: {
		cancel: global.cancel,
		save: global.save
	},
	EditSection: {
		cancel: global.cancel,
		delete: global.delete,
		save: global.save
	},
	EditInformation: {
		save: global.save
	},
	EditMaterial: {
		cancel: global.cancel,
		delete: global.delete,
		save: global.save
	},
	EditMenuItem: {
		cancel: global.cancel,
		delete: global.delete,
		save: global.save
	},

	ImagesCleaner: {
		clean: global.delete,
		refresh: global.refresh
	},
	Layout: {
		heart: "la la-heart",
		mainMenu: "la la-bars",
		secondMenu: "la la-clipboard-list",
		user: "la la-user-alt"
	},
	LoaderWait: {
		exclamation: "la la-exclamation-triangle"
	},
	LoadPhoto: {
		delete: global.delete,
		upload: global.upload
	},
	Login: {
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		key: global.key,
		question: "la la-question-circle",
		user: global.user
	},
	LoginRegisterMenu: {
		register: "la la-user-plus",
		signIn: "la la-sign-in-alt"
	},
	MainSkinsAdmin: {
		current: "la la-check",
		delete: global.delete,
		info: "la la-info",
		preview: "la la-search",
		set: "la la-play",
		upload: global.upload
	},
	Material: {
		delete: global.delete,
		publishDate: global.clock,
		restore: "la la-trash-restore",
		visits: global.eye,
        fullscreen: global.fullscreen,
        fullscreenExit: global.fullscreenExit
	},
	MaterialForm: {
		category: "la la-stream",
		tags: "la la-tags"
	},
	MenuAdminItem: {
		add: global.add,
		blank: "la la-file",
		delete: global.delete,
		down: "la la-angle-down",
		edit: global.wrench,
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		goTo: global.goTo,
		up: "la la-angle-up"
	},
	MenuItemForm: {
		css: "lab la-css3-alt",
		icons: "la la-icons",
		noIcon: "la la-stop",
		link: "la la-link",
		search: global.search
	},
	MenuItemsAdmin: {
		add: global.add
	},
	PartialSkinsAdmin: {
		delete: global.delete,
		upload: global.upload
	},
	Post: {
		comment: global.comment,
		deleted: global.delete,
		hidden: global.eyeSlash,
		readMore: "la la-file-alt"
	},
	Profile: {
		ban: "la la-ban",
		envelope: global.envelope,
		menu: "la la-ellipsis-v",
		roles: "la la-cog",
		unBan: "la la-circle",
		visits: global.eye
	},
	ProfileRoles: {
		minus: "la la-minus",
		plus: global.add
	},
	Register: {
		envelope: global.envelope,
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		key: global.key,
		user: global.user
	},
	RegisterEmailResult: {
		failed: "la la-exclamation-circle",
		success: "la la-check-circle"
	},
	ResetPassword: {
		envelope: global.envelope,
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		key: global.key,
		save: global.save
	},
	ResetPasswordFailed: {
		failed: "la la-exclamation-circle"
	},
	ResetPasswordSetNew: {
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		key: global.key
	},
	RolesPage: {
		users: "la la-user-friends"
	},
	RolesPermissions: {
		reset: global.refresh,
		save: global.save
	},
	RoleUsers: {
		search: global.search,
		user: global.user
	},
	SendPrivateMessage: {
		cancel: global.cancel,
		send: global.send
	},
	Sessions: {
		clock: global.clock,
		machine: "la la-desktop",
		signOut: "la la-sign-out-alt"
	},
	SettingsMenu: {
		ban: "la fa-ban",
		envelope: global.envelope,
		information: "la la-info-circle",
		key: global.key,
		link: "la la-link",
		photo: "la la-portrait",
		profile: "la fa-address-card",
		sessions: "la la-ticket-alt",
		user: global.user
	},
	SkinsAdmin: {
		customCss: "lab la-css3-alt",
		main: "la la-palette",
		partial: "la la-puzzle-piece",
		customJavaScript: "lab la-js-square"
	},
	SunEditor: {
		addImages: "la la-image"
	},
	Thread: {
		add: global.add
	},
	Topic: {
		category: global.category,
		comment: global.comment,
		deleted: global.delete,
		hidden: global.eyeSlash,
		publishDate: global.clock
	},
	UserMenu: {
		admin: "la la-cog",
		personal: "la la-address-card",
		profile: "la la-user-circle",
		signOut: "la la-sign-out-alt"
	}
};
