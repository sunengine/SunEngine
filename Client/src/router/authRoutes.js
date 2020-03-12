const routes = [
	{
		name: "Login",
		path: "/auth/login/:ret?",
		component: sunImport("auth", "Login"),
		props: true
	},
	{
		name: "Register",
		path: "/auth/register",
		component: sunImport("auth", "Register")
	},
	{
		name: "RegisterEmailResult",
		path: "/auth/RegisterEmailResult".toLowerCase(),
		component: sunImport("auth", "RegisterEmailResult")
	}
];

for (const rote of routes) {
	if (!rote.meta) {
		rote.meta = {
			roles: ["Unregistered"]
		};
	}
}

export default routes;
