import imagePath from 'sun'

export default async ({ Vue }) => {
  Vue.prototype.$imagePath = imagePath;
}
