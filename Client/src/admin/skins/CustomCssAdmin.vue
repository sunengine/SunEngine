<template>
	<div class="custom-css-admin q-gutter-y-md">
		<div class="flex q-gutter-md">
			<q-btn
				no-caps
				:icon="$iconsSet.CustomCssAdmin.save"
				@click="updateCustomCss"
				class="send-btn"
				:label="$tl('saveBtn')"
			/>
			<q-btn
				@click="clearCss"
				no-caps
				:icon="$iconsSet.CustomCssAdmin.clean"
				class="clear-btn"
				color="warning"
				:label="$tl('clearBtn')"
			/>
			<q-space />
			<q-btn
				no-caps
				:icon="$iconsSet.CustomCssAdmin.reset"
				@click="reloadData"
				class="refresh-btn"
				:label="$tl('refreshBtn')"
			/>
		</div>
		<q-input
			filled
			:label="$tl('cssInput')"
			input-class="custom-css-admin__text-area"
			type="textarea"
			v-model="customCss"
		/>
		<iframe id="testFrame" class="hidden" sandbox="allow-same-origin"></iframe>
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
		clearCss() {
			const doc = document.getElementById("testFrame").contentDocument;
			doc.open();
			doc.write(`<!DOCTYPE html><title>CSS</title><style></style>`);
			doc.close();
			doc.querySelector("style").textContent = this.customCss;

			this.customCss = [...doc.styleSheets[0].cssRules]
				.map(s => s.cssText.replace("{", "{\n").replace("}", "\n}"))
				.join("\n\n");
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
