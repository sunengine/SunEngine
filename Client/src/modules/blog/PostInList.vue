<template>
  <div>
    <div class="header">
      <img class="avatar" :src="$imagePath(post.authorAvatar)"/>
      <div>
        <div class="q-mb-xs ttl" style="font-weight: 600">
          <router-link :to="path">{{post.title}}</router-link>
        </div>
        <div class="q-my-xs info-block">
          <router-link :to="`/user/${post.authorLink}`" class="text-grey-6 q-mr-xl">
            {{post.authorName}}
          </router-link>
          <span class="text-grey-6">
            <q-icon name="far fa-clock"/>
            {{$formatDate(this.post.publishDate)}} &nbsp;
          </span>
        </div>
      </div>
    </div>
    <div class="q-my-xs" v-html="post.preview">

    </div>
    <div class="q-mb-xs footer">
      <router-link :to="path" :class="['q-mr-xl','q-ml-sm', {'text-grey-6': !post.messagesCount}]">
        <q-icon name="far fa-comment" class="q-mr-sm"/>
        {{post.messagesCount}} сообщений
      </router-link>

      <router-link :to="path" v-if="post.hasMoreText">Читать дальше
        <q-icon name="fas fa-arrow-right"/>
      </router-link>
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

  .ttl {
    //font-weight: 600;
  }

  .header {
    display: flex;
    margin-bottom: 12px;
  }

  .footer {
    margin-top: 12px;
  }

  hr {
    height: 0px;
    margin: 18px 0 - $flex-gutter-sm;
    border-top: solid rgba(42, 171, 210, 0.07) 1px;
    border-left: none;
  }
</style>
