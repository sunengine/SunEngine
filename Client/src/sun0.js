import {consoleInit} from 'utils/consoleStyles'


const modules = {};

const all = require.context('./', true, /\.(js|vue)$/, 'sync');

const exclude = ["i18n", "index", "site-template", "mixins"];


all.keys().forEach(key => {
  const compName = filePathToComponentName(key);
  if(!compName)
    return;

  const mod = all(key);

  if (mod.default) {
    if (mod.default.name)
      modules[mod.default.name] = mod.default;
    else
      modules[compName] = mod.default;
  } else
    modules[compName] = mod;
});

function filePathToComponentName(name) {
  let arr = name.replace('\\', '/').split('/');
  if(arr.length <= 1)
    return;
  if(exclude.includes(arr[1]))
    return;

  const fileName = arr[arr.length - 1];

  arr = fileName.split('.');
  arr.pop();

  const rez = arr.join('.');
  //console.log(name, rez);
  return rez;
}

console.info('%cES Modules registered', consoleInit, config.Log.InitExtended ? modules : '');


export default modules
