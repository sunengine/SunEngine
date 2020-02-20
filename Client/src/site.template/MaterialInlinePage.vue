<template>
	<SunPage class="material-inline-page ">
		<div class="material-inline-page__pan"></div>
		<PageHeader class="page-padding text-center" :title="title" />
		<MaterialInline
			@loaded="loaded"
			ref="inline"
			class="page-padding material-inline-page__material"
			name="inline1"
		/>
	</SunPage>
</template>

<script>
import { MaterialInline } from "sun";
import { Page } from "mixins";

export default {
	name: "MaterialInlinePage",
	mixins: [Page],
	methods: {
		loaded() {
			this.title = this.$refs.inline.material.title;
		}
	},
	beforeCreate() {
		this.$options.components.MaterialInline = require("sun").MaterialInline;
	}
};
</script>

<style lang="scss">
.material-inline-page__pan {
	background-image: url("/site/statics/svg1.svg");
	height: 200px;
	background-size: 38%;
}

.material-inline-page__material {
	text-align: center;
}
</style>
