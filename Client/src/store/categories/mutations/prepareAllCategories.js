import { Category } from "sun";
import { consoleInit } from "sun";

export default function prepareAllCategories(state, root) {
	state.root = root;
	state.all = {};

	buildStructureRecursive(root);

	detectCanSomeChildrenWriteMaterial(root);

	injectPrototype();

	console.info(
		"%cCategories prepared",
		consoleInit,
		config.Dev.LogInitExtended ? state.all : ""
	);

	function buildStructureRecursive(category, sectionRoot = null) {
		if (!category) return;

		// Add to all
		state.all[category.name.toLowerCase()] = category;

		// Make section types
		if (category.layoutName) sectionRoot = category;

		if (sectionRoot) category.sectionRoot = sectionRoot;

		if (!category.subCategories) return;

		for (const subCategory of category.subCategories) {
			// Make parents
			subCategory.parent = category;

			buildStructureRecursive(subCategory, sectionRoot);
		}
	}

	function detectCanSomeChildrenWriteMaterial(category) {
		if (!category) return;

		let has = false;
		if (category.subCategories)
			for (const cat of category.subCategories)
				if (detectCanSomeChildrenWriteMaterial(cat)) has = true;

		if (category.categoryPersonalAccess?.MaterialWrite) has = true;

		category.canSomeChildrenWriteMaterial = has;

		return has;
	}

	function injectPrototype() {
		for (const category of Object.values(state.all))
			Object.setPrototypeOf(category, Category.prototype);
	}
}
