export default function(state) {
	return function(name) {
		if (!name) return null;
		return state.componentsTypes[name.toLowerCase()];
	};
}
