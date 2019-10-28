/**
 *  Index maker script
 *  Version: 1.0.2
 *
 *  This script makes index file "/src/sun,js" from all project
 *
 *  How it works?
 *  It parses all directories from cons "dirs", excluding pathes started with "excludePaths"
 *  and make index file from all '.js' and '.vue' components.
 *  Then it goes to '/src/site' directory and overrides entries.
 *  Made by: Dimitrij Polianin
 **/


const glob = require('glob');
const fs = require('fs');


const dirs = ['api', 'admin', 'classes', 'components', 'mixins', 'modules', 'pages', 'router', 'store', 'utils'];
const excludePaths = ['src/router/index.js'];


const patternAll = 'src/**/*.@(js|vue)';
const patternSite = 'src/site/**/*.@(js|vue)';

const ind = indexDic();

proccess(glob.sync(patternAll), dirs, excludePaths);
proccess(glob.sync(patternSite), ['site'], ['src/site/i18n','src/site/routes.js']);


ind.addLine("routes", "export routes from 'src/site/routes.js'");
ind.addLine("store-index", "export * from 'src/store-index'");
ind.addLine("routerInstance", "export * from 'src/router/routerInstance'");
ind.addLine("storeInstance", "export * from 'src/store/storeInstance'");
ind.addLine("App", "export {app} from 'src/App'");


fs.writeFile('./src/sun.js', ind.makeText(), function (err) {
  if (err)
    return console.log(err);

  console.log("\n☼☼☀ \x1b[32m\x1b[1mIndex file generated successfully \x1b[0m => \x1b[33m '/src/sun.js'\x1b[0m ☀☼☼\n");
});


function proccess(arr, dirs, excludePaths) {

  for (const path of arr) {
    const name = filePathToComponentName(path, dirs, excludePaths);
    if (!name)
      continue;

    const fileText = fs.readFileSync(path, 'utf8');

    if (/export( )+default/.test(fileText))
      ind.addLine(`${name}`, `export ${name} from '${path}'`);
    if (/export( )+(?!default)/.test(fileText))
      ind.addLine(`${name}-star`, `export * from '${path}'`);

  }
}


function filePathToComponentName(name, dirs, excludePaths) {
  if (excludePaths.some(x => name.startsWith(x)))
    return;

  let arr = name.replace('\\', '/').split('/');
  if (arr.length <= 1)
    return;
  if (!dirs.includes(arr[1]))
    return;

  const fileName = arr[arr.length - 1];

  arr = fileName.split('.');
  arr.pop();

  const rez = arr.join('.');
  return rez;
}

function indexDic() {
  return {

    dic: {},
    number: 1,

    addLine(compName, line) {
      this.dic[compName] =
        {
          line: line,
          number: this.number++
        };
    },

    makeText() {
      let text = "";
      let arr = [];
      for (const key in this.dic) {
        arr.push(this.dic[key]);
      }

      arr = arr.sort((a, b) => a.number - b.number);

      for (const obj of arr) {
        text += obj.line + "\n";
      }

      return text;
    }
  }
}

