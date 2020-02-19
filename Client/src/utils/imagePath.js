import { buildPath } from "sun";

export default function(image) {
	return buildPath(config.UrlPaths.UploadImages, image);
}
