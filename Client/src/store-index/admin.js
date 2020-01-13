import adminState from "src/store/admin/adminState";
import * as breadcrumbs  from "src/store/admin/getters/breadcrumbs";

export default {
	state: adminState,
	getters: breadcrumbs
};
