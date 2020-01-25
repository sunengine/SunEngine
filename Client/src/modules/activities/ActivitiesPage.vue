<template>
	<SunPage class="activities-page">
		<PageHeader
			class="page-padding"
			:title="component.settings.Title"
			:subTitle="component.settings.SubTitle"
		/>

		<ActivitiesList :componentName="componentName" />
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "ActivitiesPage",
	mixins: [Page],
	props: {
		componentName: {
			type: String,
			required: true
		}
	},
	computed: {
		component() {
			return this.$store.getters.getComponent(this.componentName);
		}
	},
	beforeCreate() {
		this.$options.centered = true;
		this.$options.components.ActivitiesList = require("sun").ActivitiesList;
	},
	created() {
		this.title = this.component.settings.Title ?? this.$tl("defaultTitle");
	}
};
</script>

<style lang="scss">
.activities-page {
}
</style>
