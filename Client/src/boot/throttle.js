import { throttle } from "utils";

export default ({ Vue }) => {
	Vue.prototype.$throttle = throttle;
};
