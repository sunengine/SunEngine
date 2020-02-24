<template>
	<q-field
		class="material-form__category cursor-pointer"
		:label="label"
		:stack-label="stackLabel"
	>
		<template v-slot:control>
			<div tabindex="0" class="no-outline full-width">
				<template v-if="multiple">
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
				</template>
				<template v-else>
					{{ category ? category.title : "select category" }}
				</template>
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
		showRoot: {
			type: Boolean,
			required: false,
			default: false
		},
		/*	categoryName: {
			type: String,
			required: false
		},*/
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
			required: true
		}
	},
	data() {
		return {
			names: this.multiple ? this.value.split(",") : null,
			name: this.multiple ? null : this.value
		};
	},
	watch: {
		names() {
			if (this.multiple) this.$emit("input", this.names.join(","));
		},
		name() {
			if (!this.multiple) this.$emit("input", this.name);
		}
	},
	computed: {
		category() {
			if (!this.multiple) return this.$store.getters.getCategory(this.name);
		},
		categories() {
			if (this.multiple)
				return this.names.map(x => this.$store.getters.getCategory(x));
		},
		categoriesNodes() {
			if (this.showRoot) {
				return [this.$store.getters.getCategory("Root")];
			} else {
				return [...this.$store.getters.getCategory("Root").children];
			}
		},
		namesArray() {
			if (this.multiple) return this.names.split(",");
		}
	},
	methods: {
		showCats(cat, filter) {
			const showCats = this.categoriesNames.split(",").map(x => x.trim());
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
