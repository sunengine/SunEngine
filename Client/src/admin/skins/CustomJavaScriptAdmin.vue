<template>
	<div class="custom-java-script-admin q-gutter-y-md">
		<div class="flex q-gutter-md">
			<q-btn
				 no-caps
				 :icon="$iconsSet.CustomJavaScriptAdmin.save"
				 @click="updateCustomJavaScript"
				 class="send-btn"
				 :label="$tl('saveBtn')"
			/>
			<q-space />
			<q-btn
				 no-caps
				 :icon="$iconsSet.CustomJavaScriptAdmin.reset"
				 @click="reloadData"
				 class="refresh-btn"
				 :label="$tl('refreshBtn')"
			/>
		</div>
		<q-input
			 filled
			 :label="$tl('javaScriptInput')"
			 input-class="custom-java-script-admin__text-area"
			 type="textarea"
			 v-model="customJavaScript"
		/>
		<iframe id="testFrame" class="hidden" sandbox="allow-same-origin"></iframe>
	</div>
</template>

<script>
import { Page } from "mixins";

export default {
    name: "CustomJavaScriptAdmin",
    mixins: [Page],
    data() {
        return { customJavaScript: null };
    },
    methods: {
        reloadData() {
            this.loadData().then(_ => {
                this.$successNotify(this.$tl("reloadSuccessNotify"), "info");
            });
        },
        loadData() {
            return this.$request(this.$AdminApi.SkinsAdmin.GetCustomJavaScript).then(
                response => {
                    this.customJavaScript = response.data;
                }
            );
        },
        updateCustomJavaScript() {
            this.$request(this.$AdminApi.SkinsAdmin.UpdateCustomJavaScript, {
                javaScriptText: this.customJavaScript
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
.custom-java-script-admin__text-area {
 font-size: 0.9em !important;
 max-height: 600px !important;
 min-height: 400px !important;
 line-height: unset !important;
}
</style>
