export default function(state) {
	return function(name) {
		if (!state.all || !name) return null;
		return state.all[name.toLowerCase()];
	};
}
