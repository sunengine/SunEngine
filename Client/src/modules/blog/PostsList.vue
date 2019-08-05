<template>
  <div class="posts-list">
    <LoaderWait v-if="!posts.items"/>

    <template v-else>
      <div v-for="post in posts.items" :key="post.id">
        <Post :post="post"/>
        <hr class="hr-sep"/>
      </div>
    </template>
  </div>
</template>

<script>

  export default {
    name: 'PostsList',
    data() {
      return {
        posts: {}
      }
    },
    beforeCreate() {
      this.$options.components.Post = require('sun').Post;
      this.$options.components.LoaderWait = require('sun').LoaderWait;
    }
  }

</script>

<style lang="stylus">

  .posts-list {
    .hr-sep {
      height: 0;
      border-top: dashed #d3eecc 1px !important;
      border-left: none;
    }
  }

</style>
