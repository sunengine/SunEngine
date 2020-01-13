import { request } from "sun";

export default ({ Vue }) => {
	Vue.prototype.$request = request;
};
