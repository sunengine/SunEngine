import { store } from "sun";

export default function(route) {
	return (
		!route.meta?.roles ||
		store.state.auth.roles.some(x => route.meta.roles.some(y => x === y))
	);
}
