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
export default {
	name: "ActivitiesPage",
	components: {
		ActivitiesList: sunImport.ActivitiesList
	},
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
