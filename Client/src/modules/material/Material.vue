<template>
  <q-page>
    <div v-if="material" class="page-padding">
      <h2 class="q-title">
        {{material.title}}
      </h2>
      <div v-if="category" style="margin-top: -10px;" class="q-mb-md">
        <span class="text-grey-7">{{$tl("category")}} </span>
        <router-link :to="categoryPath">{{category.title}}</router-link>
      </div>
      <div class="material q-mb-lg" v-html="material.text">
      </div>
      <div v-if="material.tags && material.tags.length > 0" class="q-mt-lg" style="text-align: center">
        {{$tl("tags")}}
        <q-chip class="q-mx-xs" dense color="info" v-for="tag in material.tags" :key="tag">
          {{tag}}
        </q-chip>
      </div>
      <div class="q-py-sm text-grey-8 flex" style="align-items: center">
        <div class="q-mr-md">
          <router-link :to="{name: 'User', params: {link: material.authorLink}}">
            <img class="avatar mat-avatar" :src="$imagePath(material.authorAvatar)"/>{{material.authorName}}
          </router-link>
        </div>
        <div style="flex-grow: 1">

        </div>
        <div class="q-mr-md" v-if="canEdit">
          <a href="#" style="display: inline-flex; align-items: center;"
             @click.prevent="$router.push({name: 'EditMaterial', params: {id: material.id}})">
            <q-icon name="fas fa-edit" class="q-mr-xs"/>
            {{$tl("edit")}}</a>
        </div>
        <div class="q-mr-md" v-if="canDelete">
          <a href="#" style="display: inline-flex; align-items: center;"
             @click.prevent="deleteMaterial">
            <q-icon name="fas fa-trash"/>
          </a>
        </div>
        <div class="mat-date-color">
          <q-icon name="far fa-clock"/>
          {{$formatDate(material.publishDate)}}
        </div>
      </div>


      <div class="clear"></div>
    </div>

    <div id="comments" v-if="material && comments" class="msgs">
      <hr class="hr-sep"/>
      <div v-for="(comment,index) in comments" :key="comment.id">
        <CommentContainer class="page-padding" :comment="comment" :checkLastOwn="checkLastOwn"
                          :categoryPersonalAccess="categoryPersonalAccess"
                          :isLast="index === maxCommentNumber"/>
        <hr class="hr-sep"/>
      </div>
      <div v-if="canCommentWrite">
        <CreateEditComment class="page-padding" @done="commentAdded" :materialId="material.id"/>
      </div>
    </div>

    <LoaderWait v-if="!material || !comments"/>
  </q-page>

</template>

<script>
  import CommentContainer from "comments/CommentContainer";
  import CreateEditComment from "comments/CreateEditComment";
  import {date} from 'quasar';
  import LoaderWait from "LoaderWait";
  import {scroll} from 'quasar';
  import Page from "Page";

  const {getScrollTarget, setScrollPosition} = scroll;

  export default {
    name: "Material",
    components: {CommentContainer, CreateEditComment, LoaderWait},
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
    data: function () {
      return {
        material: null,
        comments: null,
        page: null,
      }
    },
    watch: {
      'idOrName': 'loadData',
      'categoryName': 'loadData',
      '$store.state.auth.user': 'loadData'
    },
    computed: {
      maxCommentNumber() {
        return this.comments.length - 1;
      },
      category() {
        return this.$store.getters.getCategory(this.categoryName);
      },
      categoryPath() {
        return this.category.path;
      },
      canCommentWrite() {
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
          const publish = this.material.publishDate;
          const til = date.addToDate(publish, {minutes: config.Materials.TimeToOwnEditInMinutes});
          if (til < now) {
            return false;
          }
        }
        if (category.categoryPersonalAccess.materialEditOwn) {
          return true;
        }
        return false;
      },
      canDelete() {
        if (!this.material || !this.comments) {
          return false;
        }
        if (!this.$store.state.auth.user) {
          return false;
        }
        const category = this.$store.getters.getCategory(this.material.categoryName);

        if (category.categoryPersonalAccess.materialDeleteAny) {
          return true;
        }
        if (this.material.authorId !== this.$store.state.auth.user.id) {
          return false;
        }
        if (!category.categoryPersonalAccess.materialDeleteOwnIfHasReplies &&
          this.comments.length >= 1 && !this.checkLastOwn(this.comments[0])
        ) {
          return false;
        }
        if (!category.categoryPersonalAccess.materialDeleteOwnIfTimeNotExceeded) {
          const now = new Date();
          const publish = this.material.publishDate;
          const til = date.addToDate(publish, {minutes: config.Materials.TimeToOwnDeleteInMinutes});
          if (til < now) {
            return false;
          }
        }
        if (category.categoryPersonalAccess.materialDeleteOwn) {
          return true;
        }
        return false;
      },
    },
    methods: {
      async loadDataMaterial() {
        await this.$store.dispatch("request",
          {
            url: "/Materials/Get",
            data: {
              idOrName: this.idOrName
            }
          }).then(
          response => {
            this.material = response.data;
            this.title = this.material.title;
          }
        ).catch(x => {
          console.log("error", x);
        });
      },

      async loadDataComments() {
        await this.$store.dispatch("request",
          {
            url: "/Comments/GetMaterialComments",
            data:
              {
                materialId: this.material.id
              }
          }).then(
          response => {
            this.comments = response.data;
            this.$nextTick(function () {
              if (this.$route.hash) {
                let el = document.getElementById(this.$route.hash.substring(1))
                setScrollPosition(getScrollTarget(el), el.offsetTop, 300)
              }
            });
          }
        ).catch(x => {
          console.log("error", x);
        });
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
        this.$q.dialog({
          title: 'Удалить материал?',
          //message: '',
          ok: 'Да',
          cancel: 'Отмeна'
        }).then(async () => {
          await this.$store.dispatch("request",
            {
              url: "/Materials/Delete",
              data:
                {
                  id: this.material.id
                }
            }).then(
            () => {
              this.$q.notify({
                message: `Материал успешно удалён`,
                timeout: 2000,
                type: 'info',
                position: 'top'
              });
              this.$router.push(this.category.path);
            }).catch((x) => {
            console.log("error", x)
          });
        }).catch(() => {
        });
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

    async created() {
      await this.loadData();
    }
  }
</script>

<style scoped>
  .msgs {
    margin-top: 18px;
  }

  .mat-avatar {
    margin-right: 12px;
  }
</style>

<style lang="stylus">
  @import '~quasar-variables'

  .mat-date-color {
    color: $grey-7;
  }

  .hr-sep {
    height: 0;
    border-top: solid #d3eecc 1px !important;
    border-left: none;
  }
</style>
