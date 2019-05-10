import buildPath from "services/buildPath"

export default function (image) {
  return buildPath(config.UploadedImages, image);
}
