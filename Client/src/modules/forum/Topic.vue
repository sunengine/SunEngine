<template>
	<div
		:class="[
			'topic',
			'row',
			'items-stretch',
			{ 'material-hidden': topic.isHidden },
			{ 'material-deleted': topic.deletedDate }
		]"
	>
		<div class="col-xs-12 col-sm-8">
			<q-item class="topic__main-block page-padding" :to="to">
				<q-item-section avatar>
					<q-avatar class="avatar shadow-1" size="46px">
						<img :src="$avatarPath(topic.authorAvatar)" />
					</q-avatar>
				</q-item-section>
				<q-item-section>
					<q-item-label class="material-header">
						<q-icon
							:name="$iconsSet.Topic.deleted"
							color="maroon"
							class="q-mr-xs"
							v-if="topic.deletedDate"
						/>
						<q-icon
							:name="$iconsSet.Topic.hidden"
							v-else-if="topic.hidden"
							class="q-mr-xs"
						/>
						{{ topic.title }}
						<span class="q-ml-sm" v-if="topic.deletedDate"
							>[{{ $tl("deleted") }}]</span
						>
						<span class="q-ml-sm" v-else-if="topic.isHidden"
							>[{{ $tl("hidden") }}]</span
						>
					</q-item-label>
					<q-item-label
						v-if="topic.subTitle"
						class="topic__sub-title material-header-info-block"
						caption
					>
						<span>{{ topic.subTitle }}</span>
					</q-item-label>
					<q-item-label class="material-header-info-block" caption>
						<span>{{ topic.authorName }}</span>
						<span v-if="topic.categoryTitle">
							<q-icon :name="$iconsSet.Topic.category" />
							{{ topic.categoryTitle }}
						</span>
						<span>
							<q-icon :name="$iconsSet.Topic.publishDate" />
							<time :datetime="$formatToSemTime(topic.publishDate)">{{
								$formatDate(topic.publishDate)
							}}</time>
						</span>
						<span v-if="topic.commentsCount > 0">
							<q-icon :name="$iconsSet.Topic.comment" />
							{{ topic.commentsCount }}
						</span>
					</q-item-label>
				</q-item-section>
			</q-item>
		</div>

		<div class="topic__last-reply col-xs-12 col-sm-4" v-if="topic.lastCommentId">
			<q-item :to="toLast" class="full-height">
				<q-item-section avatar>
					<q-item-label>
						<q-avatar class="avatar shadow-1" size="46px">
							<img :src="$avatarPath(topic.lastCommentAuthorAvatar)" />
						</q-avatar>
					</q-item-label>
				</q-item-section>
				<q-item-section class="topic__info-block material-header-info-block">
					<q-item-label caption>
						<span class="xs">{{ $tl("lastFrom") }}</span>
						{{ topic.lastCommentAuthorName }}
					</q-item-label>
					<q-item-label caption>
						<q-icon :name="$iconsSet.Topic.publishDate" />
						{{ $formatDate(topic.lastCommentPublishDate) }}
					</q-item-label>
				</q-item-section>
			</q-item>
		</div>
	</div>
</template>

<script>
export default {
	name: "Topic",
	props: {
		topic: Object
	},
	computed: {
		to() {
			return this.category?.getMaterialRoute(this.topic.id);
		},
		toLast() {
			return this.category?.getMaterialRoute(this.topic.id, "#comment-last");
		},
		category() {
			return this.$store.getters.getCategory(this.topic.categoryName);
		}
	}
};
</script>

<style lang="scss">
.topic__last-reply {
	.q-item {
		padding-left: 10px;
	}
}

.topic__sub-title {
	overflow: hidden;
}

.topic__hidden-deleted-block {
	color: dimgrey !important;

	* {
		color: silver !important;
	}

	.q-avatar {
		filter: grayscale(1);
	}
}

.topic__hidden-block {
	overflow: hidden;
}

.topic {
	.q-item {
		padding-top: 18px;
		padding-bottom: 18px;
	}
}

@media (max-width: 576px) {
	.topic__last-reply {
		.q-item {
			transform: scale(0.88);
			padding-left: 44px;
			padding-top: 0;
		}
	}
}
</style>
