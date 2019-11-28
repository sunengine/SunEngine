import {buildPath} from 'sun'

export default function (token) {
  return buildPath(config.global.SiteApi, token);
}
