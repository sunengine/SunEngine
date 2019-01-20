<template>
  <div>
    <q-item :to="path" class="header">
      <img class="avatar" :src="$imagePath(post.authorAvatar)"/>
      <div>
        <div style="font-weight: 600">
          {{post.title}}
        </div>
        <div class="q-my-xs info-block">
          <router-link :to="`/user/${post.authorLink}`" class="user-link q-mr-xl">
            {{post.authorName}}
          </router-link>
          <span class="text-grey-6">
            <q-icon name="far fa-clock"/>
            {{$formatDate(this.post.publishDate)}} &nbsp;
          </span>
        </div>
      </div>
    </q-item>
    <div class="q-my-xs post-preview" v-html="post.preview">

    </div>
    <div class="flex footer">
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
    <hr/>
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


<style scoped lang="stylus">
  @import '~variables'

  .avatar {
    margin-right: 12px;
    width: 44px;
    height: 44px;
    border-radius: 22px;
  }

  .header {
    display: flex;
    margin-bottom: 12px;
    margin-left: -16px;
    padding-left: 16px !important;
    color: #3a67d3 !important;
  }

  .footer {
    margin-left: -16px;

    .q-item {
      color: #3a67d3 !important;
      margin-left: 2px;
    }
  }

  hr {
    height: 0px;
    margin: 18px 0 - $flex-gutter-sm;
    border-top: solid rgba(42, 171, 210, 0.07) 1px;
    border-left: none;
  }

  .post-preview {
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
</style>
