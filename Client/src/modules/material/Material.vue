<template>
  <q-page>
    <template v-if="material">
      <h2 class="q-title">
        {{material.title}}
      </h2>
      <div v-if="category" style="margin-top: -10px;" class="q-mb-md">
        <span class="text-grey-7">раздел: </span>
        <router-link :to="categoryPath">{{category.title}}</router-link>
      </div>
      <div v-html="material.text">
      </div>
      <div class="q-mt-lg" style="text-align: center">
        <q-chip class="q-mx-xs" small tag color="info" v-for="tag in material.tags" :key="tag">
          {{tag}}
        </q-chip>
      </div>
      <div class="q-py-sm text-grey-8 flex" style="align-items: center">
        <div class="q-mr-md">
          <router-link :to="'/user/'+material.authorLink">
            <img class="avatar mat-avatar" :src="$imagePath(material.authorAvatar)"/>{{material.authorName}}
          </router-link>
        </div>
        <div style="flex-grow: 1">

        </div>
        <div class="q-mr-md" v-if="canEdit">
          <a href="#" style="display: inline-flex; align-items: center;"
             @click.prevent="$router.push(`/AddEditMaterial?id=`+material.id)">
            <q-icon name="fas fa-edit" class="q-mr-xs"/>
            Редактировать</a>
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


      <div style="clear: both"></div>
    </template>

    <div id="messages" v-if="messages" class="msgs">
      <hr class="hr-sep margin-back"/>
      <div v-for="(message,index) in messages" :key="message.id">
        <MessageContainer :message="message" :checkLastOwn="checkLastOwn"
                          :categoryPersonalAccess="categoryPersonalAccess"
                          :isLast="index == maxMessageNumber" />
        <hr class="hr-sep margin-back"/>
      </div>
      <div v-if="canMessageWrite">
        <AddEditMessage @done="messageAdded" :materialId="id"/>
      </div>
    </div>

    <LoaderWait v-if="!material || !messages"/>
  </q-page>

</template>

<script>
  import MessageContainer from "message/MessageContainer";
  import AddEditMessage from "message/AddEditMessage";
  import {date} from 'quasar';
  import LoaderWait from "LoaderWait";
  import {scroll} from 'quasar';
  import Page from "../../components/Page";

  const {getScrollTarget, setScrollPosition} = scroll;

  export default {
    name: "Material",
    components: {MessageContainer, AddEditMessage, LoaderWait},
    mixins: [Page],
    props: {
      id: {
        type: Number,
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
        messages: null,
        page: null,
      }
    },
    watch: {
      'id': 'loadData',
      'categoryName': 'loadData',
      '$store.state.auth.user': 'loadData'
    },
    computed: {
      maxMessageNumber() {
        return this.messages.length - 1;
      },
      category() {
        return this.$store.getters.getCategory(this.categoryName);
      },
      categoryPath() {
        return this.category.getPath();
      },
      canMessageWrite() {
        return this.category.categoryPersonalAccess.MessageWrite;
      },
      categoryPersonalAccess() {
        return this.category.categoryPersonalAccess;
      },
      canEdit() {
        if (!this.material || !this.messages) {
          return false;
        }
        if (!this.$store.state.auth.user) {
          return false;
        }
        const category = this.$store.getters.getCategory(this.material.categoryName);

        if (category.categoryPersonalAccess.MaterialEditAny) {
          return true;
        }
        if (this.material.authorId != this.$store.state.auth.user.id) {
          return false;
        }
        if (!category.categoryPersonalAccess.MaterialEditOwnIfHasReplies &&
          this.messages.length >= 1 && !this.checkLastOwn(this.messages[0])
        ) {
          return false;
        }
        if (!category.categoryPersonalAccess.MaterialEditOwnIfTimeNotExceeded) {
          const now = new Date();
          const publish = this.material.publishDate;
          const til = date.addToDate(publish, {minutes: config.Materials.MaterialsTimeToOwnEditInMinutes});
          if (til < now) {
            return false;
          }
        }
        if (category.categoryPersonalAccess.MaterialEditOwn) {
          return true;
        }
        return false;
      },
      canDelete() {
        if (!this.material || !this.messages) {
          return false;
        }
        if (!this.$store.state.auth.user) {
          return false;
        }
        const category = this.$store.getters.getCategory(this.material.categoryName);

        if (category.categoryPersonalAccess.MaterialDeleteAny) {
          return true;
        }
        if (this.material.authorId != this.$store.state.auth.user.id) {
          return false;
        }
        if (!category.categoryPersonalAccess.MaterialDeleteOwnIfHasReplies &&
          this.messages.length >= 1 && !this.checkLastOwn(this.messages[0])
        ) {
          return false;
        }
        if (!category.categoryPersonalAccess.MaterialDeleteOwnIfTimeNotExceeded) {
          const now = new Date();
          const publish = this.material.publishDate;
          const til = date.addToDate(publish, {minutes:  config.Materials.MaterialsTimeToOwnDeleteInMinutes});
          if (til < now) {
            return false;
          }
        }
        if (category.categoryPersonalAccess.MaterialDeleteOwn) {
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
              id: this.id
            }
          }).then(
          response => {
            this.material = response.data;
            this.setTitle(this.material.title);
          }
        ).catch(x => {
          console.log("error", x);
        });
      },

      async loadDataMessages() {
        await this.$store.dispatch("request",
          {
            url: "/Messages/GetMaterialMessages",
            data:
              {
                materialId: this.id
              }
          }).then(
          response => {
            this.messages = response.data;
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

      checkLastOwn(message) {
        if (!this.messages) {
          return false;
        }
        let userId = this.$store.state.auth.user.id;
        let ind = this.messages.indexOf(message);
        for (let i = ind; i < this.messages.length; i++) {
          if (this.messages[i].authorId != userId) {
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
                  id: this.id
                }
            }).then(
            () => {
              this.$q.notify({
                message: `Материал успешно удалён`,
                timeout: 2000,
                type: 'info',
                position: 'top'
              });
              this.$router.push(this.category.getPath(true));
            }).catch((x) => {
            console.log("error", x)
          });
        }).catch(() => {
        });
      },

      async messageAdded() {
        let currentPath = this.$route.fullPath;
        let ind = currentPath.lastIndexOf("#");
        let path = currentPath.substring(0, ind);
        window.history.pushState("", document.title, path);
        await this.loadData();
      },

      async loadData() {
        await this.loadDataMaterial();
        await this.loadDataMessages();
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
    margin-bottom: 25px;
  }

  .mat-avatar {
    margin-right: 12px;
  }
</style>

<style lang="stylus">
  @import '~variables';

  .mat-date-color {
    color: $grey-7;
  }

  .hr-sep {
    height: 0;
    border-top: solid #d3eecc 1px !important;
    border-left: none;
  }
</style>
