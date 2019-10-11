const glob = require('glob');
const fs = require('fs');


const dirs = ['api', 'admin', 'classes', 'components', 'mixins', 'modules', 'pages', 'router', 'store', 'utils','site'];
const excludePaths = ['src/router/index.js', 'src/site/i18n'];



const pattern = 'src/**/*.@(js|vue)';

var text = "";

const arr = glob.sync(pattern);

proccess(arr);

text += "export * from 'src/store-index'\n";
text += "export * from 'src/router/routerInstance'\n";
text += "export * from 'src/store/storeInstance'\n";
text += "export {app} from 'src/App'\n";


fs.writeFile('./src/sun.js', text, function (err) {
  if (err) {
    return console.log(err);
  }
  console.log('\x1b[32m','\n☼☼☀ Index generated successfully! ☀☼☼\n');
});


function proccess(arr) {
  for (const path of arr) {
    const name = filePathToComponentName(path);
    if (!name)
      continue;

    const fileText = fs.readFileSync(path, 'utf8');

    if(/export( )+(?!default)/.test(fileText))
      text += `export * from '${path}'\n`;
    if(/export( )+default/.test(fileText))
      text += `export ${name} from '${path}'\n`;
  }
}


function filePathToComponentName(name) {

  if (excludePaths.some(x=> name.startsWith(x)))
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
