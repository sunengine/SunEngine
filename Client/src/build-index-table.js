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
const excludePaths = [];

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

		for (const obj of arr) text += obj.line + ",\n";

		text = "export default {\n" + text + "};\n";

		return text;
	}
}

const indSun = new IndexDic();
const indAdmin = new IndexDic();

const glob = require("glob");
const fs = require("fs");

proccess(glob.sync(patternAll), dirs, excludePaths, indSun);
proccess(glob.sync(patternSite), ["site"], excludePaths, indSun);
proccess(glob.sync(patternAdmin), adminDirs, excludePaths, indAdmin);

indSun.addLine("app", '"app": {"def": false, "value": "src/App.vue"}');

fs.writeFileSync("./src/index/sunTable.js", indSun.makeText());
fs.writeFileSync("./src/index/adminTable.js", indAdmin.makeText());

console.log(
	'\n\x1b[33m☼☼☼   \x1b[32mIndex file generated successfully!\x1b[0m  \x1b[34m"/src/index/sunTable.js"   \x1b[33m☼☼☼\x1b[0m\n' +
	'\x1b[33m☼☼☼   \x1b[32mIndex file generated successfully!\x1b[0m  \x1b[34m"/src/index/adminTable.js"   \x1b[33m☼☼☼\x1b[0m\n'
);

function proccess(arr, dirs, excludePaths, index) {
	for (const path of arr) {
		const name = filePathToComponentName(path, dirs, excludePaths);
		if (!name) continue;

		const fileText = fs.readFileSync(path, "utf8");

		if (/export( )+default/.test(fileText)) {
			addLine(index, name, path, true);
		}
		const matches = fileText.matchAll(
			/export(?: )+(?:function|const|var|let)(?: )+([a-zA-Z0-9_]+?)[( =]/gi
		);

		for (const match of matches) {
			addLine(index, match[1], path, false);
		}
	}

	function addLine(index, name, path, isDefault) {
		index.addLine(
			`${name}`,
			`"${name}": {"def": ${isDefault}, "value": "${path}"}`
		);
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
