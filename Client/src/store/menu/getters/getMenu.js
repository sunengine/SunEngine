export default function getMenu(state) {
	return function(name) {
		if (!state.namedMenuItems || !name) return null;
		return state.namedMenuItems[name.toLowerCase()];
	};
}
