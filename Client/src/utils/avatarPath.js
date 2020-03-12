import { buildPath } from "utils";

export default function(image) {
	if (!image) return "/statics/default-avatar.svg";
	return buildPath(config.UrlPaths.UploadImages, image);
}
