import Vue from "vue";

export function getWhereToMove() {
	const rez = goDeep(store.state.categories.root);
	return [...rez.children];
}

export function getWhereToAdd(categoriesNames) {
	if (categoriesNames.includes(","))
		return getWhereToAddMultiCat(categoriesNames);
	else return getWhereToAddOneCat(categoriesNames);
}

export function getWhereToAddOneCat(categoryName) {
	let rez = goDeep(store.getters.getCategory(categoryName));
	if (rez.selectable) rez = [rez];
	else rez = [...rez.children];

	return rez;
}

export function getWhereToAddMultiCat(categoriesNames) {
	const categories = categoriesNames.split(",").map(x => x.trim());
	const nodes = [];
	for (let categoryName of categories) {
		const node = goDeep(store.getters.getCategory(categoryName));
		if (node) nodes.push(node);
	}

	return nodes;
}

function goDeep(category) {
	if (!category) return null;

	let children;
	if (category.subCategories) {
		children = [];
		for (let child of category.subCategories) {
			let one = goDeep(child);
			if (one) children.push(one);
		}
	}

	if (children && children.length === 0) children = null;

	const ret = {
		title: category.title,
		name: category.name,
		category: category,
		children: children,
		selectable: false
	};

	if (category.categoryPersonalAccess?.MaterialWrite) {
		if (category.isMaterialsContainer) {
			// writable
			ret.icon = Vue.prototype.$iconsSet.global.category;
			ret.iconColor = "green-5";
			ret.selectable = true;
		} else if (children) {
			// disabled mode on FolderCategory
			ret.selectable = false;
		}
	} else if (!children) {
		return null;
	}
	return ret;
}
