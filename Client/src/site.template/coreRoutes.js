import { authRoutes } from "router";
import { accountRoutes } from "router";
import { miscRoutes } from "router";
import { personalRoutes } from "router";
import { adminRoutes } from "router";

export default [
	...authRoutes,
	...accountRoutes,
	...miscRoutes,
	...personalRoutes,
	...adminRoutes
];
