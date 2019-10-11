const glob = require('glob');
const fs = require('fs');


const excludeDirs = ["i18n", "index", "site-template", "boot"];
//const excludePaths = ["i18n", "index", "site-template", "boot"];


const pattern = 'src/**/*.@(js|vue)';

var text = "";

const arr = glob.sync(pattern);

proccess(arr);

fs.writeFile("./src/sun.js", text, function (err) {
  if (err) {
    return console.log(err);
  }
  console.log("Index generated successfully!");
});



function proccess(arr) {
  for (const path of arr) {
    let name = filePathToComponentName(path);
    if (!name)
      continue;

    text += `export ${name} from '${path}'\n`;
  }
}


function filePathToComponentName(name) {

  let arr = name.replace('\\', '/').split('/');
  if (arr.length <= 1)
    return;
  if (excludeDirs.includes(arr[1]))
    return;

  const fileName = arr[arr.length - 1];

  arr = fileName.split('.');
  arr.pop();

  const rez = arr.join('.');
  return rez;
}
