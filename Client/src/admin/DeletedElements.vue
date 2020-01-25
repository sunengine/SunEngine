<template>
	<SunPage class="deleted-elements page-padding">
		<PageHeader :title="title" />

		<q-checkbox
			class="deleted-elements__show-deleted"
			:toggle-indeterminate="false"
			v-model="$store.state.admin.showDeletedElements"
		>
			{{ $tl("showDeleted") }}
		</q-checkbox>

		<div class="deleted-elements__info-box">
			{{ $tl("info1") }}
			<br />
			<br />
			{{ $tl("info2") }}
			<br />
			<br />
			{{ $tl("info3") }}
		</div>

		<q-btn
			class="q-mt-md"
			color="primary"
			no-caps
			:icon="$iconsSet.DeletedElements.trashBtn"
			:label="$tl('btnDeleteAllMarkedComments')"
			:loading="loading"
			@click="deleteAllMarkedComments()"
		>
			<LoaderSent slot="loading" />
		</q-btn>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "DeletedElements",
	mixins: [Page],
	data() {
		return {
			loading: false
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Admin");
		}
	},
	beforeCreate() {
		this.$options.components.LoaderSent = require("sun").LoaderSent;
	},
	created() {
		this.title = this.$tl("title");
	},
	methods: {
		deleteAllMarkedComments() {
			this.loading = true;
			this.$request(this.$AdminApi.DeletedElements.DeleteAllMarkedComments)
				.then(response => {
					this.loading = false;
					const deletedCounts = {
						materialsCount: response.data.deletedMaterials,
						commentsCount: response.data.deletedComments
					};
					this.$successNotify(this.$tl("deleteSuccess", deletedCounts));
				})
				.catch(err => {
					this.loading = false;
					this.$errorNotify(err);
				});
		}
	}
};
</script>

<style lang="scss">
.deleted-elements__info-box {
	border-radius: 6px;
	padding: 15px;
	margin: 10px 0;
	border: 1px solid #d8d8d8;
}
</style>
