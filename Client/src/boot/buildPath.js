import {buildPath} from 'sun'

export default async ({ Vue }) => {
  Vue.prototype.$buildPath = buildPath;
}
