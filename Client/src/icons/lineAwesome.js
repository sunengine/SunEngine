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
	send: "la la-arrow-circle-right"
};

export default {
	name: "line-awesome",
	sunName: "LineAwesome",
	AdminMenu: {
		AdminInformation: "la la-info-circle",
		ConfigurationAdmin: "la la-sliders-h",
		MenuItemsAdmin: "la la-bars",
		CategoriesAdmin: "la la-stream",
		ComponentsAdmin: "la la-shapes",
		SkinsAdmin: "la la-palette",
		RolesPage: "la la-user-friends",
		RolesPermissions: "la la-user-lock",
		DeletedElements: "la la-trash",
		resetCache: "la la-sync-alt",
		systemTools: "la la-tools",
		ImagesCleaner: "la la-broom",
		CypherSecrets: "la la-key"
	},
	SunEditor: {
		addImages: "la la-image"
	},
	CategoriesAdmin: {
		addCategoryBtn: global.add
	},
	CategoryItem: {
		up: "la la-angle-up", //"la la-arrow-up", // "la la-caret-up", "la la-angle-up",
		down: "la la-angle-down", //"la la-arrow-up" // "la la-caret-down", "la la-angle-down"
		eyeSlash: global.eyeSlash,
		edit: "la la-wrench",
		plus: global.add,
		goTo: "la la-arrow-right",
		material: "la la-file-alt"
	},
	CreateCategory: {
		create: global.save,
		cancel: global.cancel
	},
	EditCategory: {
		save: global.save,
		cancel: global.cancel,
		delete: global.delete
	},
	CategoryForm: {
		icons: "la la-icons",
		category: "la la-stream",
		layout: "la la-table"
	},
	ConfigurationAdmin: {
		search: global.search,
		question: "la la-question-circle",
		save: global.save,
		reset: global.refresh,
		cancel: global.cancel
	},
	CreateMenuItem: {
		create: global.save,
		cancel: global.cancel
	},
	EditMenuItem: {
		save: global.save,
		cancel: global.cancel,
		delete: global.delete
	},
	MenuAdminItem: {
		up: "la la-angle-up", //"la la-arrow-up", // "la la-caret-up", "la la-angle-up",
		down: "la la-angle-down", //"la la-arrow-up" // "la la-caret-down", "la la-angle-down"
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		blank: "la la-file",
		edit: "la la-wrench",
		add: global.add,
		goTo: global.goTo,
		delete: global.delete
	},
	MenuItemsAdmin: {
		add: global.add
	},
	MenuItemForm: {
		link: "la la-link",
		css: "lab la-css3-alt",
		icons: "la la-icons",
		search: global.search
	},
	ComponentsAdmin: {
		add: global.save,
		component: "la la-cube",
		edit: "la la-wrench",
		goTo: global.goTo
	},
	ComponentForm: {
		component: "la la-cube"
	},
	CreateComponent: {
		add: global.save,
		cancel: global.cancel
	},
	EditComponent: {
		save: global.save,
		cancel: global.cancel,
		delete: global.delete
	},
	SkinsAdmin: {
		main: "la la-palette",
		partial: "la la-puzzle-piece",
		customCss: "lab la-css3-alt"
	},
	MainSkinsAdmin: {
		upload: "la la-upload",
		preview: "la la-search",
		current: "la la-check",
		set: "la la-play",
		info: "la la-info",
		delete: global.delete
	},
	PartialSkinsAdmin: {
		upload: global.upload,
		delete: global.delete
	},
	CustomCssAdmin: {
		save: global.save,
		reset: global.refresh,
		clean: "la la-broom"
	},
	RolesPage: {
		users: "la la-user-friends"
	},
	RoleUsers: {
		user: "la la-user",
		search: global.search
	},
	ProfileRoles: {},
	RolesPermissions: {
		save: global.save,
		reset: global.refresh
	},
	DeletedElements: {
		trashBtn: global.delete
	},
	ImagesCleaner: {
		clean: global.delete,
		refresh: global.refresh
	},
	CypherSecrets: {
		key: "la la-key"
	},

	Material: {
		delete: global.delete,
		restore: "la la-trash-restore",
		visits: global.eye,
		publishDate: global.clock
	},
	Comment: {
		delete: global.delete,
		publishDate: global.clock
	},
	Article: {
		delete: global.delete,
		publishDate: global.clock,
		comments: "la la-comment",
		user: global.user
	},
	ArticlesPage: {
		add: global.add
	},
	ArticlesMultiCatPage: {
		add: global.add
	},
	CreateComment: {
		send: global.send
	},
	EditComment: {
		save: global.save,
		cancel: global.cancel
	},
};
