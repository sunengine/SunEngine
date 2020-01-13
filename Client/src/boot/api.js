import { Api } from "sun";
import { AdminApi } from "sun";

export default ({ Vue }) => {
	Vue.prototype.$Api = Api;
	Vue.prototype.$AdminApi = AdminApi;
};
