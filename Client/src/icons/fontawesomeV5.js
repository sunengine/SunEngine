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
	send: "fas fa-arrow-circle-right",
	comment: "far fa-comment",
	category: "fas fa-folder"
};

export default {
	name: "fontawesome-v5",
	sunName: "FontAwesome",
	AdminMenu: {
		AdminInformation: "fas fa-info-circle",
		ConfigurationAdmin: "fas fa-sliders-h",
		MenuItemsAdmin: "fas fa-bars",
		CategoriesAdmin: "fas fa-folder",
		ComponentsAdmin: "fas fa-shapes",
		SkinsAdmin: "fas fa-palette",
		RolesPage: "fas fa-user-friends",
		RolesPermissions: "fas fa-user-shield",
		DeletedElements: "fas fa-trash",
		resetCache: global.refresh,
		systemTools: "fas fa-tools",
		ImagesCleaner: "fas fa-broom",
		CypherSecrets: "fas fa-key"
	},
	SunEditor: {
		addImages: "fas fa-image"
	},
	CategoriesAdmin: {
		addCategoryBtn: global.add
	},
	CategoryItem: {
		up: "fas fa-chevron-up",
		down: "fas fa-chevron-down",
		eyeSlash: global.eyeSlash,
		edit: "fas fa-wrench",
		plus: global.add,
		goTo: global.goTo,
		material: "far fa-file-alt"
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
		icons: "fas fa-icons",
		category: global.category,
		layout: "fas fa-table"
	},
	ConfigurationAdmin: {
		search: global.search,
		question: "far fa-question-circle",
		save: global.save,
		reset: global.refresh,
		cancel: global.cancel
	},
	CreateMenuItem: {
		create: global.add,
		cancel: global.cancel
	},
	EditMenuItem: {
		save: global.save,
		cancel: global.cancel,
		delete: global.delete
	},
	MenuAdminItem: {
		up: "fa fa-angle-up", //"la la-arrow-up", // "la la-caret-up", "la la-angle-up",
		down: "fa fa-angle-down", //"la la-arrow-up" // "la la-caret-down", "la la-angle-down"
		eye: global.eye,
		eyeSlash: global.eyeSlash,
		blank: "fas fa-file",
		edit: "fas fa-wrench",
		add: global.add,
		goTo: global.goTo,
		delete: global.delete
	},
	MenuItemsAdmin: {
		add: global.add
	},
	MenuItemForm: {
		link: "fas fa-link",
		css: "fab fa-css3-alt",
		icons: "fas fa-icons",
		search: global.search
	},
	ComponentsAdmin: {
		add: global.add,
		component: "fas fa-cube",
		edit: "fas fa-wrench",
		goTo: global.goTo
	},
	ComponentForm: {
		component: "fas fa-cube"
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
		main: "fas fa-user-astronaut",
		partial: "fas fa-puzzle-piece",
		customCss: "fab fa-css3-alt"
	},
	MainSkinsAdmin: {
		upload: global.upload,
		preview: global.search,
		current: "fas fa-check",
		set: "fas fa-play",
		info: "fas fa-info",
		delete: global.delete
	},
	PartialSkinsAdmin: {
		upload: global.upload,
		delete: global.delete
	},
	CustomCssAdmin: {
		save: global.save,
		reset: global.refresh,
		clean: "fas fa-snowplow"
	},
	RolesPage: {
		users: "fas fa-user-friends"
	},
	RoleUsers: {
		user: "fas fa-user",
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
		key: "fas fa-key"
	},

	Material: {
		delete: global.delete,
		restore: "fas fa-trash-restore",
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
		comments: global.comment,
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
	Activity: {
		comment: global.comment,
		material: "far fa-file-alt",
		publishDate: global.clock,
		category: global.category
	},
	Post: {
		deleted: global.delete,
		hidden: global.eyeSlash,
		readMore: "far fa-file-alt",
		comment: global.comment
	},
	BlogMultiCatPage: {
		add: global.add
	},
	BlogPage: {
		add: global.add
	},
	Topic: {
		deleted: global.delete,
		hidden: global.eyeSlash,
		comment: global.comment,
		category: global.category,
		publishDate: global.clock
	},
	Thread: {
		add: global.add
	}
};
