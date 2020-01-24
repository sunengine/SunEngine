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
	clock: "la la-clock"
};

export default {
	name: "line-awesome",
	sunName: "LineAwesome",
	AdminMenu: {
		AdminInformation: "las la-info-circle",
		ConfigurationAdmin: "las la-sliders-h",
		MenuItemsAdmin: "las la-bars",
		CategoriesAdmin: "las la-stream",
		ComponentsAdmin: "las la-shapes",
		SkinsAdmin: "las la-palette",
		RolesPage: "las la-user-friends",
		RolesPermissions: "las la-user-lock",
		DeletedElements: "las la-trash",
		resetCache: "las la-sync-alt",
		systemTools: "las la-tools",
		ImagesCleaner: "las la-broom",
		CypherSecrets: "las la-key"
	},
	SunEditor: {
		addImages: "las la-image"
	},
	CategoriesAdmin: {
		addCategoryBtn: global.add
	},
	CategoryItem: {
		up: "la la-angle-up", //"la la-arrow-up", // "la la-caret-up", "la la-angle-up",
		down: "la la-angle-down", //"la la-arrow-up" // "la la-caret-down", "la la-angle-down"
		eyeSlash: global.eyeSlash,
		edit: "las la-wrench",
		plus: global.add,
		goTo: "las la-arrow-right",
		material: "las la-file-alt"
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
		icons: "las la-icons",
		category: "las la-stream",
		layout: "las la-table"
	},
	ConfigurationAdmin: {
		search: global.search,
		question: "las la-question-circle",
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
		blank: "las la-file",
		edit: "las la-wrench",
		add: global.add,
		goTo: global.goTo,
		delete: global.delete
	},
	MenuItemsAdmin: {
		add: global.add
	},
	MenuItemForm: {
		link: "las la-link",
		css: "lab la-css3-alt",
		icons: "las la-icons",
		search: global.search
	},
	ComponentsAdmin: {
		add: global.save,
		component: "las la-cube",
		edit: "las la-wrench",
		goTo: global.goTo
	},
	ComponentForm: {
		component: "las la-cube"
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
		main: "las la-palette",
		partial: "las la-puzzle-piece",
		customCss: "lab la-css3-alt"
	},
	MainSkinsAdmin: {
		upload: "las la-upload",
		preview: "las la-search",
		current: "las la-check",
		set: "las la-play",
		info: "las la-info",
		delete: global.delete
	},
	PartialSkinsAdmin: {
		upload: global.upload,
		delete: global.delete
	},
	CustomCssAdmin: {
		save: global.save,
		reset: global.refresh,
		clean: "las la-broom"
	},
	RolesPage: {
		users: "las la-user-friends"
	},
	RoleUsers: {
		user: "las la-user",
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
		key: "las la-key"
	},

	Material: {
		delete: global.delete,
		restore: "las la-trash-restore",
		visits: global.eye,
		publishDate: global.clock
	},
	Comment: {
		delete: global.delete,
		publishDate: global.clock
	}
};
