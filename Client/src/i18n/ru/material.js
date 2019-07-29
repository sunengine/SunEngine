export default {
  CreateMaterial: {
    title: "Добавить материал",
    sendBtn: "Отправить",
    successNotify: "Материал успешно добавлен",
  },
  EditMaterial: {
    title: "Редактировать материал",
    saveBtn: "@:Global.btn.save",
    cancelBtn: "@:Global.btn.cancel",
    deleteBtn: "@:Global.btn.delete",
    restoreBtn: "Восстановить из корзины материал",
    successNotify: "Материал успешно сохранён",
    deleted: "материал удалён",
    deleteDialogTitle: "Удалить материал?",
    deleteDialogMessage: "",
    deleteDialogOk: "@:Global.dialog.ok",
    deleteDialogCancel: "@:Global.dialog.cancel",
    deleteSuccess: "Материал успешно удалён",
    restoreDialogTitle: "Восстановить материал?",
    restoreDialogMessage: "",
    restoreDialogOk: "@:Global.dialog.ok",
    restoreDialogCancel: "@:Global.dialog.cancel",
    restoreSuccess: "Материал успешно восстановлен"
  },
  Material: {
    category: "раздел:",
    edit: "Редактировать",
    tags: "Метки:",
    visitsCount: "просмотры",
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
    name: "Имя (eng)",
    title: "Заголовок",
    description: "Короткое описание",
    tags: "Метки",
    selectCategory: "Раздел",
    category: "Раздел: {0}",
    hide: "Спрятать",
    blockComments: "Запретить комментарии",
    settingsJson: "Json настройки",
    validation: {
      name: {
        allowedChars: "Имя должно содержать только английские буквы цифры и символ '-'",
        numberNotAllowed: "Имя материала не может быть числом",
        minLength: "Минимальная длинна имени - 3",
        maxLength: `Максимальная длинна имени - ${config.DbColumnSizes.Materials_Name}`,
      },
      title: {
        required: "Введите заголовок",
        minLength: "Минимальная длинна заголовка - 3",
        maxLength: `Максимальная длинна заголовка - ${config.DbColumnSizes.Materials_Title}`,
      },
      text: {
        required: "Введите текст",
        htmlTextSizeOrHasImage: "Минимальная длинна текста - 5",
      },
      subTitle: {
        maxLength: "Максимально допустимая длинна " + config.DbColumnSizes.Materials_Description
      },
      category: {
        required: "Выберите раздел"
      },
      settingsJson: {
        jsonFormatError: "Неверный формат Json",
      }
    }
  }
}
