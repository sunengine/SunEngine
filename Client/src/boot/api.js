export default async ({ Vue }) => {
	Vue.prototype.$Api = require("sun").Api;;
};
