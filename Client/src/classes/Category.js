import { extend } from "quasar";

export default class Category {
	get children() {
		return this.subCategories;
	}

	get selectable() {
		return this.categoryPersonalAccess?.MaterialWrite && this.isMaterialsContainer;
	}
	
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

	getAllSubCanWriteMaterial() {
		const rez = [];

		if (this.categoryPersonalAccess.MaterialWrite) rez.push(this);

		if (this.subCategories)
			for (const sub of this.subCategories) {
				const subs = sub.getAllSubCanWriteMaterial();
				if (subs) subs.forEach(x => rez.push(x));
			}

		return rez.length > 0 ? rez : null;
	}
}
