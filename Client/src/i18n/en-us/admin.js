export default {
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
  AdminMenu: {
    categoriesAdmin: "Categories",
    rolesPermissions: "Permission settings",
    rolesUsers: "Groups",
    cacheSettings: "Cache settings"
  },
  AdminPage: {
    title: "@:Admin.AdminPanel.title"
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
  CategoriesAdmin: {
    title: "Admin page",
    addCategoryBtn: "Add category"
  },
  CategoryItem: {
    rootCategory: "Root category"
  },
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
    textAreaLabel: "Json файл конфигурации прав групп"
  },
  RoleUsers: {
    users: "Users",
    filter: "Find by name",
    noResults: "Not found",
    filterLimitReached: "First {0} results are derived"
  }
}
