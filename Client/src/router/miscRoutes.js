import { Profile } from "sun";
import { CreateMaterial } from "sun";
import { EditMaterial } from "sun";
import { SendPrivateMessage } from "sun";

const routes = [
	{
		name: "CreateMaterial",
		path:
			"/CreateMaterial/".toLowerCase() + ":categoriesNames/:initialCategoryName?",
		components: {
			default: CreateMaterial,
			navigation: null
		},
		props: {
			default: true
		},
		meta: {
			roles: ["Registered"]
		}
	},
	{
		name: "EditMaterial",
		path: "/EditMaterial/".toLowerCase() + ":id",
		components: {
			default: EditMaterial,
			navigation: null
		},
		props: {
			default: route => {
				return {
					id: +route.params.id
				};
			},
			navigation: null
		},
		meta: {
			roles: ["Registered"]
		}
	},
	{
		name: "SendPrivateMessage",
		path: "/SendPrivateMessage".toLowerCase(),
		components: {
			default: SendPrivateMessage,
			navigation: null
		},
		props: {
			default: route => {
				return {
					userId: route.query.userId,
					userName: route.query.userName,
					userLink: route.query.userLink
				};
			}
		}
	},
	{
		name: "User",
		path: "/user/:link",
		components: {
			default: Profile,
			navigation: null
		},
		props: {
			default: true
		}
	}
];

export default routes;
