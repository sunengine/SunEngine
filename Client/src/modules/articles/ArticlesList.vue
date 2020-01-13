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
	props: {
		articles: {
			type: Object,
			required: true
		}
	},
	beforeCreate() {
		this.$options.components.Article = require("sun").Article;
	}
};
</script>

<style lang="scss"></style>
