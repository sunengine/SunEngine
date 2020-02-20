import { IndexPage } from "sun";
import { PostsAndActivitiesPage } from "sun";
import { MaterialInlinePage } from "sun";
import { SecretPage } from "sun";
import { coreRoutes } from "sun";
import Vue from "vue";

const siteRoutes = [
	{
		name: "Home",
		path: "/",
		component: IndexPage
	},
	{
		name: "PostsAndActivitiesPage",
		path: "/PostsAndActivitiesPage".toLowerCase(),
		components: {
			default: PostsAndActivitiesPage
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
