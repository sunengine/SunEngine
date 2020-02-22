<template>
	<div>
		<q-checkbox dense v-if="item.type === 'Boolean'" v-model="item.value" />
		<q-input
			ref="input"
			dense
			v-else-if="item.type === 'Integer'"
			type="number"
			v-model="item.value"
		/>
		<q-input
			ref="input"
			dense
			clearable
			v-else-if="item.type === 'String'"
			type="text"
			v-model="item.value"
		/>
		<q-input
			ref="input"
			dense
			clearable
			v-else-if="item.type === 'LongString'"
			type="textarea"
			v-model="item.value"
		/>
		<q-select
			dense
			v-else-if="enums && item.type === 'Enum'"
			:options="enums[item.enum]"
			v-model="item.value"
		/>
		<q-select
			dense
			v-else-if="item.type === 'Tokens'"
			:value="item.value.split(',') | removeWhiteSpace"
			@input="
				v => {
					item.value = v.join(',');
				}
			"
			hide-dropdown-icon
			input-debounce="0"
			new-value-mode="add"
			use-input
			multiple
			use-chips
		/>
		<SunEditor
			ref="input"
			height="5rem"
			min-height="3rem"
			v-else-if="item.type === 'HtmlString'"
			v-model="item.value"
		/>
		<q-input
			input-style="height:7rem"
			ref="input"
			dense
			v-else-if="item.type === 'JsonString'"
			type="textarea"
			hide-hint
			hide-bottom-space
			:rules="jsonRules"
			v-model="item.value"
		/>
		<q-input
			clearable
			ref="input"
			dense
			v-else
			type="text"
			v-model="item.value"
		/>
	</div>
</template>

<script>
import { jsonRules } from "sun";

export default {
	name: "ConfigItem",
    filters: {
        removeWhiteSpace(value) {
          return value.filter(x=>x)
        }
    },
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
		},
		hasError() {
			this.$refs?.input?.hasError;
		}
	},
	methods: {
		validate() {
			this.$refs?.input?.validate();
		}
	}
};
</script>

<style lang="scss">
.config-item__name-column {
	width: 200px !important;
	padding: 15px !important;
}
</style>
