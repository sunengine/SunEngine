<template>
	<q-item
		class="article"
		:to="to"
		:class="[
			'page-padding',
			{ 'material-hidden': article.isHidden },
			{ 'material-deleted': article.deletedDate }
		]"
	>
		<q-item-section>
			<q-item-label class="material-header article__header">
				<q-icon
					 :name="$iconsSet.Article.delete"
					color="maroon"
					class="q-mr-sm"
					v-if="article.deletedDate"
				/>
				<q-icon
					 :name="$iconsSet.Article.hidden"
					v-else-if="article.isHidden"
					class="q-mr-sm"
				/>
				{{ article.title }}
				<span class="q-ml-sm" v-if="article.deletedDate">
					[{{ $tl("deleted") }}]
				</span>
				<span class="q-ml-sm" v-else-if="article.isHidden">
					[{{ $tl("hidden") }}]
				</span>
			</q-item-label>
			<q-item-label v-if="description" class="material-header-info-block" caption>
				<div v-html="description"></div>
			</q-item-label>
			<q-item-label class="material-header-info-block" caption>
				<span>
					<q-icon  :name="$iconsSet.Article.user" />
					{{ article.authorName }}
				</span>
				<span>
					<q-icon  :name="$iconsSet.Article.publishDate" />
					<time :datetime="$formatToSemTime(article.publishDate)">
						{{ $formatDate(article.publishDate) }}
					</time>
				</span>
				<span v-if="article.commentsCount > 0">
					<q-icon  :name="$iconsSet.Article.comments" />
					{{ article.commentsCount }}
				</span>
			</q-item-label>
		</q-item-section>
	</q-item>
</template>

<script>
export default {
	name: "Article",
	props: {
		article: Object,
		required: true
	},
	computed: {
		description() {
			return this.article.description?.replace(/\n/g, "<br/>");
		},
		category() {
			return this.$store.getters.getCategory(this.article.categoryName);
		},
		to() {
			return this.category.getMaterialRoute(this.article.name ?? this.article.id);
		}
	}
};
</script>

<style lang="scss">
.article {
	padding-top: 18px;
	padding-bottom: 18px;
}

.q-item__section .material-header-info-block {
	margin-top: 8px;
}
</style>
