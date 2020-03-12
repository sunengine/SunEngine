import { buildPath } from "utils";

export default ({ Vue }) => {
	Vue.prototype.$buildPath = buildPath;
};
