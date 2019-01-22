import imagePath from "services/imagePath"

export default ({ Vue }) => {
  Vue.prototype.$imagePath = imagePath;
}
