<template>
	<SunPage class="components-admin page-padding">
		<PageHeader :title="$tl('title')">
			<q-btn
				:icon="$iconsSet.ComponentsAdmin.add"
				class="post-btn q-mr-lg"
				type="a"
				:to="{ name: 'CreateComponent' }"
				no-caps
				:label="$tl('addComponentBtn')"
			/>
		</PageHeader>

		<div class="components-admin__components" v-if="components">
			<div v-for="component in components">
				<q-icon
					:name="$iconsSet.ComponentsAdmin.component"
					color="grey-6"
					class="q-mr-sm"
				/>
				{{ component.name }}
				<q-btn
					color="info"
					class="components-admin__btn-edit q-ml-sm"
					dense
					size="10px"
					flat
					:icon="$iconsSet.ComponentsAdmin.edit"
					:to="{ name: 'EditComponent', params: { name: component.name } }"
				/>

				<q-btn
					color="info"
					class="components-admin__to"
					dense
					size="10px"
					flat
					:icon="$iconsSet.ComponentsAdmin.goTo"
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
	name: "ComponentsAdmin",
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
			this.$request(this.$AdminApi.ComponentsAdmin.GetAllComponents).then(
				response => {
					this.components = response.data;
				}
			);
		}
	},
	beforeCreate() {
		this.$options.components.LoaderWait = require("sun").LoaderWait;
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
