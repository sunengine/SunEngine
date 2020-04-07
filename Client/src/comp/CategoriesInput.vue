<template>
	<q-field
		class="categories-input cursor-pointer"
		:label="label"
		:stack-label="!!category || stackLabel"
	>
		<template v-slot:control>
			<div tabindex="0" class="categories-input__chips no-outline full-width">
				<template v-if="multiple">
					<q-chip
						class="categories-input__chip"
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
				</template>
				<div v-else-if="category" class="categories-input__selected-category">
					{{ category.title }}
				</div>
			</div>
		</template>
		<template v-if="showIcon" v-slot:prepend>
			<q-icon
				:name="
					multiple
						? $iconsSet.CategoriesInput.categories
						: $iconsSet.CategoriesInput.category
				"
				class="categories-input__categories-icon q-mr-xs"
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
				:selected.sync="name"
				:ticked.sync="names"
				:tick-strategy="multiple ? 'strict' : undefined"
				node-key="name"
				label-key="title"
				filter="all"
				:filter-method="showCats"
			>
				<template v-slot:default-header="prop">
					<div class="material-form__menu-item">
						<q-icon
							v-if="showIcons && prop.node.selectable"
							:name="$iconsSet.CategoriesInput.category"
							class="categories-input__category-icon q-mr-xs"
							color="green"
						/>
						<span class="q-ml-sm">{{ prop.node.title }}</span>
					</div>
				</template>
			</q-tree>
		</q-menu>
	</q-field>
</template>

<script>
export default {
	name: "CategoriesInput",
	props: {
		showIcon: {
			type: Boolean,
			required: false,
			default: false
		},
		showIcons: {
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
		showRoot: {
			type: Boolean,
			required: false,
			default: false
		},
		categoriesNames: {
			type: String,
			required: false
		},
		categoriesNamesExclude: {
			type: String,
			required: false
		},
		value: {
			type: String,
			required: false
		}
	},
	data() {
		if (this.multiple) {
			if (this.value) return { names: this.value.split(","), name: null };
			else return { names: [], name: null };
		} else {
			return { names: null, name: this.value };
		}
	},
	watch: {
		names() {
			if (this.multiple) this.$emit("input", this.names.join(","));
		},
		name() {
			if (!this.multiple) this.$emit("input", this.name);
		},
		value() {
			if (this.multiple) {
				if (this.value) this.names = this.value.split(",");
				else this.names = [];
			} else {
				this.name = this.value;
			}
		}
	},
	computed: {
		category() {
			if (!this.multiple) return this.$store.getters.getCategory(this.name);
		},
		categories() {
			if (this.multiple)
				return this.names?.map(x => this.$store.getters.getCategory(x)) ?? [];
		},
		categoriesNodes() {
			if (this.showRoot) return [this.$store.getters.getCategory("Root")];
			else return this.$store.getters.getCategory("Root").children;
		},
		namesArray() {
			if (this.multiple) return this.names.split(",");
		}
	},
	methods: {
		showCats(cat, filter) {
			const showCats = this.categoriesNames?.split(",").map(x => x.trim()) ?? [];
			let current = cat;

			while (current) {
				if (showCats.some(x => x === current.name)) return true;
				current = current.parent;
			}
			return false;
		}
	}
};
</script>

<style lang="scss"></style>
