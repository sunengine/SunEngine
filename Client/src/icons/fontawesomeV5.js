const global = {
	delete: "fas fa-trash-alt",
	save: "far fa-save",
	cancel: "fas fa-times",
	refresh: "fas fa-sync-alt",
	goTo: "fas fa-arrow-right",
	search: "fas fa-search",
	eye: "far fa-eye",
	eyeSlash: "far fa-eye-slash",
	upload: "fas fa-cloud-upload-alt",
	add: "fas fa-plus",
	clock: "far fa-clock",
	user: "far fa-user",
	users: "fas fa-user",
	send: "fas fa-arrow-circle-right",
	comment: "far fa-comment",
	category: "fas fa-folder",
	key: "fas fa-key",
	envelope: "fas fa-envelope",
    wrench: "fas fa-wrench"
};

export default {
	name: "fontawesome-v5",
	sunName: "FontAwesome",
	global: global,
	Activity: {
		category: global.category,
		comment: global.comment,
		material: "far fa-file-alt",
		publishDate: global.clock
	},
	AdminMenu: {
		AdminInformation: "fas fa-info-circle",
		CategoriesAdmin: global.category,
		ComponentsAdmin: "fas fa-shapes",
		ConfigurationAdmin: "fas fa-sliders-h",
		CypherSecrets: global.key,
		DeletedElements: "fas fa-trash",
		ImagesCleaner: "fas fa-broom",
		MenuItemsAdmin: "fas fa-bars",
		resetCache: global.refresh,
		RolesPage: "fas fa-user-friends",
		RolesPermissions: "fas fa-user-shield",
		SkinsAdmin: "fas fa-palette",
		systemTools: "fas fa-tools"
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
		hand: "fas fa-hand-point-right",
		refresh: global.refresh
	},
	CategoriesAdmin: {
		addCategoryBtn: global.add
	},
	CategoryForm: {
		category: global.category,
		icons: "fas fa-icons",
		layout: "fas fa-table"
	},
	CategoryItem: {
		down: "fas fa-chevron-down",
		edit: global.wrench,
		eyeSlash: global.eyeSlash,
		goTo: global.goTo,
		material: "far fa-file-alt",
		plus: global.add,
		up: "fas fa-chevron-up"
	},
	ChangeEmail: {
		envelope: global.envelope,
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		key: global.key,
		save: global.save
	},
	ChangeEmailResult: {
		failed: "fas fa-exclamation-circle",
		success: "fas fa-check-circle"
	},
	ChangeLink: {
		link: "fas fa-link",
		save: global.save
	},
	ChangeName: {
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		key: global.key,
		save: global.save,
		user: global.users
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
	ComponentForm: {
		component: "fas fa-cube"
	},
	ComponentsAdmin: {
		add: global.add,
		component: "fas fa-cube",
		edit: global.wrench,
		goTo: global.goTo
	},
	ConfigurationAdmin: {
		cancel: global.cancel,
		question: "far fa-question-circle",
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
	CreateComponent: {
		add: global.save,
		cancel: global.cancel
	},
	CreateMaterial: {
		cancel: global.cancel,
		save: global.save
	},
	CreateMenuItem: {
		cancel: global.cancel,
		create: global.add
	},
	CustomCssAdmin: {
		clean: "fas fa-snowplow",
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
	EditComponent: {
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
		heart: "fas fa-heart",
		mainMenu: "la la-bars",
		secondMenu: "fa fa-clipboard-list",
		user: global.users
	},
	LoaderWait: {
		exclamation: "fas fa-exclamation-triangle"
	},
	LoadPhoto: {
		delete: global.delete,
		upload: global.upload
	},
	Login: {
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		key: global.key,
		question: "far fa-question-circle",
		user: global.users
	},
	LoginRegisterMenu: {
		register: "fas fa-user-plus",
		signIn: "fas fa-sign-in-alt"
	},
	MainSkinsAdmin: {
		current: "fas fa-check",
		delete: global.delete,
		info: "fas fa-info",
		preview: global.search,
		set: "fas fa-play",
		upload: global.upload
	},
	Material: {
		delete: global.delete,
		publishDate: global.clock,
		restore: "fas fa-trash-restore",
		visits: global.eye
	},
	MaterialForm: {
		category: global.category,
		tags: "fas fa-tags"
	},
	MenuAdminItem: {
		add: global.add,
		blank: "far fa-file",
		delete: global.delete,
		down: "fa fa-angle-down",
		edit: global.wrench,
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		goTo: global.goTo,
		up: "fa fa-angle-up"
	},
	MenuItemForm: {
		css: "fab fa-css3-alt",
		icons: "fas fa-icons",
		link: "fas fa-link",
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
		readMore: "far fa-file-alt"
	},
	Profile: {
		ban: "fas fa-ban",
		envelope: global.envelope,
		menu: "fas fa-ellipsis-v",
		roles: "fas fa-cog",
		unBan: "far fa-circle"
	},
	ProfileRoles: {
		minus: "fas fa-minus",
		plus: global.add
	},
	Register: {
		envelope: global.envelope,
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		key: global.key,
		user: global.users
	},
	RegisterEmailResult: {
		failed: "fas fa-exclamation-circle",
		success: "fas fa-check-circle"
	},
	ResetPassword: {
		envelope: global.envelope,
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		key: global.key,
		save: global.save
	},
	ResetPasswordFailed: {
		failed: "fas fa-exclamation-circle"
	},
	ResetPasswordSetNew: {
		eye: global.eye,
		eyeSlash: "far fa-eye-slash",
		key: global.key
	},
	RolesPage: {
		users: "fas fa-user-friends"
	},
	RolesPermissions: {
		reset: global.refresh,
		save: global.save
	},
	RoleUsers: {
		search: global.search,
		user: global.users
	},
	SendPrivateMessage: {
		cancel: global.cancel,
		send: global.send
	},
	Sessions: {
		clock: global.clock,
		machine: "fas fa-desktop",
		signOut: "fas fa-sign-out-alt"
	},
	SettingsMenu: {
		ban: "fas fa-ban",
		envelope: global.envelope,
		information: "fas fa-info-circle",
		key: global.key,
		link: "fas fa-link",
		photo: "fas fa-portrait",
		profile: "fas fa-address-card",
		sessions: "fas fa-ticket-alt",
		user: global.users
	},
	SkinsAdmin: {
		customCss: "fab fa-css3-alt",
		main: "fas fa-user-astronaut",
		partial: "fas fa-puzzle-piece"
	},
	SunEditor: {
		addImages: "fas fa-image"
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
		admin: "fas fa-cog",
		personal: "fas fa-address-card",
		profile: "fas fa-user-circle",
		signOut: "fas fa-sign-out-alt"
	}
};
