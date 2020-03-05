import { apiPath } from "sun";

export default ({ Vue }) => {
	Vue.prototype.$apiPath = apiPath;
};
