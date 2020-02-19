import { buildPath } from "sun";

export default function(token) {
	return buildPath(config.UrlPaths.Api, token);
}
