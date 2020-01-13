import de from "quasar/lang/de";

export function breadcrumbsAdmin() {
    return {
        title: "Админка",
        route: {
            name: "AdminInformation"
        },
        showInBreadcrumbs: true,
        id: "AdminInformation"
    }
}

export function breadcrumbsCategoriesAdmin() {
    return {
        title: "Категории",
        route: {
            name: "CategoriesAdmin"
        },
        showInBreadcrumbs: true,
        id: "CategoriesAdmin",
        parent: breadcrumbsAdmin()
    }
}

export function breadcrumbsComponentsAdmin() {
    return {
        title: "Компоненты",
        route: {
            name: "ComponentsAdmin"
        },
        showInBreadcrumbs: true,
        id: "ComponentsAdmin",
        parent: breadcrumbsAdmin()
    }
}

export function breadcrumbsMenuAdmin() {
    return {
        title: "Меню",
        route: {
            name: "MenuItemsAdmin"
        },
        showInBreadcrumbs: true,
        id: "MenuItemsAdmin",
        parent: breadcrumbsAdmin()
    }
}
