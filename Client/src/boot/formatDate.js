import { date as dateutil } from "quasar";

export default ({ Vue }) => {
	Vue.prototype.$formatDate = function(date) {
		return dateutil.formatDate(date, "DD.MM.YYYY HH:mm");
	};

	Vue.prototype.$formatDateOnly = function(date) {
		return dateutil.formatDate(date, "DD.MM.YYYY");
	};

	Vue.prototype.$formatToSemTime = function(date) {
		return dateutil.formatDate(date, "YYYY-MM-DDTHH:MM");
	};
};
