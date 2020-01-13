import { store } from "sun";

export default function clearAllUserRelatedData(state) {
	state.user = null;
	state.roles = ["Unregistered"];
	store.state.categories.root = null;
	store.state.categories.all = null;
	store.state.menu.namedMenuItems = null;
	store.state.components.allComponents = null;
	store.state.admin.showDeletedElements = false;
}
