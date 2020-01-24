<template>
	<section>
		<article>
			<div :id="'comment-' + comment.id" class="comment">
				<q-avatar class="avatar comment__avatar">
					<img :src="$avatarPath(comment.authorAvatar)" />
				</q-avatar>
				<div class="q-my-md">
					<div class="q-mb-xs flex">
						<router-link
							class="link user-link"
							:to="{ name: 'User', params: { link: comment.authorLink } }"
						>
							{{ comment.authorName }}
						</router-link>
						<q-space />
						<div class="edit-btn-block q-gutter-x-lg q-mb-sm">
							<span v-if="canEdit">
								<a class="link" href="#" @click.prevent="$emit('goEdit')">{{
									$tl("edit")
								}}</a>
							</span>
							<span v-if="canMoveToTrash">
								<a class="link" href="#" @click.prevent="moveToTrash"
									><q-icon :name="$iconsSet.Comment.delete"
								/></a>
							</span>
							<span class="material-footer-info-block">
								<q-icon :name="$iconsSet.Comment.publishDate" class="q-mr-xs" />
								<time :datetime="$formatToSemTime(comment.publishDate)">
									{{ $formatDate(comment.publishDate) }}
								</time>
							</span>
							<span>
								<a
									class="link"
									@click="linkToClipboard"
									:href="$route.path + '#comment-' + comment.id"
									>#</a
								>
							</span>
						</div>
					</div>
					<div class="comment__text" v-html="comment.text"></div>
					<div class="clear"></div>
				</div>
			</div>
		</article>
	</section>
</template>

<script>
import { prepareLocalLinks } from "sun";
import { copyToClipboard } from "quasar";

export default {
	name: "Comment",
	props: {
		comment: Object,
		canEdit: {
			type: Boolean,
			default: false
		},
		canMoveToTrash: {
			type: Boolean,
			default: false
		},
		goEdit: Function
	},
	methods: {
		linkToClipboard(e) {
			e.preventDefault();
			const link =
				window.location.href.split("#")[0] + "#comment-" + this.comment.id;
			copyToClipboard(link)
				.then(() => this.$successNotify(this.$tl("linkCopied")))
				.catch(() => this.$router.push(link));
			return false;
		},
		prepareLocalLinks() {
			prepareLocalLinks.call(this, this.$el, "comment__text");
		},
		moveToTrash() {
			const deleteDialogMessage = this.$tl("deleteDialogMessage");
			const okButtonLabel = this.$t("Global.dialog.ok");
			const cancelButtonLabel = this.$t("Global.dialog.cancel");

			this.$q
				.dialog({
					title: deleteDialogMessage,
					//message: deleteDialogMessage,
					ok: okButtonLabel,
					cancel: cancelButtonLabel
				})
				.onOk(async () => {
					await this.$request(this.$Api.Comments.MoveToTrash, {
						id: this.comment.id
					})
						.then(() => {
							const msg = this.$tl("moveToTrashSuccess");
							this.$successNotify(msg);
							this.comment.deletedDate = new Date();
						})
						.catch(error => {
							this.$errorNotify(error);
						});
				});
		}
	},
	mounted() {
		this.prepareLocalLinks();
	}
};
</script>

<style lang="scss">
.comment__avatar {
	width: 42px;
	height: 42px;
	float: left;
	margin: 2px 15px 12px 0;
}

.comment__text {
	text-align: justify;
	font-size: 20px;
	font-weight: 300;
}
</style>
