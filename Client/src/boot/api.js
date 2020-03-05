import {Api} from "sun";
import {AdminApi} from "sun";

export default async ({ Vue }) => {
	Vue.prototype.$Api = Api;
	Vue.prototype.$AdminApi = AdminApi;
};
