import VueI18n from 'vue-i18n'
import messages from 'src/i18n'

export default async ({app, Vue}) => {
  Vue.use(VueI18n)

  // Set i18n instance on app
  app.i18n = new VueI18n({
    locale: 'ru',
    fallbackLocale: 'en-us',
    messages
  });

  Vue.prototype.$tl = function (key, locale, values) {
    return this.$t(this.$options.name + "." + key, locale, values);
  };

  Vue.prototype.$ta = function (key, locale, values) {
    return this.$t("admin." + this.$options.name + "." + key, locale, values);
  };
}
