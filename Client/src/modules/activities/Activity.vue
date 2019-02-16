<template>
  <div>
    <q-item class="margin-back pp-right pp-left" :to='path'>
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
        let category = this.$store.getters.getCategory(this.activity.categoryName);
        let path = category.getPath() + "/" + this.activity.materialId;
        if (this.activity.messageId)
          path += "#message-" + this.activity.messageId;
        return path;
      }
    }
  }
</script>

<style scoped>
  .desc {
    word-break: break-all;
  }
</style>
