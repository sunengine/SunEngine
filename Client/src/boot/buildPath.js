import { buildPath } from "sun";

export default ({ Vue }) => {
	Vue.prototype.$buildPath = buildPath;
};
