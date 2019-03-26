import imagePath from "services/imagePath"

export default async ({ Vue }) => {
  Vue.prototype.$imagePath = imagePath;
}
