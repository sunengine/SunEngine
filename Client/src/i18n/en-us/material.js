export default {
  CreateMaterial: {
    title: "Add material",
    sendBtn: "Post",
    successNotify: "Material successfully added"
  },
  EditMaterial: {
    title: "Edit material",
    saveBtn: "@:Global.btn.save",
    cancelBtn: "@:Global.btn.cancel",
    deleteBtn: "@:Global.btn.delete",
    restoreBtn: "Restore from trash material",
    successNotify: "Material successfully saved",
    deleted: "material deleted",
    deleteDialogTitle: "Delete material?",
    deleteDialogMessage: "",
    deleteDialogOk: "@:Global.dialog.ok",
    deleteDialogCancel: "@:Global.dialog.cancel",
    deleteSuccess: "Material deleted",
    restoreDialogTitle: "Restore material?",
    restoreDialogMessage: "",
    restoreDialogOk: "@:Global.dialog.ok",
    restoreDialogCancel: "@:Global.dialog.cancel",
    restoreSuccess: "Material restored"
  },
  Material: {
    category: "category:",
    edit: "Edit",
    tags: "Tags:",
    visitsCount: "views",
    deleted: "@:EditMaterial.deleted",
    deleteDialogTitle: "@:EditMaterial.deleteDialogTitle",
    deleteDialogMessage: "",
    deleteDialogOk: "@:EditMaterial.deleteDialogOk",
    deleteDialogCancel: "@:EditMaterial.deleteDialogCancel",
    deleteSuccess: "@:EditMaterial.deleteSuccess",
    restoreDialogTitle: "@:EditMaterial.restoreDialogTitle",
    restoreDialogMessage: "",
    restoreDialogOk: "@:EditMaterial.restoreDialogOk",
    restoreDialogCancel: "@:EditMaterial.restoreDialogCancel",
    restoreSuccess: "@:EditMaterial.restoreSuccess"
  },
  MaterialForm: {
    name: "Name (eng)",
    title: "Header",
    description: "Short description",
    tags: "Tags",
    selectCategory: "Category",
    category: "Category: {0}",
    settingsJson: "Json settings",
    validation: {
      name: {
        allowedChars: "Name must be contain only english letters, numbers and character '-'",
        numberNotAllowed: "Material name can`t be number",
        minLength: "Minimal name length - 3",
        maxLength: `Maximal name length - ${config.DbColumnSizes.Materials_Name}`,
      },
      title: {
        required: "Enter header",
        minLength: "Minimal header length - 3",
        maxLength: `Maximal header length - ${config.DbColumnSizes.Materials_Title}`,
      },
      text: {
        required: "Enter text",
        htmlTextSizeOrHasImage: "Minimal text length - 5",
      },
      description: {
        maxLength: "Maximal length " + config.DbColumnSizes.Materials_Description
      },
      category: {
        required: "Selected category"
      },
      settingsJson: {
        jsonFormatError: "Invalid Json format",
      }
    }
  }
}
