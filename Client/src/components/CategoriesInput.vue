<template>
	<q-field
		class="material-form__category cursor-pointer"
		:label="label"
		:stack-label="stackLabel"
	>
		<template v-slot:control>
			<div tabindex="0" class="no-outline full-width">
				<q-chip
					dense
					:key="cat.name"
					v-for="cat in categories"
					:label="cat.title"
					removable
					@remove="
						v => {
							names = names.filter(x => x !== cat.name);
						}
					"
				/>
			</div>
		</template>
		<template v-if="showIcon" v-slot:prepend>
			<q-icon
				:name="
					multiple
						? $iconsSet.CategoriesInput.categories
						: $iconsSet.CategoriesInput.category
				"
				class="q-mr-xs"
			/>
		</template>
		<template v-slot:append>
			<q-icon :name="$q.iconSet.expansionItem.denseIcon"></q-icon>
		</template>
		<template v-slot:error>
			{{ $tl("validation.category.required") }}
		</template>
		<q-menu fit auto-close>
			<q-tree
				:nodes="categoriesNodes"
				default-expand-all
				:ticked.sync="names"
				tick-strategy="strict"
				node-key="name"
				label-key="title"
			>
				<template v-slot:default-header="prop">
					<div class="material-form__menu-item">
						<q-icon
							v-if="prop.node.icon"
							:name="prop.node.icon"
							class="q-ml-sm"
							:color="prop.node.iconColor"
							size="16px"
						/>
						<span class="q-ml-sm">{{ prop.node.title }}</span>
					</div>
				</template>
			</q-tree>
		</q-menu>
	</q-field>
</template>

<script>
import { getWhereToAddMultiCat } from "sun";

export default {
	name: "CategoriesInput",
	props: {
		showIcon: {
			type: Boolean,
			required: false,
			default: false
		},
		label: {
			type: String,
			required: false
		},
		stackLabel: {
			type: Boolean,
			required: false,
			default: false
		},
		multiple: {
			type: Boolean,
			required: false,
			default: false
		},
		categoriesNames: {
			type: String,
			required: false,
			default: "Root"
		},
		categoriesNamesExclude: {
			type: String,
			required: false,
			default: null
		},
		value: {
			type: String,
			required: true
		},
		getAllCategoriesFunction: {
			type: Function,
			required: false,
			default: getWhereToAddMultiCat
		}
	},
	data() {
		return {
			names: this.value.split(",")
		};
	},
	watch: {
		names() {
			this.$emit("input", this.names.join(","));
		}
	},
	computed: {
		categories() {
			return this.names.map(x => this.$store.getters.getCategory(x));
		},
		categoriesNodes() {
			return getWhereToAddMultiCat(this.categoriesNames);
		}
	}
};
</script>

<style lang="scss"></style>
