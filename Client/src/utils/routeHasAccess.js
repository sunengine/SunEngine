import { store } from "src/store/index";

export default function(route) {
	return (
		!route.meta?.roles ||
		store.state.auth.roles.some(x => route.meta.roles.some(y => x === y))
	);
}
