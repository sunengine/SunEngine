<template>
	<q-list
		class="categories categories2"
		no-border
		dense
		v-if="subCategories"
		highlight
	>
		<template v-for="folder in subCategories">
			<q-item class="categories2__header">
				<q-item-section v-if="folder.icon" avatar>
					<q-icon :name="folder.icon" />
				</q-item-section>
				<q-item-section>
					<q-item-label>
						{{ folder.title }}
					</q-item-label>
					<q-item-label v-if="folder.subTitle" caption>
						{{ folder.subTitle }}
					</q-item-label>
				</q-item-section>
			</q-item>

			<q-item
				:to="category.getRoute()"
				link
				multiline
				v-for="category in folder.subCategories"
				:key="category.id"
			>
				<q-item-section v-if="category.icon" avatar>
					<q-icon :name="category.icon" />
				</q-item-section>
				<q-item-section>
					<q-item-label>
						{{ category.title }}
					</q-item-label>
					<q-item-label v-if="category.subTitle" caption>
						{{ category.subTitle }}
					</q-item-label>
				</q-item-section>
			</q-item>
		</template>
	</q-list>
</template>

<script>
export default {
	name: "Categories2",
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
.categories2__header {
	padding: 8px 16px;
	min-height: unset;
	font-size: unset;
	background-color: #e7ffc1;
	color: grey;

	.text-caption {
		color: #9c9c9c;
	}
}

.categories2 {
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
