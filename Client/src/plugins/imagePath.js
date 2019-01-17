import buildPath from "services/buildPath"
import config from "services/config"

export default ({ Vue }) => {
  Vue.prototype.$imagePath = function(image) {
    return buildPath(config.UploadedImages,image);
  };
}
