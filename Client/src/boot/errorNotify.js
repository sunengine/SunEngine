
export default async ({app, Vue}) => {
  Vue.prototype.$errorNotify = function (error) {

    const errors = error.response.data.errors;

    for (const error of errors) {
      const localizeDescription = this.$t("Errors." + error.code);
      console.error(`Error code: ${error.code}, description: ${error.description}\nLocalize description: ${localizeDescription}`);
      const color = error.isSoft ? "warning" : "negative";

      this.$q.notify({
        message: localizeDescription,
        timeout: 2800,
        color: color,
        position: 'top'
      });
    }



  }
}
