<template>
	<SunPage class="material">
		<div v-if="material" class="page-padding">
			<article>
				<PageHeader
					v-if="showTitle"
					:title="material.title"
					:subTitle="material.subTitle"
				/>

				<div class="material__text q-mb-lg" v-html="material.text"></div>

				<footer>
					<div
						v-if="showFooter && material.tags && material.tags.length > 0"
						class="material__tags q-mt-lg"
					>
						{{ $tl("tags") }}
						<q-chip class="q-mx-xs" dense v-for="tag in material.tags" :key="tag">
							{{ tag }}
						</q-chip>
					</div>

					<div
						v-if="showFooter"
						class="material__footer q-gutter-x-lg q-py-sm flex align-center"
					>
						<div v-if="showUser" class="material__author q-mr-md">
							<router-link
								class="link user-link"
								:to="{ name: 'User', params: { link: material.authorLink } }"
							>
								<q-avatar class="avatar material__avatar">
									<img :src="$avatarPath(material.authorAvatar)" /> </q-avatar
								>{{ material.authorName }}
							</router-link>
						</div>

						<q-space />

						<div class="material-edit-btn edit-btn-block" v-if="canEdit">
							<a
								class="link"
								href="#"
								@click.prevent="
									$router.push({
										name: 'EditMaterial',
										params: { id: material.id }
									})
								"
							>
								{{ $tl("edit") }}</a
							>
						</div>

						<div
							v-if="!material.deletedDate && canDelete"
							class="material-footer-info-block"
						>
							<a class="link" href="#" @click.prevent="deleteMaterial">
								<q-icon :name="$iconsSet.Material.delete" />
							</a>
						</div>

						<div
							v-if="material.deletedDate && canRestore"
							class="material-footer-info-block"
						>
							<a class="link" href="#" @click.prevent="restoreMaterial">
								<q-icon :name="$iconsSet.Material.restore" />
							</a>
						</div>

						<div
							v-if="showVisitsCount"
							class="material__visits material-footer-info-block"
						>
							<q-icon :name="$iconsSet.Material.visits" class="q-mr-xs" />
							{{ material.visitsCount }}
						</div>
						<div v-if="showDate" class="material__date material-footer-info-block">
							<q-icon :name="$iconsSet.Material.publishDate" class="q-mr-xs" />
							<time :datetime="$formatToSemTime(material.publishDate)">
								{{ $formatDate(material.publishDate) }}
							</time>
						</div>
					</div>
				</footer>
			</article>
			<div class="clear"></div>
		</div>

		<div
			id="material-comments"
			v-if="material && comments && comments.length > 0"
			class="material__comments"
		>
			<hr class="material__comments-sep" />

			<div v-for="(comment, index) in comments" :key="comment.id">
				<CommentContainer
					class="page-padding"
					:comment="comment"
					:checkLastOwn="checkLastOwn"
					:categoryPersonalAccess="categoryPersonalAccess"
					:isLast="index === maxCommentNumber"
				/>
				<hr class="material__comments-sep" />
			</div>
		</div>

		<LoaderWait v-if="!material || !comments" />

		<div class="material__write-comment q-mt-md" v-if="canCommentWrite">
			<CreateComment
				class="page-padding"
				@done="commentAdded"
				:materialId="material.id"
			/>
		</div>
	</SunPage>
</template>

<script>
import { Page } from "mixins";
import { deleteMaterial } from "sun";
import { restoreMaterial } from "sun";
import { canDeleteMaterial } from "sun";
import { canRestoreMaterial } from "sun";
import { prepareLocalLinks } from "sun";

import { copyToClipboard, date } from "quasar";
import { scroll } from "quasar";

const { getScrollTarget, setScrollPosition } = scroll;

export default {
	name: "Material",
	mixins: [Page],
	props: {
		idOrName: {
			type: String,
			required: true
		},
		categoryName: {
			type: String,
			required: true
		}
	},
	data() {
		return {
			material: null,
			comments: null,
			page: null,
			headersPrepared: false
		};
	},
	watch: {
		$route: function(old1, new1) {
			if (old1.path !== new1.path) this.loadData();
			else {
				const el = document.getElementById(this.$route.hash.substring(1));
				setScrollPosition(getScrollTarget(el), el.offsetTop, 300);
			}
		}
	},
	computed: {
		maxCommentNumber() {
			return this.comments.length - 1;
		},
		category() {
			return (this.$store.state.currentCategory = this.$store.getters.getCategory(
				this.categoryName
			));
		},
		showTitle() {
			return (
				this.category &&
				!(
					this.category.settingsJson?.hideTitle ||
					this.material.settingsJson?.hideTitle
				)
			);
		},
		hideBreadcrumbs() {
			return (
				this.category?.settingsJson?.hideBreadcrumbs ||
				this.material?.settingsJson?.hideBreadcrumbs
			);
		},
		showDate() {
			return this.category && (this.showFooter || this.canEdit);
		},
		showVisitsCount() {
			return this.category && (this.showFooter || this.canEdit);
		},
		showUser() {
			return this.category && (this.showFooter || this.canEdit);
		},
		showFooter() {
			if (!this.category) return false;

			if (this.category.categoryPersonalAccess.MaterialEditAny) return true;

			return !(
				this.category.settingsJson?.hideFooter ||
				this.material.settingsJson?.hideFooter
			);
		},
		canCommentWrite() {
			if (!this.material || this.material.isCommentsBlocked) return false;
			return this.category.categoryPersonalAccess.CommentWrite;
		},
		categoryPersonalAccess() {
			return this.category.categoryPersonalAccess;
		},
		canEdit() {
			if (!this.material || !this.comments) {
				return false;
			}
			if (!this.$store.state.auth.user) {
				return false;
			}

			const category = this.$store.getters.getCategory(this.material.categoryName);

			if (category.categoryPersonalAccess.MaterialEditAny) {
				return true;
			}
			if (this.material.authorId !== this.$store.state.auth.user.id) {
				return false;
			}
			if (
				!category.categoryPersonalAccess.MaterialEditOwnIfHasReplies &&
				this.comments.length >= 1 &&
				!this.checkLastOwn(this.comments[0])
			) {
				return false;
			}
			if (!category.categoryPersonalAccess.MaterialEditOwnIfTimeNotExceeded) {
				const now = new Date();
				const publish = new Date(this.material.publishDate);
				const til = date.addToDate(publish, {
					minutes: config.Materials.TimeToOwnEditInMinutes
				});
				if (til < now) {
					return false;
				}
			}
			return !!category.categoryPersonalAccess.MaterialEditOwn;
		},
		canDelete() {
			return canDeleteMaterial.call(this);
		},
		canRestore() {
			return canRestoreMaterial.call(this);
		}
	},
	methods: {
		prepareParagraphs() {
			if (this.headersPrepared) return;

			const textEl = this.$el.getElementsByClassName("material__text")[0];
			const headers = textEl.querySelectorAll("h1,h2,h3,h4,h5,h6");

			const router = this.$router;
			const successNotify = this.$successNotify.bind(this);
			const tl = this.$tl.bind(this);
			const allNames = {};
			for (const header of headers) {
				const link = document.createElement("a");
				link.classList.add("header-anchor");
				link.classList.add("link");
				let id = encodeURIComponent(header.innerText);
				while(allNames[id]) 
                id = id + "1";
				allNames[id] = true;
				header.id = id;
				link.href = window.location.href.split("#")[0] + "#" + id;
				link.addEventListener("click", function(e) {
					e.preventDefault();
					copyToClipboard(link.href)
						.then(() => successNotify(tl("linkCopied")))
						.catch(() => router.replace(link.href));
					return false;
				});
				link.innerText = "#";
				header.appendChild(link);
			}

			this.headersPrepared = true;

			if (this.$route.hash) {
				const el = document.getElementById(this.$route.hash.substring(1));
				setScrollPosition(getScrollTarget(el), el.offsetTop, 300);
			}
		},
		prepareLocalLinks() {
			prepareLocalLinks.call(this, this.$el, "material__text");
		},
		async loadDataMaterial() {
			await this.$request(this.$Api.Materials.Get, {
				idOrName: this.idOrName
			}).then(response => {
				this.material = response.data;
				if (this.material.settingsJson) {
					try {
						this.material.settingsJson = JSON.parse(this.material.settingsJson);
					} catch (e) {}
				}
				this.title = this.material.title;
				this.$nextTick(() => {
					this.prepareLocalLinks();
					this.prepareParagraphs();
				});
			});
		},
		async loadDataComments() {
			await this.$request(this.$Api.Comments.GetMaterialComments, {
				materialId: this.material.id
			}).then(response => {
				this.comments = response.data;
				this.$nextTick(function() {
					if (this.$route.hash) {
						const el = document.getElementById(this.$route.hash.substring(1));
						setScrollPosition(getScrollTarget(el), el.offsetTop, 300);
					}
				});
			});
		},
		checkLastOwn(comment) {
			if (!this.comments) return false;

			let userId = this.$store.state.auth.user.id;
			let ind = this.comments.indexOf(comment);
			for (let i = ind; i < this.comments.length; i++) {
				if (this.comments[i].authorId !== userId) return false;
			}
			return true;
		},
		async deleteMaterial() {
			deleteMaterial.call(this);
		},
		async restoreMaterial() {
			restoreMaterial.call(this);
		},
		async commentAdded() {
			let currentPath = this.$route.fullPath;
			let ind = currentPath.lastIndexOf("#");
			let path = currentPath.substring(0, ind);
			window.history.pushState("", document.title, path);
			await this.loadData();
		},
		async loadData() {
			await this.loadDataMaterial();
			await this.loadDataComments();
		}
	},
	beforeCreate() {
		this.$options.centered = true;
		this.$options.components.CommentContainer = require("sun").CommentContainer;
		this.$options.components.CreateComment = require("sun").CreateComment;
		this.$options.components.Article = require("sun").Article;
	},
	created() {
		this.loadData();
	}
};
</script>

<style lang="scss">
.material {
	.q-chip {
		background-color: #e5fbe3;
	}
}

.material__comments {
	margin-top: 18px;
}

.material__comments-sep {
	height: 0;
	border-top: solid #d3eecc 1px !important;
	border-left: none;
}

.material__avatar {
	width: 42px;
	height: 42px;
	margin-right: 15px;
}

.material__tags {
	text-align: center;
}
</style>
