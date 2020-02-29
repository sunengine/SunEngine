import sun from "sun"

export async function sunImport(componentName,moduleName = "sun") {
    const component = sun[componentName];
    if(component.def) {
        const module = await import(component.value);
        return module;
    } else {
        const module = await import(component.value);
        return module.componentName;
    }
}

export function sunRequire(componentName,moduleName = "sun") {
    const component = sun[componentName];
    if(component.def) {
        const module = require(component.value);
        return module;
    } else {
        const module = require(component.value);
        return module.componentName;
    }
}