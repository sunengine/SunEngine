import { IndexPage } from "sun";
import { News2ColPage } from "sun";
import { MaterialInlinePage } from "sun";
import { SecretPage } from "sun";
import { coreRoutes } from "sun";

const siteRoutes = [
	{
		name: "Home",
		path: "/",
		component: IndexPage
	},
	{
		name: "News2ColPage",
		path: "/News2ColPage".toLowerCase(),
		components: {
			default: News2ColPage
		}
	},
	{
		name: "MaterialInlinePage",
		path: "/MaterialInlinePage".toLowerCase(),
		components: {
			default: MaterialInlinePage
		}
	},
	{
		name: "Secret",
		path: "/secret",
		components: {
			default: SecretPage,
			navigation: null
		},
		meta: {
			roles: ["Registered"]
		}
	}
];

export default [...coreRoutes, ...siteRoutes];
