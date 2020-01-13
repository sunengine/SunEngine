<template>
	<div class="material-inline" v-if="material">
		<div class="material-inline__text" v-html="material.text"></div>
	</div>
</template>

<script>
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
					this.$emit("loaded");
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
