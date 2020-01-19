import { app } from "sun";

const breadcrumbs = {};

export function makeBreadcrumbs() {
	breadcrumbs.Personal = {
		title: app.$t("BreadcrumbsNames.personal"),
		route: {
			name: "ProfileInSettings"
		},
		showInBreadcrumbs: true,
		id: "ProfileInSettings"
	};

	breadcrumbs.Users = {
		title: app.$t("BreadcrumbsNames.user"),
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
		title: app.$t("BreadcrumbsNames.adminPanel"),
		route: {
			name: "AdminInformation"
		},
		showInBreadcrumbs: true,
		id: "AdminInformation"
	};

	breadcrumbs.CategoriesAdmin = {
		title: app.$t("BreadcrumbsNames.adminPanelCategories"),
		route: {
			name: "CategoriesAdmin"
		},
		showInBreadcrumbs: true,
		id: "CategoriesAdmin",
		parent: breadcrumbs.Admin
	};

	breadcrumbs.ComponentsAdmin = {
		title: app.$t("BreadcrumbsNames.adminPanelComponents"),
		route: {
			name: "ComponentsAdmin"
		},
		showInBreadcrumbs: true,
		id: "ComponentsAdmin",
		parent: breadcrumbs.Admin
	};

	breadcrumbs.MenuItemsAdmin = {
		title: app.$t("BreadcrumbsNames.adminPanelMenu"),
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
