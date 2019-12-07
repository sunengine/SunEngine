import {request} from 'sun'
import {Api} from 'sun'


export function getDynamicConfig() {
  return request(Api.Configuration.GetDynamicConfig)
    .then(response => {

      for (const [key, value] of Object.entries(response.data)) {
        if (!config[key]) {
          config[key] = value;
        } else {
          config[key] = {...config[key], ...value};
        }
      }

      initSkins();
    })
}

export function configFreeze() {
  for (const obj in config)
    Object.freeze(obj);
  Object.freeze(config);
}

export function initSkins() {
  const camelToSnakeCase = str => str.replace(/([A-Z])/g, " $1").trim().split(' ').join('-').toLowerCase();

  const pSkins = config.Skins.PartialSkinsNames.split(",").map(x => x.trim());

  const skinEl = document.createElement("link");
  skinEl.setAttribute("rel", "stylesheet");
  skinEl.setAttribute("href", `/statics/Skins/${config.Skins.CurrentSkinName}/styles.css`);
  document.head.appendChild(skinEl);

  for (const pSkin of pSkins) {
    const pSkinEl = document.createElement("link");
    pSkinEl.setAttribute("rel", "stylesheet");
    pSkinEl.setAttribute("href", `/statics/PartialSkins/${pSkin}/styles.css`);
    document.head.appendChild(pSkinEl);
  }

  document.body.classList.add(camelToSnakeCase("Skin" + config.Skins.CurrentSkinName));
  for (const pSkin of pSkins)
    document.body.classList.add(camelToSnakeCase("PartialSkin" + pSkin));
}
