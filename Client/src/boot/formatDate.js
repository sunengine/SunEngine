import {date as dateutil} from 'quasar'

export default async ({ Vue }) => {
  Vue.prototype.$formatDate = function formatDate(date) {
    return dateutil.formatDate(date, 'DD.MM.YYYY HH:mm');
  }
}
