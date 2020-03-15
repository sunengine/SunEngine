import sunTable from "sunTable";

console.log(sunTable);

export default async function sunImport(moduleFrom, component) {
	const mod = await sunTable[moduleFrom]();
	if (!mod)
		console.error(`sunImport("${moduleFrom}", "${component}") Module not found.`);
	if(!mod[component])
		console.error(`sunImport("${moduleFrom}", "${component}") Component not found.`);
	return mod[component];
}
