﻿<template>
	<div
		:class="['category-item', { 'category-item--hidden': category.isHidden }]"
	>
		<span class="category-item__item-block" v-if="notRoot">
			<span class="q-mr-sm category-item__up-down">
				<q-btn
					:disabled="isFirst"
					@click="$emit('up', category)"
					color="positive"
					dense
					size="10px"
					flat
					:icon="$iconsSet.CategoryItem.up"
				>
					<q-tooltip :delay="1000">
						{{ $tl("moveUpBtnTooltip") }}
					</q-tooltip>
				</q-btn>
				<q-btn
					:disabled="isLast"
					@click="$emit('down', category)"
					color="positive"
					dense
					size="10px"
					flat
					:icon="$iconsSet.CategoryItem.down"
				>
					<q-tooltip :delay="1000">
						{{ $tl("moveDownBtnTooltip") }}
					</q-tooltip>
				</q-btn>
			</span>
			<span>
				<router-link
					class="link"
					v-if="category.isMaterialsContainer"
					:to="{ name: 'CatView', params: { categoryName: category.name } }"
					>{{ category.title }}
					<q-tooltip :delay="1000">
						{{ $tl("showCategoryBtnTooltip") }}
					</q-tooltip>
				</router-link>
				<template v-else>{{ category.title }}</template>
			</span>
			<q-icon
				class="q-ml-sm text-grey-5"
				v-if="category.isHidden"
				:name="$iconsSet.CategoryItem.eyeSlash"
			/>
			<span class="q-ml-md">
				<q-btn
					class="category-item__btn-edit"
					:to="{ name: 'EditCategory', params: { categoryId: category.id } }"
					:icon="$iconsSet.CategoryItem.edit"
					color="info"
					dense
					size="10px"
					flat
				>
					<q-tooltip :delay="1000">
						{{ $tl("editBtnTooltip") }}
					</q-tooltip>
				</q-btn>
				<q-btn
					class="category-item__btn-create"
					:to="{ name: 'CreateCategory', params: { parentCategoryId: category.id } }"
					:icon="$iconsSet.CategoryItem.plus"
					color="info"
					dense
					size="10px"
					flat
				>
					<q-tooltip :delay="1000">
						{{ $tl("createSubCategoryBtnTooltip") }}
					</q-tooltip>
				</q-btn>
				<q-btn
					class="category-item__btn-to-route"
					:disabled="!route"
					:to="route"
					:icon="$iconsSet.CategoryItem.goTo"
					color="info"
					dense
					size="10px"
					flat
				>
					<q-tooltip :delay="1000">
						{{ $tl("moveToBtnTooltip") }}
					</q-tooltip>
				</q-btn>
			</span>

			<span
				v-if="showInfo"
				class="category-item__category-name text-grey-7 q-ml-md"
				>{{ category.name }}</span
			>

			<span
				v-if="showInfo && category.materialsCount"
				class="category-item__materails-count text-grey-8 q-ml-md"
			>
				<q-icon color="grey-5" :name="$iconsSet.CategoryItem.material" />
				{{ category.materialsCount }}</span
			>
		</span>
		<div
			class="category-item__sub-categories-block"
			v-if="category.subCategories"
			:class="[{ 'padding-shift': notRoot }]"
		>
			<category-item
				:showInfo="showInfo"
				:category="sub"
				:isFirst="index === 0"
				:isLast="index === lastIndex"
				:key="sub.id"
				v-for="(sub, index) in category.subCategories"
				v-on="$listeners"
			/>
		</div>
	</div>
</template>

<script>
export default {
	name: "CategoryItem",
	props: {
		category: {
			type: Object,
			required: true
		},
		showInfo: {
			type: Boolean,
			required: false,
			default: false
		},
		isFirst: Boolean,
		isLast: Boolean
	},
	computed: {
		route() {
			return this.$store.getters.getCategory(this.category.name)?.getRoute();
		},
		notRoot() {
			return this.category.name !== "Root";
		},
		lastIndex() {
			return this.category.subCategories.length - 1;
		}
	},
	methods: {},
	data: function() {
		return {};
	}
};
</script>

<style lang="scss">
.category-item {
	.padding-shift {
		padding-left: 25px;
	}

	.q-btn:disabled,
	.q-btn[disabled] {
		filter: grayscale(1);
	}

	.desktop {
		.category-item__item-block > .category-item__up-down {
			visibility: hidden;
		}

		.category-item__item-block:hover > .category-item__up-down {
			visibility: visible;
		}
	}
}

.category-item--hidden {
	filter: grayscale(0.75);

	* {
		color: $grey-5;
	}
}
</style>
