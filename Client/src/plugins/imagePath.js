import buildPath from "services/buildPath"

export default ({ Vue }) => {
  Vue.prototype.$imagePath = function(image) {
    return buildPath(process.env.UploadedImages,image);
  };
}
