<template>
	<div class="articles-list">
		<q-list v-if="articles" no-border>
			<Article
				:article="article"
				v-for="article in articles.items"
				:key="article.id"
			/>
		</q-list>
	</div>
</template>

<script>
export default {
	name: "ArticlesList",
	components: {
		Article: sunImport("articles","Article"),
	},
	props: {
		articles: {
			type: Object,
			required: true
		}
	}
};
</script>

<style lang="scss"></style>
