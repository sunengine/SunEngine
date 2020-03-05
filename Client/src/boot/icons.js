export default ({ Vue }) => {
	const lineAwesome = require("sun").lineAwesome;;
	const fontawesomeV5 = require("sun").fontawesomeV5;;
	
	Vue.prototype.$iconsSets = {
		LineAwesome: lineAwesome,
		"line-awesome": lineAwesome,
		FontAwesome: fontawesomeV5,
		"fontawesome-v5": fontawesomeV5
	};
	Vue.prototype.$iconsSet = lineAwesome;
};
