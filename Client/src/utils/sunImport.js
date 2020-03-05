import sun from "src/index/sunTable";
import admin from "src/index/adminTable";

export default async function sunImport(componentName, moduleName = "sun") {
	const component =
		moduleName === "sun" ? sun[componentName] : admin[componentName];
	if (component.def) {
		const module = await import(component.value);
		return module;
	} else {
		const module = await import(component.value);
		return module.componentName;
	}
}

