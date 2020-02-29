export default async ({ Vue }) => {
	Vue.prototype.$Api = sunRequire("Api");
};
