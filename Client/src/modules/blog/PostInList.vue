<template>
  <div>
    <q-item :to="path" class="header">
      <img class="avatar" :src="$imagePath(post.authorAvatar)"/>
      <div>
        <div style="font-weight: 600">
          {{post.title}}
        </div>
        <div class="q-my-xs">
          <router-link :to="`/user/${post.authorLink}`" class="user-link q-mr-xl">
            {{post.authorName}}
          </router-link>

        </div>
      </div>
    </q-item>
    <div class="post-preview" v-html="post.preview">

    </div>
    <div class="date text-grey-6">
      <q-icon name="far fa-clock"/>
      <span>{{$formatDate(this.post.publishDate)}} &nbsp;</span>
    </div>
    <div class="flex footer float-left">
      <q-item :to="path+'#messages'" class="q-mr-md">
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
      post: Object,
      startPath: {
        type: String,
        required: true
      }
    },
    computed: {
      path() {
        return this.$buildPath(this.startPath, this.post.categoryName, this.post.id);
      }
    }
  }
</script>


<style lang="stylus" scoped>
  @import '~variables'

  .avatar {
    margin-right: 12px;
    width: 44px;
    height: 44px;
    border-radius: 22px;
  }

  .header {
    display: flex;
    padding: 2px 0;
    margin-left: -16px;
    padding-left: 16px !important;
    color: #3a67d3 !important;
  }

  $footer-line-height = 38px;

  .footer {
    margin-left: -16px;
    align-items: center;

    .q-item {
      color: #3a67d3 !important;
      min-height: unset !important;
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

</style>
