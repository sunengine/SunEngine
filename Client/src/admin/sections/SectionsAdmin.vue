<template>
	<SunPage class="sections-admin page-padding">
		<PageHeader :title="$tl('title')">
			<q-btn
				:icon="$iconsSet.SectionsAdmin.add"
				class="post-btn q-mr-lg"
				type="a"
				:to="{ name: 'CreateSection' }"
				no-caps
				:label="$tl('addSectionBtn')"
			/>
		</PageHeader>

		<div class="sections-admin__sections" v-if="sections">
			<div v-for="section in sections">
				<q-icon
					:name="$iconsSet.SectionsAdmin.section"
					color="grey-6"
					class="q-mr-sm"
				/>
				{{ section.name }}
				<q-btn
					color="info"
					class="sections-admin__btn-edit q-ml-sm"
					dense
					size="10px"
					flat
					:icon="$iconsSet.SectionsAdmin.edit"
					:to="{ name: 'EditSection', params: { name: section.name } }"
				/>

				<q-btn
					color="info"
					class="sections-admin__to"
					dense
					size="10px"
					flat
					:icon="$iconsSet.SectionsAdmin.goTo"
					:to="'/' + component.name.toLowerCase()"
				/>

				<span class="q-ml-lg text-grey-7">[{{ component.type }}]</span>
			</div>
		</div>

		<LoaderWait v-else />
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "SectionsAdmin",
	mixins: [Page],
	data() {
		return {
			components: null
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Admin");
		}
	},
	methods: {
		loadData() {
			this.$request(this.$AdminApi.SectionsAdmin.GetAllSections).then(
				response => {
					this.sections = response.data;
				}
			);
		}
	},
	created() {
		this.title = this.$tl("title");
		this.loadData();
	}
};
</script>

<style lang="scss">
.components-admin {
	.components {
		font-size: 1.15em;
	}
}
</style>
