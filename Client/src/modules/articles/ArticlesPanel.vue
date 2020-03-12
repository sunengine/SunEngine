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
export default {
	name: "ArticlesPanel",
	components: {
		PanelWrapper: sunImport("components","PanelWrapper"),
	},
	props: {
		categories: {
			type: [Object, Function],
			required: true
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
	}
};
</script>

<style lang="scss"></style>
