<template>
  <div class="activity">
    <q-item :to='path'>
      <q-item-main :label="activity.title">
        <q-item-tile class="desc" sublabel>
          {{activity.description}}
        </q-item-tile>
        <q-item-tile class="info-block" sublabel>
          <template v-if="!activity.messageId">
            <span>
              <q-icon name="far fa-file-alt"/>
              Текст
              </span>
          </template>
          <template v-else>
            <span>
              <q-icon name="far fa-comment"/>
              Ответ
            </span>
          </template>
          <span>
            <q-icon name="far fa-user"/>
            {{activity.authorName}}
          </span>
          <span>
            <q-icon name="far fa-folder"/>
            {{category.title}}
          </span>
          <span>
            <q-icon name="far fa-clock"/>
            {{$formatDate(activity.publishDate)}}
          </span>
        </q-item-tile>
      </q-item-main>
    </q-item>
  </div>
</template>

<script>
  import SettingsPanel from "personal/SettingsPanel";

  export default {
    name: "Activity",
    components: {SettingsPanel},
    props: {
      activity: {
        type: Object,
        required: true
      }
    },
    computed: {
      path() {
        let path = this.category.path + "/" + this.activity.materialId;
        if (this.activity.messageId)
          path += "#message-" + this.activity.messageId;
        return path;
      },
      category() {
        return this.$store.getters.getCategory(this.activity.categoryName);
      }
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~variables'
  @import '~src/css/app'

  .desc {
    word-break: break-all;
  }


  .pull-right {
    .q-item {
      @extend .margin-back-right;
      @extend .pp-right;
    }
  }

  .pull-left {
    .q-item {
      @extend .margin-back-left;
      @extend .pp-left;
    }
  }
</style>
