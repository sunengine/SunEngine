import { getBreadcrumbs } from "sun";

export default ({ Vue }) => {
    Vue.prototype.$getBreadcrumbs = getBreadcrumbs;
};
