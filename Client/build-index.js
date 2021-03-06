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
	"admin",
	"api",
	"classes",
	"comp",
	"mixins",
	"shared",
	"modules",
	"layout",
	"router",
	"store",
	"utils",
	"icons"
];
const excludePaths = ["src/site/i18n"];

const patternAll = "src/**/*.@(vue|js)";
const patternSite = "src/site/**/*.@(vue|js)";

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

		for (let i = 0; i < arr.length; i++) {
			text += arr[i].line;
			text += ";\n";
		}

		return text;
	}
}

const indexes = {};

const glob = require("glob");
const fs = require("fs");



function makeIndex() {
    process(glob.sync(patternAll), dirs, excludePaths, indexes, addLine);
    process(glob.sync(patternSite), ["site"], excludePaths, indexes, addLine);

    for (const [name, index] of Object.entries(indexes)) {
        fs.writeFileSync(`./src/index/${name}.js`, index.makeText());
    }

    makeSunImport();

    console.log(
        '\n\x1b[33m☼☼☼   \x1b[32mIndex files generated successfully!\x1b[0m  \x1b[34m"/src/index"   \x1b[33m☼☼☼\x1b[0m\n'
    );
}



function makeSunImport() {
	let imports = `export default {\n`;
	for (const name of Object.keys(indexes)) {
		imports += `"${name}": async function() {
		const mod = await import("src/index/${name}.js");
		return mod;
	},\n`;
	}
	imports += "}";

	fs.writeFileSync(`./src/index/sunTable.js`, imports);
}


function process(arr, dirs, excludePaths, indexes, addLine) {
	for (const path of arr) {
		const [dir, name] = getDirAndComponentName(path, dirs, excludePaths);
		if (!name) continue;

		if (!indexes[dir]) indexes[dir] = new IndexDic();

		const index = indexes[dir];

		const fileText = fs.readFileSync(path, "utf8");

		if (/export( )+default/.test(fileText)) addLine(index, name, path, true);

		const matches = fileText.matchAll(
			/export(?: )+(?:function|const|var|let)(?: )+([a-zA-Z0-9_]+?)[( =]/gi
		);

		for (const match of matches) addLine(index, match[1], path, false);
	}
}

function addLine(index, name, path, isDefault) {
	if (isDefault) {
		index.addLine(`${name}`, `export ${name} from '${path}'`);
	} else {
		index.addLine(`${path}-star`, `export * from '${path}'`);
	}
}

function getDirAndComponentName(name, dirs, excludePaths) {
	if (excludePaths.some(x => name.startsWith(x))) return [];

	let arr = name.replace("\\", "/").split("/");
	if (arr.length <= 2) return [];
	if (!dirs.includes(arr[1])) return [];

	const fileName = arr[arr.length - 1];
	let dirName = arr[arr.length - 2];
	if(dirName === "methods")
		dirName = arr[arr.length - 3];

	arr = fileName.split(".");
	arr.pop();

	return [dirName, arr.join(".")];
}

module.exports = makeIndex;
