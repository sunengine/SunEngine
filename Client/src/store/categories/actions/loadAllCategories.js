import { request } from "sun";
import { Api } from "sun";
import { consoleInit } from "sun";

export default function(context, data) {
	return request(Api.Categories.GetAllCategoriesAndAccesses, {
		skipLock: data?.skipLock
	}).then(response => {
		console.info(
			"%cLoadAllCategories",
			consoleInit,
			config.Dev.LogInitExtended ? response.data : ""
		);
		context.commit("prepareAllCategories", response.data);
	});
}
