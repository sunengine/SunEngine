export default async ({app, Vue}) => {
  Vue.prototype.$successNotify = function (msg) {

    if (!msg) {
      if (this.$tle("successNotify"))
        msg = this.$tl("successNotify");
      else
        msg = this.$t("Global.successNotify");
    }

    this.$q.notify({
      message: msg,
      timeout: 2800,
      color: "positive",
      position: 'top'
    });
  }
}
