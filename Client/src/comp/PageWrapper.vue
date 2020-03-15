﻿<template>
	<SunPage class="page-wrapper page-padding">
		<PageHeader :title="pageTitle" :icon="icon" />
		<component v-if="wrapComponent" :is="wrapComponent" />
	</SunPage>
</template>

<script>
export default {
	name: "PageWrapper",
	props: {
		wrapComponentProp: Object,
		pageTitleProp: String,
		iconProp: String
	},
	computed: {
		wrapComponent() {
			return this.$options.wrapComponentOption ?? this.wrapComponentProp;
		},
		pageTitle() {
			return (
				this.$options.pageTitleOption ?? this.pageTitleProp ?? this.$tl("title")
			);
		},
		icon() {
			return this.$options.iconOption ?? this.iconProp;
		}
	},
	wrapComponentOption: null,
	pageTitleOption: null,
	iconOption: null
};
</script>

<style lang="scss"></style>
