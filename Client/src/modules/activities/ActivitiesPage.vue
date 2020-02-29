<template>
	<SunPage class="activities-page">
		<PageHeader
			class="page-padding"
			:title="section.options.Title"
			:subTitle="section.options.SubTitle"
		/>

		<ActivitiesList :sectionName="sectionName" />
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "ActivitiesPage",
	mixins: [Page],
	props: {
		sectionName: {
			type: String,
			required: true
		}
	},
	computed: {
		section() {
			return this.$store.getters.getSection(this.sectionName);
		}
	},
	beforeCreate() {
		this.$options.centered = true;
		this.$options.components.ActivitiesList = sunRequire("ActivitiesList");
	},
	created() {
		this.title = this.section.options.Title ?? this.$tl("defaultTitle");
	}
};
</script>

<style lang="scss">
.activities-page {
}
</style>
