<template>
	<SunPage class="sections-admin page-padding">
		<PageHeader :title="$tl('title')">
			<q-btn-dropdown
				:icon="$iconsSet.SectionsAdmin.add"
				class="post-btn q-mr-lg"
				type="a"
				:label="$tl('addSectionBtn')"
			>
				<q-list>
					<q-item
						:key="templateName"
						v-for="templateName in templatesNames"
						v-close-popup
						no-caps
						:to="{ name: 'CreateSection', params: { templateName: templateName } }"
					>
						<q-item-section>
							<q-item-label>{{ $t(`SectionsEditor.${templateName}.name`) }}</q-item-label>
						</q-item-section>
					</q-item>
				</q-list>
			</q-btn-dropdown>
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
					:to="'/' + section.name.toLowerCase()"
				/>

				<span class="q-ml-lg text-grey-7">[{{ section.type }}]</span>
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
			sections: null
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Admin");
		},
		templatesNames() {
			const rez = Object.values(this.$store.state.sections.sectionsTypes).map(
				x => x.name
			);
			return rez;
		}
	},
	methods: {
		loadData() {
			this.$request(this.$AdminApi.SectionsAdmin.GetAllSections).then(response => {
				this.sections = response.data;
			});
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
