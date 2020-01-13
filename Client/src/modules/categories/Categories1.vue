<template>
	<q-list class="categories categories1" no-border dense v-if="category">
		<q-item
			:to="category.getRoute()"
			v-for="category in subCategories"
			:key="category.id"
		>
			<q-item-section v-if="category.icon" avatar>
				<q-icon :name="category.icon" />
			</q-item-section>
			<q-item-section>
				<q-item-label>
					{{ category.title }}
				</q-item-label>
				<q-item-label v-if="category.subTitle" caption="">
					{{ category.subTitle }}
				</q-item-label>
			</q-item-section>
		</q-item>
	</q-list>
</template>

<script>
export default {
	name: "Categories1",
	props: {
		categoryName: {
			type: String,
			required: true
		}
	},
	computed: {
		subCategories() {
			return this.category?.subCategories?.filter(x => !x.isHidden);
		},
		category() {
			return this.$store.getters.getCategory(this.categoryName);
		}
	}
};
</script>

<style lang="scss">
.categories1 {
	padding: 0 !important;

	.q-item__section--avatar {
		min-width: unset;
	}

	.q-item__section--side {
		padding-right: 10px;
	}

	.q-icon {
		font-size: 20px !important;
		color: #a3a3a3;
	}
}
</style>
