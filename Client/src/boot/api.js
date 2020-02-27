import { Api } from "sun";
import { AdminApi } from "admin";

export default ({ Vue }) => {
	Vue.prototype.$Api = Api;
	Vue.prototype.$AdminApi = AdminApi;
};
