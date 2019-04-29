import VueI18n from 'vue-i18n'
import messages from 'i18n'

export default async ({app, Vue}) => {
  Vue.use(VueI18n)

  // Set i18n instance on app
  app.i18n = new VueI18n({
    locale: 'ru',
    fallbackLocale: 'en-us',
    messages
  });

  Vue.prototype.$tl = function (key, ...values) {
    if(this.$options.i18nPrefix)
      return this.$t(this.$options.i18nPrefix + "." + this.$options.name + "." + key, values);

    return this.$t(this.$options.name + "." + key, values);
  };

  Vue.prototype.$tle = function (key, ...values) {
    if(this.$options.i18nPrefix)
      return this.$te(this.$options.i18nPrefix + "." + this.$options.name + "." + key, values);

    return this.$te(this.$options.name + "." + key, values);
  };
}
