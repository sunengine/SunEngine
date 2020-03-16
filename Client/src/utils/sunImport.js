import sunTable from "sunTable";

export default function(moduleFrom, component) {
	return async function() {
		const mod = await sunTable[moduleFrom]();
		if (!mod) {
			console.error(`sunImport("${moduleFrom}", "${component}") Module not found.`);
		}
		if (!mod[component])
			console.error(
				`sunImport("${moduleFrom}", "${component}") Component not found.`
			);
		return mod[component];
	}
}
