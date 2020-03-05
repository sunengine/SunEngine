
const routes = [
	{
		name: "Login",
		path: "/auth/login/:ret?",
		component: sunImport.Login,
		props: true
	},
	{
		name: "Register",
		path: "/auth/register",
		component: sunImport.Register
	},
	{
		name: "RegisterEmailResult",
		path: "/auth/RegisterEmailResult".toLowerCase(),
		component: sunImport.RegisterEmailResult
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
