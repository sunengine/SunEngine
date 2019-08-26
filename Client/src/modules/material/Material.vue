<template>
  <q-page class="material">
    <div v-if="material" class="page-padding">
      <h2 v-if="showTitle" class="q-title">
        {{material.title}}
      </h2>
      <div v-else class="page-padding-top"></div>

      <div v-if="showCategory" style="margin-top: -10px;" class="q-mb-md">
        <span class="text-grey-7">{{$tl("category")}} </span>
        <router-link :to="category.getRoute()">{{category.title}}</router-link>
      </div>
      <div v-if="material.deletedDate" class="text-red q-mb-md">
        <q-chip icon="fas fa-trash" color="red" text-color="white" :label="$tl('deleted')"/>
      </div>
      <div class="material-text q-mb-lg" v-html="material.text">
      </div>
      <div v-if="material.tags && material.tags.length > 0" class="q-mt-lg" style="text-align: center">
        {{$tl("tags")}}
        <q-chip class="q-mx-xs" dense v-for="tag in material.tags" :key="tag">
          {{tag}}
        </q-chip>
      </div>
      <div class="q-py-sm text-grey-8 flex" style="align-items: center">
        <div v-if="showUser" class="q-mr-md">
          <router-link :to="{name: 'User', params: {link: material.authorLink}}">
            <img class="avatar mat-avatar" :src="$imagePath(material.authorAvatar)"/>{{material.authorName}}
          </router-link>
        </div>
        <div style="flex-grow: 1">

        </div>
        <div class="q-mr-md edit-btn-block" v-if="canEdit">
          <a href="#" style="display: inline-flex; align-items: center;"
             @click.prevent="$router.push({name: 'EditMaterial', params: {id: material.id}})">
            <q-icon name="fas fa-edit" class="q-mr-xs"/>
            {{$tl("edit")}}</a>
        </div>
        <div class="q-mr-lg" v-if="!material.deletedDate && canDelete">
          <a href="#" style="display: inline-flex; align-items: center;"
             @click.prevent="deleteMaterial">
            <q-icon name="fas fa-trash"/>
          </a>
        </div>
        <div class="q-mr-md" v-if="material.deletedDate && canRestore">
          <a href="#" style="display: inline-flex; align-items: center;"
             @click.prevent="restoreMaterial">
            <q-icon name="fas fa-trash-restore"/>
          </a>
        </div>
        <div v-if="showVisitsCount" class="visits date-info-block q-mr-md">
          <q-icon name="far fa-eye" class="q-mr-xs"/>
          {{material.visitsCount}}
        </div>
        <div v-if="showDate" class="mat-date date-info-block">
          <q-icon name="far fa-clock" class="q-mr-xs"/>
          {{$formatDate(material.publishDate)}}
        </div>
      </div>

      <div class="clear"></div>
    </div>

    <div id="comments" v-if="material && comments && comments.length > 0" class="comments">
      <hr class="hr-sep"/>
      <div v-for="(comment,index) in comments" :key="comment.id">
        <CommentContainer class="page-padding" :comment="comment" :checkLastOwn="checkLastOwn"
                          :categoryPersonalAccess="categoryPersonalAccess"
                          :isLast="index === maxCommentNumber"/>
        <hr class="hr-sep"/>
      </div>
      <div v-if="canCommentWrite">
        <CreateComment class="page-padding" @done="commentAdded" :materialId="material.id"/>
      </div>
    </div>

    <LoaderWait v-if="!material || !comments"/>
  </q-page>

</template>

<script>
  import {Page} from 'sun'
  import {deleteMaterial} from 'sun'
  import {restoreMaterial} from 'sun'
  import {canDeleteMaterial} from 'sun'
  import {canRestoreMaterial} from 'sun'
  import {prepareLocalLinks} from 'sun'


  import {date} from 'quasar'
  import {scroll} from 'quasar'

  const {getScrollTarget, setScrollPosition} = scroll;


  export default {
    name: 'Material',
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
        if (this.material.isCommentsBlocked)
          return false;
        return this.category.categoryPersonalAccess.commentWrite;
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

        if (category.categoryPersonalAccess.materialEditAny) {
          return true;
        }
        if (this.material.authorId !== this.$store.state.auth.user.id) {
          return false;
        }
        if (!category.categoryPersonalAccess.materialEditOwnIfHasReplies &&
          this.comments.length >= 1 && !this.checkLastOwn(this.comments[0])
        ) {
          return false;
        }
        if (!category.categoryPersonalAccess.materialEditOwnIfTimeNotExceeded) {
          const now = new Date();
          const publish = new Date(this.material.publishDate);
          const til = date.addToDate(publish, {minutes: config.Materials.TimeToOwnEditInMinutes});
          if (til < now) {
            return false;
          }
        }
        return !!category.categoryPersonalAccess.materialEditOwn;
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
        prepareLocalLinks.call(this, this.$el, 'material-text');
      },
      async loadDataMaterial() {
        await this.$store.dispatch('request',
          {
            url: '/Materials/Get',
            data: {
              idOrName: this.idOrName
            }
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
        await this.$store.dispatch('request',
          {
            url: '/Comments/GetMaterialComments',
            data: {
              materialId: this.material.id
            }
          }).then(response => {
            this.comments = response.data;
            this.$nextTick(function () {
              if (this.$route.hash) {
                let el = document.getElementById(this.$route.hash.substring(1));
                setScrollPosition(getScrollTarget(el), el.offsetTop, 300);
              }
            });
          }
        );
      },
      checkLastOwn(comment) {
        if (!this.comments) {
          return false;
        }
        let userId = this.$store.state.auth.user.id;
        let ind = this.comments.indexOf(comment);
        for (let i = ind; i < this.comments.length; i++) {
          if (this.comments[i].authorId !== userId) {
            return false;
          }
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


<style lang="stylus">

  .material {
    .hr-sep {
      height: 0;
      border-top: solid #d3eecc 1px !important;
      border-left: none;
    }

    .mat-avatar {
      margin-right: 12px;
    }

    .comments {
      margin-top: 18px;
    }

    .q-chip {
      background-color: #e5fbe3;
    }
  }

</style>
