export default ({ Vue }) => {
	const lineAwesome = sunRequire("lineAwesome");
	const fontawesomeV5 = sunRequire("fontawesomeV5");
	
	Vue.prototype.$iconsSets = {
		LineAwesome: lineAwesome,
		"line-awesome": lineAwesome,
		FontAwesome: fontawesomeV5,
		"fontawesome-v5": fontawesomeV5
	};
	Vue.prototype.$iconsSet = lineAwesome;
};
