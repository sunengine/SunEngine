export default async function sunImport(module, component)  {
	const mod = await moduleTable[module]();
	return mod[component];
}

export const moduleTable = {
"admin": async function() {
		const module = await import("src/index/admin.js");
		return module.default;
	},
"categories": async function() {
		const module = await import("src/index/categories.js");
		return module.default;
	},
"menuItems": async function() {
		const module = await import("src/index/menuItems.js");
		return module.default;
	},
"roles": async function() {
		const module = await import("src/index/roles.js");
		return module.default;
	},
"sections": async function() {
		const module = await import("src/index/sections.js");
		return module.default;
	},
"skins": async function() {
		const module = await import("src/index/skins.js");
		return module.default;
	},
"api": async function() {
		const module = await import("src/index/api.js");
		return module.default;
	},
"classes": async function() {
		const module = await import("src/index/classes.js");
		return module.default;
	},
"components": async function() {
		const module = await import("src/index/components.js");
		return module.default;
	},
"icons": async function() {
		const module = await import("src/index/icons.js");
		return module.default;
	},
"layouts": async function() {
		const module = await import("src/index/layouts.js");
		return module.default;
	},
"mixins": async function() {
		const module = await import("src/index/mixins.js");
		return module.default;
	},
"account": async function() {
		const module = await import("src/index/account.js");
		return module.default;
	},
"activities": async function() {
		const module = await import("src/index/activities.js");
		return module.default;
	},
"articles": async function() {
		const module = await import("src/index/articles.js");
		return module.default;
	},
"auth": async function() {
		const module = await import("src/index/auth.js");
		return module.default;
	},
"blog": async function() {
		const module = await import("src/index/blog.js");
		return module.default;
	},
"comments": async function() {
		const module = await import("src/index/comments.js");
		return module.default;
	},
"erorrs": async function() {
		const module = await import("src/index/erorrs.js");
		return module.default;
	},
"forum": async function() {
		const module = await import("src/index/forum.js");
		return module.default;
	},
"material": async function() {
		const module = await import("src/index/material.js");
		return module.default;
	},
"pages": async function() {
		const module = await import("src/index/pages.js");
		return module.default;
	},
"personal": async function() {
		const module = await import("src/index/personal.js");
		return module.default;
	},
"profile": async function() {
		const module = await import("src/index/profile.js");
		return module.default;
	},
"router": async function() {
		const module = await import("src/index/router.js");
		return module.default;
	},
"shared": async function() {
		const module = await import("src/index/shared.js");
		return module.default;
	},
"actions": async function() {
		const module = await import("src/index/actions.js");
		return module.default;
	},
"getters": async function() {
		const module = await import("src/index/getters.js");
		return module.default;
	},
"mutations": async function() {
		const module = await import("src/index/mutations.js");
		return module.default;
	},
"store": async function() {
		const module = await import("src/index/store.js");
		return module.default;
	},
"index": async function() {
		const module = await import("src/index/index.js");
		return module.default;
	},
"menu": async function() {
		const module = await import("src/index/menu.js");
		return module.default;
	},
"root": async function() {
		const module = await import("src/index/root.js");
		return module.default;
	},
"routes": async function() {
		const module = await import("src/index/routes.js");
		return module.default;
	},
"utils": async function() {
		const module = await import("src/index/utils.js");
		return module.default;
	},
"site": async function() {
		const module = await import("src/index/site.js");
		return module.default;
	},
"i18n": async function() {
		const module = await import("src/index/i18n.js");
		return module.default;
	},
};