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
    successNotify: "Material successfully saved",
  },
  Material: {
    category: "category:",
    edit: "Edit",
    tags: "Tags:",
    deleteDialogTitle: "Delete material?",
    deleteDialogMessage: "",
    deleteDialogOk: "@:Global.dialog.ok",
    deleteDialogCancel: "@:Global.dialog.cancel",
    deleteSuccess: "Material deleted"
  },
  MaterialForm: {
    name: "Name (eng)",
    title: "Header",
    description: "Short description",
    tags: "Tags",
    selectCategory: "Select category",
    category: "Category: {0}",
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
      }
    }
  }
}
