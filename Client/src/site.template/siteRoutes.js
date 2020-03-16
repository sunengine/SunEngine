export default [
	{
		name: "Home",
		path: "/",
		component: sunImport("pages", "IndexPage")
	},
	{
		name: "PostsAndActivitiesPage",
		path: "/PostsAndActivitiesPage".toLowerCase(),
		components: {
			default: sunImport("pages", "PostsAndActivitiesPage")
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
			default: sunImport("site", "MaterialInlinePage")
		}
	},
	{
		name: "Secret",
		path: "/secret",
		components: {
			default: sunImport("site", "SecretPage"),
			navigation: null
		},
		meta: {
			roles: ["Registered"]
		}
	}
];
