<template>
	<div>
		<q-checkbox dense v-if="item.type === 'Boolean'" v-model="item.value" />
		<q-input
			ref="input"
			dense
			v-else-if="item.type === 'Integer'"
			type="number"
			:value="item.value"
			@input="
				v => {
					item.value = parseInt(v);
				}
			"
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
			:value="item.value.split(',').filter(x => x)"
			@input="
				v => {
					item.value = v.join(',');
				}
			"
			hide-dropdown-icon
			input-debounce="0"
			new-value-mode="add-unique"
			use-input
			multiple
			use-chips
		/>
		<SunEditor
			ref="input"
			height="5rem"
			min-height="3rem"
			v-else-if="item.type === 'Html'"
			v-model="item.value"
		/>
		<q-input
			class="jj"
			style="position: relative"
			input-style="height:7rem"
			ref="input"
			dense
			v-else-if="item.type === 'Json'"
			type="textarea"
			hide-hint
			hide-bottom-space
			:rules="jsonRules"
			v-model="item.value"
		>
			<q-btn
				round
				dense
				class="config-item__pretty-btn"
				size="12px"
				@click="prepareJson"
				style="position: absolute; top:5px; right:24px; display: none"
				:icon="$iconsSet.ConfigItem.pretty"
			>
				<q-tooltip>{{ $tl("pretty") }}</q-tooltip>
			</q-btn>
		</q-input>
		<q-select
			@loadstart="prepareRoles"
			dense
			v-else-if="item.type === 'Roles'"
			v-model="roles"
			:options="allRoles"
			multiple
			use-chips
			stack-label
			option-value="name"
			option-label="title"
		/>
		<CategoriesInput
			v-else-if="item.type === 'Categories'"
			v-model="item.value"
			categoriesNames="Root"
			multiple
			showRoot
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
	props: {
		item: {
			type: Object,
			required: true
		},
		enums: {
			type: Object,
			required: false
		},
		allRoles: {
			type: Array,
			required: true
		}
	},
	data() {
		return { roles: [] };
	},
	watch: {
		roles: "rolesUpdated"
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
		rolesUpdated() {
			this.item.value =
				this.roles
					?.map(x => x.name)
					?.filter(x => x)
					?.join(",") ?? "";
		},
		prepareRoles() {
			const selectedRoles = this.item.value?.split(",") ?? [];
			this.roles = this.allRoles.filter(x =>
				selectedRoles.some(y => y === x.name)
			);
		},
		prepareJson() {
			this.item.value = JSON.stringify(JSON.parse(this.item.value), null, 4);
		},
		validate() {
			this.$refs?.input?.validate();
		}
	},
	beforeCreate() {
		this.$options.components.CategoriesInput = require("sun").CategoriesInput;
	},
	created() {
		switch (this.item.type) {
			case "Roles":
				this.prepareRoles();
				break;
		}
	}
};
</script>

<style lang="scss">
.config-item__name-column {
	width: 200px !important;
	padding: 15px !important;
}

.q-field--focused .config-item__pretty-btn {
	display: block !important;
}
</style>
