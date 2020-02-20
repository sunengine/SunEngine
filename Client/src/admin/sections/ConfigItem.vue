<template>
	<div>
		{{ item.name }}
		<q-checkbox dense v-if="item.type === 'Boolean'" v-model="item.value" />
		<!--	<q-select
			dense
			v-else-if="item.type === 'Enum'"
			:options="enums[item.enumName]"
			v-model="item.value"
		/>-->
		<SunEditor
			height="5rem"
			min-height="3rem"
			v-else-if="item.type === 'HtmlString'"
			v-model="item.value"
		/>
		<q-input dense v-else v-model="item.value" />
		<!--	<q-input
			dense
			v-else-if="item.type === 'JsonString'"
			:type="getTypeType(item.type)"
			:rules="item.type === 'JsonString' ? rules : null"
			v-model="item.value"
		/>-->
	</div>
</template>

/* public string Name { get; set; } public object Value { get; set; } public
string Type { get; set; } public string Enum { get; set; }*/

<script>
export default {
	name: "ConfigItem",
	props: {
		item: {
			type: Object,
			required: true
		}
	},
	beforeCreate() {
		this.$options.components.LoaderSent = require("sun").LoaderSent;
		this.$options.components.SunEditor = require("sun").SunEditor;
	}
};
</script>

<style scoped></style>
