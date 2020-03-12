import { buildPath } from "utils";

export default function(token) {
	return buildPath(config.UrlPaths.Api, token);
}
