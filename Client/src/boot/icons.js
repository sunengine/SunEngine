import { lineAwesome } from "icons";
import { fontawesomeV5 } from "icons";

export default ({ Vue }) => {
	Vue.prototype.$iconsSets = {
		LineAwesome: lineAwesome,
		"line-awesome": lineAwesome,
		FontAwesome: fontawesomeV5,
		"fontawesome-v5": fontawesomeV5
	};
	Vue.prototype.$iconsSet = lineAwesome;
};
