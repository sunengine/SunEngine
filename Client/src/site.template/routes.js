import { coreRoutes } from "sun";
import Vue from "vue";

const siteRoutes = [
	{
		name: "Home",
		path: "/",
		component: sunImport.IndexPage
	},
	{
		name: "PostsAndActivitiesPage",
		path: "/PostsAndActivitiesPage".toLowerCase(),
		components: {
			default: sunImport.PostsAndActivitiesPage
		},
		props: {
			default: () => {
				return {
					pageTitle: Vue.prototype.i18n.t("PostsAndActivitiesPage.title"),
					pageSubTitle: Vue.prototype.i18n.t("PostsAndActivitiesPage.subTitle"),
					postsSectionName: "Posts",
					activitiesSectionName: "Activities"
				};
			}
		}
	},
	{
		name: "MaterialInlinePage",
		path: "/MaterialInlinePage".toLowerCase(),
		components: {
			default: sunImport.MaterialInlinePage
		}
	},
	{
		name: "Secret",
		path: "/secret",
		components: {
			default: sunImport.SecretPage,
			navigation: null
		},
		meta: {
			roles: ["Registered"]
		}
	}
];

export default [...coreRoutes, ...siteRoutes];
