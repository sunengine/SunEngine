<template>
	<SunPage class="skins-admin page-padding">
		<PageHeader :title="title" />

		<q-tabs class="bg-grey-2" align="justify" inline-label>
			<q-route-tab
				:to="{ name: 'MainSkinsAdmin' }"
				no-caps
				name="skins"
				:icon="$iconsSet.SkinsAdmin.main"
				:label="$tl('mainSkins')"
			/>
			<q-route-tab
				:to="{ name: 'PartialSkinsAdmin' }"
				no-caps
				name="partial-skins"
				:icon="$iconsSet.SkinsAdmin.partial"
				:label="$tl('partialSkins')"
			/>
			<q-route-tab
				:to="{ name: 'CustomCssAdmin' }"
				no-caps
				name="custom-css"
				:icon="$iconsSet.SkinsAdmin.customCss"
				:label="$tl('customCss')"
			/>
		</q-tabs>

		<div class="q-mb-xl"></div>

		<router-view />
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "SkinsAdmin",
	mixins: [Page],
	data() {
		return {
			tab: "skins"
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Admin");
		}
	},
	beforeCreate() {
		this.$options.components.MainSkinsAdmin = require("sun").MainSkinsAdmin;
		this.$options.components.PartialSkinsAdmin = require("sun").PartialSkinsAdmin;
		this.$options.components.CustomCssAdmin = require("sun").CustomCssAdmin;
	},
	created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss">
.skins-admin {
	.q-tab__label {
		font-weight: bold;
	}
}

.skins-admin__tabs-panel {
	padding: 0;
}
</style>
