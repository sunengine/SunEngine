import { buildPath } from "sun";

export default function(image) {
	if (!image) return "/statics/default-avatar.svg";
	return buildPath(config.Global.UploadImagesUrl, image);
}
