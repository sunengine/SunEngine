import { getBreadcrumbs } from "utils";

export default ({ Vue }) => {
	Vue.prototype.$getBreadcrumbs = getBreadcrumbs;
};
