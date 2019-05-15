<template>
  <div>
    <q-item :to="to" class="header page-padding">
      <img class="avatar" :src="$imagePath(post.authorAvatar)"/>
      <div>
        <div class="blog-title my-header">
          {{post.title}}
        </div>
        <div>
          <router-link :to="{name: 'User', params: {link: post.authorLink}}" class="user-link">
            {{post.authorName}}
          </router-link>
        </div>
      </div>
    </q-item>

    <div class="post-preview page-padding" v-html="post.preview"></div>

    <div class="date text-grey-6">
      <q-icon name="far fa-clock"/>
      <span>{{$formatDate(this.post.publishDate)}} &nbsp;</span>
    </div>

    <div class="flex footer float-left ">
      <q-item class="page-padding-left" :to="toComments">
        <span :class="[{'text-grey-6': !post.commentsCount}]">
        <q-icon name="far fa-comment" class="q-mr-sm"/>
        {{post.commentsCount}} {{$tl('commentsCount')}}
        </span>
      </q-item>
      <q-item :to="to" v-if="post.hasMoreText">
        <span>
          {{$tl('readMore')}}
          <q-icon name="fas fa-arrow-right"/>
        </span>
      </q-item>

    </div>
    <div class="clear"></div>
  </div>
</template>

<script>

  export default {
    name: "Post",
    props: {
      post: {
        type: Object,
        required: true
      }
    },
    computed: {
      to() {
        return this.category.getMaterialRoute(this.post.id);
      },
      toComments() {
        return this.category.getMaterialRoute(this.post.id, '#comments');
      },
      category() {
        return this.$store.getters.getCategory(this.post.categoryName);
      }
    }
  }
</script>


<style lang="stylus" scoped>
  @import '~quasar-variables'
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

  .blog-title {
    font-weight: 600 !important;
  }

  $footer-line-height = 38px;

  .footer {
    align-items: center;

    .q-item {
      color: #3a67d3 !important;
      min-height: unset !important;
      height: $footer-line-height;

    }

    .q-item:first-child {
      padding-left: 0;
    }

    /*.q-item, div {

      padding-top: unset !important;
      padding-bottom: unset !important;
      height: $footer-line-height;
    }*/
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
