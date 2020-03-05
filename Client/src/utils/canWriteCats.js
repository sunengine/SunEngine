
export default function(categories, exclude) {
	const categoriesList = categories
		.split(",")
		.map(x => store.getters.getCategory(x))
		.filter(x => x);
	const excludeList = exclude
		?.split(",")
		.map(x => store.getters.getCategory(x))
		.filter(x => x);

	const allow = {};

	for (const cat of categoriesList) {
		const cats = cat.getAllSubCanWriteMaterial();
		if (cats) cats.forEach(x => (allow[x.name] = x));
	}

	if (excludeList)
		for (const cat of excludeList) {
			const cats = cat.getAllSubCanWriteMaterial();
			if (cats) cats.forEach(x => delete allow[x.name]);
		}

	const values = Object.values(allow);
	return values && values.length > 0 ? values : null;
}
