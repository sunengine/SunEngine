import { app } from "sun";

export default function(file) {
	if (file.size > config.Images.ImageRequestSizeLimitBytes) {
		app.$q.notify({
			message: app.$t("Global.validation.fileSizeLimit", {
				fileName: file.name,
				maxSize: Math.floor(config.Images.ImageRequestSizeLimitBytes / 1048576, 2)
			}),
			timeout: 2200,
			color: "warning",
			position: "top"
		});
		return false;
	}

	return true;
}
