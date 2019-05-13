export default {
  CreateMaterial: {
    title: "Добавить материал",
    sendBtn: "Отправить",
    successNotify: "Материал успешно добавлен",
  },
  EditMaterial: {
    title: "Редактировать материал",
    saveBtn: "Сохранить",
    successNotify: "Материал успешно сохранён",
  },
  Material: {
    category: "раздел:",
    edit: "Редактировать",
    tags: "Метки:"
  },
  MaterialForm: {
    name: "Имя (eng)",
    title: "Заголовок",
    description: "Короткое описание",
    tags: "Метки",
    selectCategory: "Выберите раздел",
    category: "Раздел: {0}",
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
      description: {
        maxLength: "Максимально допустимая длинна " + config.DbColumnSizes.Materials_Description
      },
      category: {
        required: "Выберите раздел"
      }
    }
  }
}
