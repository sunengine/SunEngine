export default ({ Vue }) => {
	Vue.prototype.$request = sunRequire("request");
};
