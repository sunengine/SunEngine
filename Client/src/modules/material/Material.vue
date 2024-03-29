﻿<template>
	<SunPage class="material">
		<div v-if="material" class="page-padding">
			<article>
				<PageHeader
					v-if="showTitle"
					:title="material.title"
					:subTitle="material.subTitle"
				/>

                <q-carousel class="material__carousel"   transition-prev="jump-right"
                               transition-next="jump-left"
                               swipeable
                               animated
                               control-color="carousel"
                               navigation
                               infinite
                               :fullscreen.sync="isFullscreenCarousel"
                               padding
                               arrows
                               v-model="carouselCurrent" v-if="carouselTags" >
                    <template v-slot:control>
                        <q-carousel-control
                            position="bottom-right"
                            :offset="[12, 75]"
                        >
                            <q-btn
                                push round dense color="white" text-color="primary"
                                :icon="isFullscreenCarousel ? $iconsSet.Material.fullscreen : $iconsSet.Material.fullscreenExit"
                                @click="isFullscreenCarousel = !isFullscreenCarousel"
                            />
                        </q-carousel-control>
                    </template>
                    <q-carousel-slide :name="'carousel-key'+i" v-for="(tag,i) in carouselTags" :key="'carousel-key'+i" class="column no-wrap flex-center">
                       <div class="material__carousel-div" v-html="tag"></div>
                    </q-carousel-slide>
                </q-carousel>

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

		<div class="material__write-comment q-mt-md" v-if="material && comments && canCommentWrite">
			<CreateComment
				class="page-padding"
				@done="commentAdded"
				:materialId="material.id"
			/>
		</div>
	</SunPage>
</template>

<script>
import deleteMaterial from "./methods/deleteMaterial";
import restoreMaterial from "./methods/restoreMaterial";
import canDeleteMaterial from "./methods/canDeleteMaterial";
import canRestoreMaterial from "./methods/canRestoreMaterial";
import prepareLocalLinks from "src/utils/prepareLocalLinks";
import prepareParagraphs from "src/utils/prepareParagraphs";
import execScripts from "src/utils/execScripts";

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
			headersPrepared: false,
            carouselTags: null,
            carouselCurrent:null,
            isFullscreenCarousel: false
		};
	},
	watch: {
		$route(old1, new1) {
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
			return this.$store.getters.getCategory(this.categoryName);
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

		loadDataMaterial() {
			return this.$request(this.$Api.Materials.Get, {
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
                    this.preapairCarousel();
                    prepareLocalLinks.call(this, this.$el, "material__text");
                    prepareParagraphs.call(this, this.$el, "material__text");
					if (this.material.settingsJson?.allowInnerJavaScript)
						execScripts(this.$el);
				});
			});
		},
        preapairCarousel() {
            const els = Array.from(this.$el.querySelectorAll("[carousel]"));
            if(!els || els.length === 0)
                return;
            this.carouselTags = els.map(x=>x.outerHTML);
            this.carouselCurrent = "carousel-key"+0;
            els.forEach(x=>x.remove());
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
			this.headersPrepared = false;
			await this.loadDataMaterial();
			await this.loadDataComments();
		}
	},
	beforeCreate() {
		this.$options.centered = true;
		this.$options.components.CommentContainer = require("comments").CommentContainer;
		this.$options.components.CreateComment = require("comments").CreateComment;
		this.$options.components.Article = require("articles").Article;
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

.material__carousel-div {
    resize: none !important;
    height: 100%;
}

.material__carousel-div > * {
    resize: none !important;
    height: 100%;
}

.material__carousel {
    height: 500px;
}
</style>
