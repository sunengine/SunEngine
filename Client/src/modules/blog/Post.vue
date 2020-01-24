<template>
	<section>
		<div
			:class="[
				'post',
				{ 'material-hidden': post.isHidden },
				{ 'material-deleted': post.deletedDate }
			]"
		>
			<header>
				<q-item :to="to" class="post__header page-padding">
					<q-item-section avatar>
						<q-avatar class="avatar shadow-1 post__avatar avater" size="46px">
							<img :src="$avatarPath(post.authorAvatar)" />
						</q-avatar>
					</q-item-section>
					<q-item-section>
						<div class="post__title material-header">
							<q-icon
								:name="$iconsSet.Post.deleted"
								name="fas fa-trash"
								color="maroon"
								class="q-mr-sm"
								v-if="post.deletedDate"
							/>
							<q-icon
								:name="$iconsSet.Post.hidden"
								v-else-if="post.isHidden"
								class="q-mr-sm"
							/>
							{{ post.title }}
							<span class="q-ml-sm" v-if="post.deletedDate">
								[{{ $tl("deleted") }}]
							</span>
							<span class="q-ml-sm" v-else-if="post.isHidden">
								[{{ $tl("hidden") }}]
							</span>
						</div>
						<div class="flex">
							<router-link
								:to="{ name: 'User', params: { link: post.authorLink } }"
								class="post__author-link"
							>
								{{ post.authorName }}
							</router-link>
							<q-space />
							<div class="post__date text-grey-6">
								<time :datetime="$formatToSemTime(post.publishDate)">
									<span>{{ $formatDate(post.publishDate) }}</span>
								</time>
							</div>
						</div>
					</q-item-section>
				</q-item>
			</header>

			<div
				v-if="!post.isHidden && !post.deletedDate"
				class="post__text page-padding"
				v-html="post.preview"
			></div>

			<footer>
				<div class="post__footer flex">
					<q-item
						v-if="post.hasMoreText"
						:class="{ 'post__read-more-link': true, 'page-padding-left': true }"
						:to="to"
					>
						<q-item-section avatar class="post__footer-btn-icon">
							<q-icon :name="$iconsSet.Post.readMore" size="20px" />
						</q-item-section>
						<q-item-section>
							{{ $tl("readMore") }}
						</q-item-section>
					</q-item>

					<q-item
						v-if="post.commentsCount"
						:class="{
							'page-padding-left': !post.hasMoreText,
							'post__comments-link': true
						}"
						:to="toComments"
					>
						<q-item-section class="post__footer-btn-icon" avatar>
							<q-icon :name="$iconsSet.Post.comment" size="20px" />
						</q-item-section>
						<q-item-section :class="[{ 'text-grey-6': !post.commentsCount }]">
							{{ post.commentsCount }} {{ $tl("commentsCount") }}
						</q-item-section>
					</q-item>
				</div>
			</footer>
			<div class="clear"></div>
		</div>
	</section>
</template>

<script>
import { prepareLocalLinks } from "sun";

export default {
	name: "Post",
	props: {
		post: {
			type: Object,
			required: true
		}
	},
	computed: {
		to() {
			return this.category.getMaterialRoute(this.post.id);
		},
		toComments() {
			return this.category.getMaterialRoute(this.post.id, "#material-comments");
		},
		canCommentWrite() {
			if (this.post.isCommentsBlocked) return false;
			return this.category.categoryPersonalAccess.CommentWrite;
		},
		category() {
			return this.$store.getters.getCategory(this.post.categoryName);
		}
	},
	methods: {
		prepareLocalLinks() {
			prepareLocalLinks.call(this, this.$el, "post__text");
		}
	},
	mounted() {
		this.prepareLocalLinks();
	}
};
</script>

<style lang="scss">
.post__header {
	.q-item__section--side {
		display: flex;
		padding: 2px 16px 2px 0;
	}
}

.post__avatar {
}

.post__footer-btn-icon {
	min-width: unset;
}

.post__title {
	font-size: 22px;
	color: $link-color;
}

.post__author-link {
	color: $grey-6;

	&:hover {
		color: $link-color !important;
		text-decoration: underline;
	}
}

.post__date {
	display: flex;
	align-items: center;
}

.post__footer {
	$footer-line-height: 38px;

	align-items: center;
	color: $link-color !important;
	font-size: 16px;
	font-weight: 400;

	.q-item {
		min-height: unset !important;
		height: $footer-line-height;
	}

	.q-item:first-child {
		padding-left: 0;
	}
}

.post__text {
	font-size: 18px;
	margin: 8px 0;
	font-weight: 300;
	text-align: justify;

	*:first-child {
		margin-top: 0 !important;
	}

	*:last-child {
		margin-bottom: 0 !important;
	}
}
</style>
