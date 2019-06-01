export default {

  // ——— categories ————————————————————————————————————

  CategoriesAdmin: {
    title: "Admin page",
    addCategoryBtn: "Add category"
  },
  CategoryForm: {
    name: "Category name (eng)",
    title: "Title",
    shortDescription: "Short description",
    header: "Category header",
    selectParent: "Select parent category",
    sectionType: "Category type",
    deleteConfirm: "You want to remove the category?",
    parent: "Parent: ",
    hideCb: "Hide",
    appendUrlTokenCb: "Add to URL",
    appendUrlTokenInfo: "(use only if you understand what it is)",
    isMaterialsContainerCb: "Contains materials",
    isCaching: "Caching",
    cachingPageCount: "Cache N pages",
    noTypeLabel: "Without type",
    layout: "Layout",
    validation: {
      name: {
        required: "Enter category name (eng)",
        minLength: "Name (eng) must be at least 2 letters",
        allowedChars: "The name (eng) must consist of the characters `a-z`, `A-Z`, `0-9`, `-`"
      },
      title: {
        required: "Enter category title",
        minLength: "Category title must contain at least 3 letters"
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
    successNotify: "Category was added.\nDon`t forget to reload the site in your browser."
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
    successNotify: "Category was updated.\nDon`t forget to reload the site in your browser."
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
    successNotify: "Menu item successfully edited"
  },
  MenuAdminItem: {},
  MenuItemForm: {
    name: "Identifier (eng)",
    title: "Title",
    subTitle: "Sub title",
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
        maxLength: "Maximum title length - " + config.DbColumnSizes.Categories_Title,
      },
      subTitle: {
        minLength: "Minimal sub title length - 3",
        maxLength: "Maximum sub title length - " + config.DbColumnSizes.MenuItems_SubTitle,
      },
      cssClass: {
        minLength: "Minimal css class length - 3",
        maxLength: "Maximum css class length - " + config.DbColumnSizes.MenuItems_CssClass,
      },
      icon: {
        minLength: "Minimal icon length - 3",
        maxLength: "Maximum icon length - " + config.DbColumnSizes.MenuItems_Icon,
      },
      settingsJson: {
        jsonFormatError: "Invalid Json format",
      }
    }
  },
  MenuItemsAdmin: {
    title: "Edit menu",
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
    addRoleConfirm: "Add to group '{0}'?",
    addRoleConfirmOkBtn: "Yes",
    removeRoleConfirm: "Remove from group '{0}'?",
    removeRoleConfirmOkBtn: "Remove",
  },
  RolesPage: {
    title: "Groups page",
    roles: "Groups",
  },
  RolesPermissions: {
    title: "Upload group config(json)",
    backupWarning: "Before uploading, you need to make a database backup.",
    saveToServerBtn: "Save",
    getFromServer: "Load from server",
    getSuccessNotify: "Download completed successfully",
    saveSuccessNotify: "Group settings were updated successfully",
    textAreaLabel: "Json file of roles config"
  },
  RoleUsers: {
    users: "Users",
    filter: "Find by name",
    noResults: "Not found",
    filterLimitReached: "First {0} results are derived"
  },

  // ——— all ————————————————————————————————————

  AdminMenu: {
    menuItemsAdmin: "Menu",
    categoriesAdmin: "Categories",
    rolesPermissions: "Permission settings",
    rolesUsers: "Groups",
    cacheSettings: "Cache settings",
    deletedElements: "Deleted elements",
    imagesCleaner: "Images cleaner"
  },
  AdminPage: {
    title: "@:AdminPanel.title"
  },
  AdminPanel: {
    title: "Admin panel"
  },
  CacheSettings: {
    title: "Cache settings",
    cachePolicy: "Cache policy",
    alwaysPolicy: "Always cache",
    neverPolicy: "Never cache",
    customPolicy: "Custom cache",
    cacheLifetime: "Cache record lifetime",
    saveChangesBtn: "Save changes",
    withoutInvalidationTime: "No time limit",
    successNotify: "Cache policy changed",
    error: "Server error",
    validation: {
      invalidateCacheTime: {
        required: "Require input",
        invalidValue: "Value can`t be lower 0",
      }
    }
  },
  DeletedElements: {
    title: "Deleted elements",
    showDeleted: "Show deleted elements",
    info1: "If checked, deleted materials will shows on any single category.",
    info2: "If append '?deleted=1' to any single category URL deleted material will shows.",
    info3: "This function will not work on multi categories sections like 'new topics' on forum."
  },
  ImagesCleaner: {
    title: "ImagesCleaner",
    clearBtn: "Clear",
    refreshBtn: "Refresh",
    clearCount: "Cleared images: ",
    emptyResult: "Folder is empty"
  }
}
