/****************************************************************************************
 *                                                                                      *
 *    Index maker script                                                                *
 *    Version: 1.0.3                                                                    *
 *                                                                                      *
 *    This script makes index file "/src/sun.js"                                        *
 *                                                                                      *
 *    1. Parse all directories from const "dirs",                                       *
 *    excluding paths started with const "excludePaths".                                *
 *    2. Make index from all '.js' and '.vue' components.                               *
 *    3. Overrides same entries from '/src/site' directory making its prefer default.   *
 *                                                                                      *
 ****************************************************************************************/

const dirs = [
	"api",
	"classes",
	"components",
	"mixins",
	"shared",
	"modules",
	"layouts",
	"router",
	"store",
	"utils",
	"icons"
];
const adminDirs = ["admin"];
const excludePaths = ["src/router/index.js"];

const patternAll = "src/**/*.@(js|vue)";
const patternAdmin = "src/admin/**/*.@(js|vue)";
const patternSite = "src/site/**/*.@(js|vue)";

class IndexDic {
	constructor() {
		this.dic = {};
		this.number = 1;
	}

	addLine(compName, line) {
		this.dic[compName] = {
			line: line,
			number: this.number++
		};
	}

	makeText() {
		let text = "";
		let arr = [];
		for (const key in this.dic) arr.push(this.dic[key]);

		arr = arr.sort((a, b) => a.number - b.number);

		for (const obj of arr) text += obj.line + "\n";

		return text;
	}
}

const indSun = new IndexDic();
const indAdmin = new IndexDic();

const glob = require("glob");
const fs = require("fs");

proccess(glob.sync(patternAll), dirs, excludePaths, indSun);
proccess(
	glob.sync(patternSite),
	["site"],
	["src/site/i18n", "src/site/routes.js"],
	indSun
);
proccess(glob.sync(patternAdmin), adminDirs, excludePaths, indAdmin);

indSun.addLine("Api", "export Api from 'src/Api.js'");
indSun.addLine("routes", "export routes from 'src/site/routes.js'");
indSun.addLine("store-index", "export * from 'src/store/index'");
indSun.addLine("router", "export {router} from 'src/router/index.js'");
indSun.addLine("App", "export {app} from 'src/App'");

fs.writeFileSync("./src/sun.js", indSun.makeText());

fs.writeFileSync("./src/admin.js", indAdmin.makeText());

console.log(
	'\n\x1b[33m☼☼☼   \x1b[32mIndex file generated successfully!\x1b[0m  \x1b[34m"/src/sun.js"   \x1b[33m☼☼☼\x1b[0m\n' +
	'\x1b[33m☼☼☼   \x1b[32mIndex file generated successfully!\x1b[0m  \x1b[34m"/src/admin.js"   \x1b[33m☼☼☼\x1b[0m\n'
);

function proccess(arr, dirs, excludePaths, index) {
	for (const path of arr) {
		const name = filePathToComponentName(path, dirs, excludePaths);
		if (!name) continue;

		const fileText = fs.readFileSync(path, "utf8");

		if (/export( )+default/.test(fileVText))
			index.addLine(`${name}`, `export ${name} from '${path}'`);
		if (/export( )+(?!default)/.test(fileText))
			index.addLine(`${name}-star`, `export * from '${path}'`);
	}
}

function filePathToComponentName(name, dirs, excludePaths) {
	if (excludePaths.some(x => name.startsWith(x))) return;

	let arr = name.replace("\\", "/").split("/");
	if (arr.length <= 1) return;
	if (!dirs.includes(arr[1])) return;

	const fileName = arr[arr.length - 1];

	arr = fileName.split(".");
	arr.pop();

	return arr.join(".");
}
