<template>
	<div class="side-panel">
		<h4 class="side-panel__header">
			<router-link
				class="side-panel__header-link"
				v-if="titleLink"
				:to="titleLink"
			>
				<q-icon
					v-if="icon"
					:name="icon"
					size="1.4em"
					class="side-panel__header-icon q-mr-sm"
				/>
				{{ title }}
			</router-link>
			<template v-else>
				<q-icon
					v-if="icon"
					:name="icon"
					size="1.4em"
					class="side-panel__header-icon q-mr-sm"
				/>
				{{ title }}
			</template>
		</h4>
		<component v-if="wrapComponent" :is="wrapComponent" />
		<slot v-else></slot>
	</div>
</template>

<script>
export default {
	name: "PanelWrapper",
	props: {
		wrapComponentProp: Object,
		titleProp: String,
		titleLinkProp: Object,
		iconProp: String
	},
	computed: {
		wrapComponent() {
			return this.$options.wrapComponentOption ?? this.wrapComponentProp;
		},
		title() {
			return this.$options.titleOption ?? this.titleProp ?? this.$tl("title");
		},
		titleLink() {
			return this.$options.titleLinkOption ?? this.titleLinkProp;
		},
		icon() {
			return this.$options.iconOption ?? this.iconProp;
		}
	},
	wrapComponentOption: null,
	titleOption: null,
	titleLinkOption: null,
	iconOption: null
};
</script>

<style lang="scss"></style>
