<template>
  <div>
    <q-item :to="path" class="header">
      <img class="avatar" :src="$imagePath(post.authorAvatar)"/>
      <div>
        <div style="font-weight: 600">
          {{post.title}}
        </div>
        <div>
          <router-link :to="`/user/${post.authorLink}`" class="user-link">
            {{post.authorName}}
          </router-link>
        </div>
      </div>
    </q-item>

    <div class="post-preview" v-html="post.preview"></div>

    <div class="date text-grey-6">
      <q-icon name="far fa-clock"/>
      <span>{{$formatDate(this.post.publishDate)}} &nbsp;</span>
    </div>

    <div class="flex footer float-left">
      <q-item :to="path+'#messages'">
        <span :class="[{'text-grey-6': !post.messagesCount}]">
        <q-icon name="far fa-comment" class="q-mr-sm"/>
        {{post.messagesCount}} сообщений
        </span>
      </q-item>
      <q-item :to="path" v-if="post.hasMoreText">
        <span>
          Читать дальше
          <q-icon name="fas fa-arrow-right"/>
        </span>
      </q-item>

    </div>
    <div class="clear"></div>
  </div>
</template>

<script>
  import {date} from 'quasar'

  export default {
    name: "PostInList",
    props: {
      post: {
        type: Object,
        required: true
      }
    },
    computed: {
      path() {
        return this.$buildPath(this.category.path, this.post.id);
      },
      category() {
        return this.$store.getters.getCategory(this.post.categoryName);
      }
    }
  }
</script>


<style lang="stylus" scoped>
  @import '~variables'
  @import '~src/css/app'

  .avatar {
    margin-right: 12px;
    width: 44px;
    height: 44px;
    border-radius: 22px;
  }

  .header {
    display: flex;
    padding: 2px 0;
    //margin-left: -16px;
    //padding-left: 16px !important;
    color: #3a67d3 !important;
  }

  $footer-line-height = 38px;

  .footer {
    align-items: center;

    .q-item {
      color: #3a67d3 !important;
      min-height: unset !important;
    }

    .q-item:first-child {
      padding-left: 0;
    }

    .q-item, div {
      margin-left: 2px;
      padding-top: unset !important;
      padding-bottom: unset !important;
      height: $footer-line-height;
    }
  }

  .post-preview {

    margin: 3px 0;

    >>> *:first-child {
      margin-top: 0 !important;
    }

    >>> *:last-child {
      margin-bottom: 0 !important;
    }
  }

  .user-link {
    color: $grey-6;

    &:hover {
      color: #3a67d3 !important;
      text-decoration: underline;
    }
  }

  .date {
    display: flex;
    float: right;
    align-items: center;
    height: $footer-line-height;

    .q-icon {
      margin-right: 7px;
    }
  }

  .pull-right {
    .header {
      @extend .margin-back-right;
      @extend .pp-right;
    }
  }

  .pull-left {
    .header {
      @extend .margin-back-left;
      @extend .pp-left;
    }

    .footer {
      .q-item:first-child {
        margin-left: -12px;
        padding-left: 12px;
        //@extend .margin-back-left;
        //@extend .pp-left;
      }
    }
  }

</style>
