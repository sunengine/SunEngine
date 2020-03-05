
const breadcrumbs = {};

export function makeBreadcrumbs() {
	breadcrumbs.Personal = {
		title: Vue.prototype.i18n.t("BreadcrumbsNames.personal"),
		route: {
			name: "ProfileInSettings"
		},
		showInBreadcrumbs: true,
		id: "ProfileInSettings"
	};

	breadcrumbs.Users = {
		title: Vue.prototype.i18n.t("BreadcrumbsNames.user"),
		route: null,
		showInBreadcrumbs: true,
		id: "Users"
	};

	/*	breadcrumbs.PersonalUser = {
		title: app.$t("BreadcrumbsNames.you"),
		route: {},
		showInBreadcrumbs: true,
		id: "PersonalUser",
		parent: breadcrumbs.Personal
	};*/

	breadcrumbs.Admin = {
		title: Vue.prototype.i18n.t("BreadcrumbsNames.adminPanel"),
		route: {
			name: "AdminInformation"
		},
		showInBreadcrumbs: true,
		id: "AdminInformation"
	};

	breadcrumbs.CategoriesAdmin = {
		title: Vue.prototype.i18n.t("BreadcrumbsNames.adminPanelCategories"),
		route: {
			name: "CategoriesAdmin"
		},
		showInBreadcrumbs: true,
		id: "CategoriesAdmin",
		parent: breadcrumbs.Admin
	};

	breadcrumbs.SectionsAdmin = {
		title: Vue.prototype.i18n.t("BreadcrumbsNames.adminPanelSections"),
		route: {
			name: "SectionsAdmin"
		},
		showInBreadcrumbs: true,
		id: "SectionsAdmin",
		parent: breadcrumbs.Admin
	};

	breadcrumbs.MenuItemsAdmin = {
		title: Vue.prototype.i18n.t("BreadcrumbsNames.adminPanelMenu"),
		route: {
			name: "MenuItemsAdmin"
		},
		showInBreadcrumbs: true,
		id: "MenuItemsAdmin",
		parent: breadcrumbs.Admin
	};

	Object.freeze(breadcrumbs);
}

export default function getBreadcrumbs(name) {
	return breadcrumbs[name];
}
