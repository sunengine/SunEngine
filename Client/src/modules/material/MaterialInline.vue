<template>
	<div class="material-inline full-height" v-if="material">
		<div class="material-inline__text full-height" v-html="material.text"></div>
	</div>
</template>

<script>
import execScripts from "./methods/execScripts";
import prepareLocalLinks from "src/utils/prepareLocalLinks";
import prepareParagraphs from "src/utils/prepareParagraphs";


export default {
	name: "MaterialInline",
	props: {
		name: {
			type: String,
			required: true
		}
	},
	data() {
		return {
			material: null
		};
	},
	methods: {
		loadMaterial() {
			this.$request(this.$Api.Materials.Get, {
				idOrName: this.name
			})
				.then(response => {
					this.material = response.data;
                    prepareParagraphs.call(this);
                    prepareLocalLinks.call(this, this.$el, "material__text");
                    this.$emit("loaded");
					this.$nextTick(_ => {
						execScripts(this.$el);
					});
				})
				.catch(x => {
					console.log("error", x);
				});
		}
	},
	created() {
		this.loadMaterial();
	}
};
</script>

<style lang="scss"></style>
