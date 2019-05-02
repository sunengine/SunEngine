
export default {

  AdminPanel: {
    title: "Admin panel"
  },
  AdminPage: {
    title: "@:Admin.AdminPanel.title"
  },
  AdminMenu: {
    categoriesAdmin: "Categories",
    rolesPermissions: "Permission settings",
    rolesUsers: "Groups",
    cacheSettings: "Cache settings"
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
  CreateCategory: {
    title: "Create category",
    createBtn: "Create",
    cancelBtn: "@:Global.btn.cancel",
    successNotify: "Category was added.\nDon`t forget to reload the site in your browser."
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
  CategoriesAdmin: {
    title: "Admin page",
    addCategoryBtn: "Add category"
  },
  CategoryItem: {
    rootCategory: "Root category"
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
  RoleUsers: {
    users: "Users",
    filter: "Find by name",
    noResults: "Not found",
    filterLimitReached: "First {0} results are derived"
  },
  CacheSettings: {
    title: "Cache settings",
    CachePolicy: "Cache policy",
    AlwaysPolicy: "Always cache",
    NeverPolicy: "Never cache",
    CustomPolicy: "Custom cache",
    CacheLifetime: "Cache record lifetime",
    SaveChanges: "Save changes",
    WithoutInvalidationTime: "No time limit",
    successNotify: "Cache policy changed",
    error: "Server error",
    validation: {
      invalidateCacheTime: {
        required: "Require input",
        invalidValue: "Value can`t be lower 0",
      }
    }
  }
};
