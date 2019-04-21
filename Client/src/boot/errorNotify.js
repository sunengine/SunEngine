
export default async ({app, Vue}) => {
  Vue.prototype.$errorNotify = function (error) {

    const errors = error.response.data.errors;
    let errorString = "";
    for (let error of errors) {
      const localizeDescription = this.$t("Errors." + error.code);
      console.error(`Error code: ${error.code}, description: ${error.description}\nLocalize description: ${localizeDescription}`);
      errorString += localizeDescription + "\n";
    }

    this.$q.notify({
      message: errorString,
      timeout: 2000,
      color: 'negative',
      position: 'top'
    });

  }
}
