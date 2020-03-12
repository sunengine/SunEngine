import { apiPath } from "utils";

export default ({ Vue }) => {
	Vue.prototype.$apiPath = apiPath;
};
