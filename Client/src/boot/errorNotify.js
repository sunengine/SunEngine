export default async ({ Vue }) => {
	Vue.prototype.$errorNotify = function(error, ...values) {
		let errors = error?.response?.data?.errors;
		if (!errors) errors = [error?.response?.data];
		if (!errors) return;
		console.log("errors",errors);
		for (const error of errors) {
			console.log("error",error);
			
			const token = "Errors." + error.code;

			let localizeDescription;
			if (Vue.prototype.i18n.te(token)) localizeDescription = Vue.prototype.i18n.t(token, values);
			else localizeDescription = error.description;

			let errorText = `Error code: ${error.code}\n`;
			errorText += `Description: ${error.description}\nLocalize description: ${localizeDescription}\n`;

			if (error.message) errorText += `Message: ${error.message}\n`;
			if (error.stackTrace) errorText += `StackTrace: ${error.stackTrace}`;

			console.error(errorText);

			const color = error.type.toLowerCase() === "soft" ? "warning" : "negative";

			Vue.prototype.$q.notify({
				message: localizeDescription,
				timeout: 2800,
				color: color,
				position: "top"
			});
		}
	};
};
