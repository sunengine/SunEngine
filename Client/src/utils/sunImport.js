import {sun} from "sun"

export default async function(componentName) {
    const path = sun[componentName];
    const module = import(path);
    
}