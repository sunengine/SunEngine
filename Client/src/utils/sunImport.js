import sun from "./sun";
import admin from "./admin";

export async function sunImport(componentName, moduleName = "sun") {
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

