<template>
	<div>
		<q-checkbox dense v-if="item.type === 'Boolean'" v-model="item.value" />
		<q-input
			dense
			v-else-if="item.type === 'Integer'"
			type="number"
			v-model="item.value"
		/>
		<q-input
			dense
			v-else-if="item.type === 'String'"
			type="text"
			v-model="item.value"
		/>
		<q-select
			dense
			v-else-if="enums && item.type === 'Enum'"
			:options="enums[item.enum]"
			v-model="item.value"
		/>
		<SunEditor
			height="5rem"
			min-height="3rem"
			v-else-if="item.type === 'HtmlString'"
			v-model="item.value"
		/>
		<q-input
			dense
			v-else-if="item.type === 'JsonString'"
			type="text"
			:rules="jsonRules"
			v-model="item.value"
		/>
		<q-input dense type="text" v-else v-model="item.value" />
	</div>
</template>

<script>
import { jsonRules } from "sun";

export default {
	name: "ConfigItem",
	props: {
		item: {
			type: Object,
			required: true
		},
		enums: {
			type: Object,
			required: false
		}
	},
	computed: {
		jsonRules() {
			return jsonRules();
		}
	}
};
</script>

<style lang="scss">
.config-item__name-column {
	width: 150px !important;
	padding: 15px !important;
}
</style>
