import { Login } from "sun";
import { Register } from "sun";
import { RegisterEmailResult } from "sun";

const routes = [
	{
		name: "Login",
		path: "/auth/login/:ret?",
		component: Login,
		props: true
	},
	{
		name: "Register",
		path: "/auth/register",
		component: Register
	},
	{
		name: "RegisterEmailResult",
		path: "/auth/RegisterEmailResult".toLowerCase(),
		component: RegisterEmailResult
	}
];

for (let rote of routes) {
	if (!rote.meta) {
		rote.meta = {
			roles: ["Unregistered"]
		};
	}
}

export default routes;
