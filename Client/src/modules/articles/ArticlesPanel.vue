<template>
	<nav>
		<PanelWrapper
			class="articles-panel"
			iconProp="far fa-file-alt"
			:titleProp="$tl('sections')"
		>
			<q-item class="q-my-xs" exact dense :to="newArticlesRoute">
				<q-item-section>
					<q-item-label>
						{{ $tl("newArticles") }}
					</q-item-label>
				</q-item-section>
			</q-item>
			<component :is="categories" :categoryName="categoryName" />
		</PanelWrapper>
	</nav>
</template>

<script>
import { Categories1 } from "sun";

export default {
	name: "ArticlesPanel",
	props: {
		categories: {
			type: Object,
			default: Categories1
		},
		categoryName: {
			type: String,
			required: true
		}
	},
	computed: {
		category() {
			return this.$store.getters.getCategory(this.categoryName);
		},
		newArticlesRoute() {
			return this.category.getRoute();
		}
	},
	beforeCreate() {
		this.$options.components.PanelWrapper = require("sun").PanelWrapper;
	}
};
</script>

<style lang="scss"></style>
