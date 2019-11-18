<template>

  <q-page class="material">

    <div v-if="material" class="page-padding">
      <h2 v-if="showTitle" class="page-title">
        {{material.title}}
      </h2>

      <div v-else class="page-padding-top"></div>

      <div class="material__category q-mb-md" v-if="showCategory">
        <span class="material__category-label text-grey-7">{{$tl("category")}} </span>
        <router-link :to="category.getRoute()">{{category.title}}</router-link>
      </div>

      <div v-if="material.deletedDate" class="text-red q-mb-md">
        <q-chip icon="fas fa-trash" color="red" text-color="white" :label="$tl('deleted')"/>
      </div>

      <div class="material__text q-mb-lg" v-html="material.text">
      </div>

      <div v-if="material.tags && material.tags.length > 0" class="material__tags q-mt-lg">
        {{$tl("tags")}}
        <q-chip class="q-mx-xs" dense v-for="tag in material.tags" :key="tag">
          {{tag}}
        </q-chip>
      </div>

      <div class="material__footer q-gutter-x-lg q-py-sm flex align-center">

        <div v-if="showUser" class="material__author q-mr-md">
          <router-link :to="{name: 'User', params: {link: material.authorLink}}">
            <img class="avatar material__avatar" :src="$imagePath(material.authorAvatar)"/>{{material.authorName}}
          </router-link>
        </div>

        <div class="grow"></div>

        <div class="material-edit-btn edit-btn-block" v-if="canEdit">
          <a href="#" @click.prevent="$router.push({name: 'EditMaterial', params: {id: material.id}})">
            <q-icon name="fas fa-edit" class="q-mr-xs"/>
            {{$tl("edit")}}</a>
        </div>

        <div v-if="!material.deletedDate && canDelete" class="material-footer-info-block">
          <a href="#" @click.prevent="deleteMaterial">
            <q-icon name="fas fa-trash"/>
          </a>
        </div>

        <div v-if="material.deletedDate && canRestore" class="material-footer-info-block">
          <a href="#" @click.prevent="restoreMaterial">
            <q-icon name="fas fa-trash-restore"/>
          </a>
        </div>

        <div v-if="showVisitsCount" class="material__visits material-footer-info-block">
          <q-icon name="far fa-eye" class="q-mr-xs"/>
          {{material.visitsCount}}
        </div>
        <div v-if="showDate" class="material__date material-footer-info-block">
          <q-icon name="far fa-clock" class="q-mr-xs"/>
          {{$formatDate(material.publishDate)}}
        </div>

      </div>

      <div class="clear"></div>
    </div>

    <div id="material-comments" v-if="material && comments && comments.length > 0" class="material__comments">

      <hr class="material__comments-sep"/>

      <div v-for="(comment,index) in comments" :key="comment.id">
        <CommentContainer class="page-padding" :comment="comment" :checkLastOwn="checkLastOwn"
                          :categoryPersonalAccess="categoryPersonalAccess"
                          :isLast="index === maxCommentNumber"/>
        <hr class="material__comments-sep"/>
      </div>
    </div>

    <LoaderWait v-if="!material || !comments"/>

    <div class="material__write-comment q-mt-md" v-if="canCommentWrite">
      <CreateComment class="page-padding" @done="commentAdded" :materialId="material.id"/>
    </div>

  </q-page>

</template>

<script>
    import {Page} from 'mixins'
    import {deleteMaterial} from 'sun'
    import {restoreMaterial} from 'sun'
    import {canDeleteMaterial} from 'sun'
    import {canRestoreMaterial} from 'sun'
    import {prepareLocalLinks} from 'sun'


    import {date} from 'quasar'
    import {scroll} from 'quasar'
    import LoaderWait from "../../components/LoaderWait";

    const {getScrollTarget, setScrollPosition} = scroll;


    export default {
        name: 'Material',
        components: {LoaderWait},
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
            }
        },
        watch: {
            '$route': 'loadData',
        },
        computed: {
            maxCommentNumber() {
                return this.comments.length - 1;
            },
            category() {
                return this.$store.getters.getCategory(this.categoryName);
            },
            showTitle() {
                return this.category
                    && !(this.category.settingsJson?.hideTitle || this.material.settingsJson?.hideTitle);
            },
            showCategory() {
                return this.category
                    && !(this.category.settingsJson?.hideCategory || this.material.settingsJson?.hideCategory);
            },
            showDate() {
                return this.category
                    && (this.canEdit || !(this.category.settingsJson?.hideFooter || this.material.settingsJson?.hideFooter));
            },
            showVisitsCount() {
                return this.category
                    && (this.canEdit || !(this.category.settingsJson?.hideFooter || this.material.settingsJson?.hideFooter));
            },
            showUser() {
                return this.category
                    && (this.canEdit || !(this.category.settingsJson?.hideFooter || this.material.settingsJson?.hideFooter));
            },
            canCommentWrite() {
                if (!this.material || this.material.isCommentsBlocked)
                    return false;
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
                if (!category.categoryPersonalAccess.MaterialEditOwnIfHasReplies &&
                    this.comments.length >= 1 && !this.checkLastOwn(this.comments[0])
                ) {
                    return false;
                }
                if (!category.categoryPersonalAccess.MaterialEditOwnIfTimeNotExceeded) {
                    const now = new Date();
                    const publish = new Date(this.material.publishDate);
                    const til = date.addToDate(publish, {minutes: variables.Materials.TimeToOwnEditInMinutes});
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
            prepareLocalLinks() {
                prepareLocalLinks.call(this, this.$el, 'material__text');
            },
            async loadDataMaterial() {
                await this.$request(
                    this.$Api.Materials.Get,
                    {
                        idOrName: this.idOrName
                    }).then((response) => {
                        this.material = response.data;
                        if (this.material.settingsJson) {
                            try {
                                this.material.settingsJson = JSON.parse(this.material.settingsJson);
                            } catch (e) {

                            }
                        }
                        this.title = this.material.title;
                        this.$nextTick(() => {
                            this.prepareLocalLinks();
                        })
                    }
                );
            },
            async loadDataComments() {
                await this.$request(
                    this.$Api.Comments.GetMaterialComments,
                    {
                        materialId: this.material.id
                    }).then(response => {
                        this.comments = response.data;
                        this.$nextTick(function () {
                            if (this.$route.hash) {
                                const el = document.getElementById(this.$route.hash.substring(1));
                                setScrollPosition(getScrollTarget(el), el.offsetTop, 300);
                            }
                        });
                    }
                );
            },
            checkLastOwn(comment) {
                if (!this.comments)
                    return false;

                let userId = this.$store.state.auth.user.id;
                let ind = this.comments.indexOf(comment);
                for (let i = ind; i < this.comments.length; i++) {
                    if (this.comments[i].authorId !== userId)
                        return false;
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
                let ind = currentPath.lastIndexOf('#');
                let path = currentPath.substring(0, ind);
                window.history.pushState('', document.title, path);
                await this.loadData();
            },
            async loadData() {
                await this.loadDataMaterial();
                await this.loadDataComments();
            }
        },
        beforeCreate() {
            this.$options.components.CommentContainer = require('sun').CommentContainer;
            this.$options.components.CreateComment = require('sun').CreateComment;
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        },
        async created() {
            await this.loadData();
        }
    }

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
    width: 42px !important;
    height: 42px !important;
    margin-right: 12px;
  }

  .material__category {
    margin-top: -10px;
  }

  .material__tags {
    text-align: center
  }

</style>
