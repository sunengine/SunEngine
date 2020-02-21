<template>
	<tr class="config-item">
		<td class="config-item__name-column">
			{{ item.name }}
		</td>
		<td class="config-item__value-column">
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
				v-model="item.value"
			/>
		 	<q-input dense v-else v-model="item.value" />
		</td>
	</tr>
</template>

<script>
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
	methods: {},
	beforeCreate() {
		this.$options.components.LoaderSent = require("sun").LoaderSent;
		this.$options.components.SunEditor = require("sun").SunEditor;
	}
};
</script>

<style lang="scss">
.config-item__name-column {
	width: 150px !important;
	padding: 15px !important;
}
</style>
