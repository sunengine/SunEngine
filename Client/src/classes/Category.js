import { extend } from "quasar";

export default class Category {
	getRoute() {
		return this.route ?? undefined;
	}

	getMaterialRoute(idOrName, hash) {
		const route = this.getRoute();
		if (!route || !route.name) return undefined;

		let rezRoute = extend(true, {}, route);

		rezRoute.name += "-mat";
		if (!rezRoute.params) rezRoute.params = {};

		rezRoute.params.idOrName = idOrName.toString();

		if (hash) rezRoute.hash = hash;

		return rezRoute;
	}
}
