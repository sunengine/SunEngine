import { buildPath } from "utils";

export default function(image) {
	return buildPath(config.UrlPaths.UploadImages, image);
}
