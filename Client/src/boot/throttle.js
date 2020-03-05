import { throttle } from "sun";

export default ({ Vue }) => {
	Vue.prototype.$throttle = throttle;
};
