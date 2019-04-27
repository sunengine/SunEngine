
export default async ({app, Vue}) => {
  Vue.prototype.$errorNotify = function (error) {

    const errors = error?.response?.data?.errors;
    if(!errors)
      return;

    for (const error of errors) {
      console.log(app.i18n);
      const localizeDescription = app.i18n.t("Errors." + error.code);

      let errorText = `Error code: ${error.code}\n`;
      errorText += `Description: ${error.description}\nLocalize description: ${localizeDescription}\n`;

      if(error.message)
        errorText += `Message: ${error.message}\n`;
      if(error.stackTrace)
        errorText += `StackTrace: ${error.stackTrace}`;

      console.error(errorText);

      const color = error.type.toLowerCase() === "soft" ? "warning" : "negative";

      this.$q.notify({
        message: localizeDescription,
        timeout: 2800,
        color: color,
        position: 'top'
      });
    }



  }
}
