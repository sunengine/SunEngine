
export default async ({app, Vue}) => {
  Vue.prototype.$errorNotify = function (error) {

    let errorString = "";
    for (let error of error.errors) {
      let localizeDescription = this.$t("Errors." + error.code);
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
