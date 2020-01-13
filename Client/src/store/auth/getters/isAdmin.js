export default function(state) {
	return state.roles.some(x => x === "Admin");
}
