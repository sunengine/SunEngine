import { lineAwesome } from "sun";
import { fontawesomeV5 } from "sun";

export default ({ Vue }) => {
	Vue.prototype.$iconsSets = {
		"LineAwesome": lineAwesome,
		"line-awesome": lineAwesome,
		"FontAwesome": fontawesomeV5,
		"fontawesome-v5": fontawesomeV5
	};
	Vue.prototype.$iconsSet = lineAwesome;
};
