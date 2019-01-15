import {date} from 'quasar'

export default ({ Vue }) => {
  Vue.prototype.$formatDate = function formatDate(date1) {
    return date.formatDate(date1, 'DD.MM.YYYY HH:mm');
  }
}
