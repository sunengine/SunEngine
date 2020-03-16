import sunTable from "sunTable";

export default function(moduleFrom, component) {
	return async function() {
		const mod = await sunTable[moduleFrom]();
		if (!mod) {
			console.log("sunTable", sunTable, "module", sunTable[moduleFrom]);
			console.error(`sunImport("${moduleFrom}", "${component}") Module not found.`);
			console.log(mod);
		}
		if (!mod[component])
			console.error(
				`sunImport("${moduleFrom}", "${component}") Component not found.`
			);
		return mod[component];
	}
}
