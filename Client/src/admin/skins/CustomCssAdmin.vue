<template>
	<div class="custom-css-admin q-gutter-y-md">
		<q-input
			filled
			input-class="custom-css-admin__text-area"
			type="textarea"
			v-model="customCss"
		/>
		<div class="clear"></div>
		<div class="flex">
			<q-btn
				no-caps
				icon="fas fa-save"
				@click="updateCustomCss"
				class="send-btn"
				:label="$tl('saveBtn')"
			/>
			<q-space />
			<q-btn
				no-caps
				icon="fas fa-sync-alt"
				@click="reloadData"
				class="refresh-btn"
				:label="$tl('refreshBtn')"
			/>
		</div>
	</div>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "CustomCssAdmin",
	mixins: [Page],
	data() {
		return { customCss: null };
	},
	methods: {
		reloadData() {
			this.loadData().then(_ => {
				this.$successNotify(this.$tl("reloadSuccessNotify"), "info");
			});
		},
		loadData() {
			return this.$request(this.$AdminApi.SkinsAdmin.GetCustomCss).then(
				response => {
					this.customCss = response.data;
				}
			);
		},
		updateCustomCss() {
			this.$request(this.$AdminApi.SkinsAdmin.UpdateCustomCss, {
				cssText: this.customCss
			}).then(_ => {
				this.$successNotify();
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
.custom-css-admin__text-area {
	font-size: 0.9em !important;
	max-height: 600px !important;
	min-height: 400px !important;
	line-height: unset !important;
}
</style>
