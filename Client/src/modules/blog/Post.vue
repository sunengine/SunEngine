<template>
  <div :class="['post', {'mat-hidden': post.isHidden}, {'mat-deleted': post.deletedDate}]">
    <q-item :to="to" class="header page-padding">
      <q-avatar class="shadow-1 avatar" size="40px">
        <img :src="$imagePath(post.authorAvatar)"/>
      </q-avatar>
      <div>
        <div class="blog-title my-header link">
          <q-icon name="fas fa-trash" color="maroon" class="q-mr-sm" v-if="post.deletedDate"/>
          <q-icon name="far fa-eye-slash" v-else-if="post.isHidden" class="q-mr-sm"/>
          {{post.title}}
          <span class="q-ml-sm" v-if="post.deletedDate">
            [{{$tl("deleted")}}]
          </span>
          <span class="q-ml-sm" v-else-if="post.isHidden">
            [{{$tl("hidden")}}]
          </span>
        </div>
        <div>
          <router-link :to="{name: 'User', params: {link: post.authorLink}}" class="user-link">
            {{post.authorName}}
          </router-link>
        </div>
      </div>
    </q-item>

    <div v-if="!post.isHidden && !post.deletedDate" class="post-text page-padding"
         v-html="post.preview"></div>

    <div class="date text-grey-6">
      <q-icon name="far fa-clock"/>
      <span>{{$formatDate(this.post.publishDate)}} &nbsp;</span>
    </div>

    <div class="flex footer float-left">
      <q-item class="page-padding-left comments link" :to="toComments">
        <span :class="[{'text-grey-6': !post.commentsCount}]">
        <q-icon name="far fa-comment" class="q-mr-sm"/>
        {{post.commentsCount}} {{$tl('commentsCount')}}
        </span>
      </q-item>
      <q-item class="link" :to="to" v-if="post.hasMoreText">
        <span>
          {{$tl('readMore')}}
          <q-icon name="fas fa-arrow-right" class="q-ml-xs"/>
        </span>
      </q-item>

    </div>
    <div class="clear"></div>
  </div>
</template>

<script>
  import {prepareLocalLinks} from 'sun'


  export default {
    name: 'Post',
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
    },
    methods: {
      prepareLocalLinks() {
        prepareLocalLinks(this.$el, 'post-text');
      }
    },
    mounted() {
      this.prepareLocalLinks();
    }
  }

</script>


<style lang="stylus">

  .post {
    .avatar {
      margin-right: 12px;
    }

    .header {
      display: flex;
      padding: 2px 0;
    }

    .blog-title {
      font-weight: 600 !important;
      color: $link-color !important;
    }

    $footer-line-height = 38px;

    .footer {
      align-items: center;
      color: $link-color !important;

      .q-item {
        min-height: unset !important;
        height: $footer-line-height;
      }

      .q-item:first-child {
        padding-left: 0;
      }

      .comments {
        .q-icon {
          &:before {
            padding-bottom: 2px !important;
          }
        }
      }
    }

    .post-text {

      margin: 8px 0;

      *:first-child {
        margin-top: 0 !important;
      }

      *:last-child {
        margin-bottom: 0 !important;
      }
    }

    .user-link {
      color: $grey-6;

      &:hover {
        color: $link-color !important;
        text-decoration: underline;
      }
    }

    .date {
      display: flex;
      float: right;
      align-items: center;
      height: $footer-line-height;

      .q-icon {
        margin-right: 8px;

        &:before {
          padding-top: 1px !important;
        }
      }
    }
  }

</style>
