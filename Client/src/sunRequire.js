import sun from "./sun";
import admin from "./admin";

export default function sunRequire(componentName, moduleName = "sun") {
    console.log("componentName",componentName);
    const component =
        moduleName === "sun" ? sun[componentName] : admin[componentName];
    if (component.def) {
        const module = require(component.value);
        return module;
    } else {
        const module = require("./"+component.value);
        return module.componentName;
    }
}
