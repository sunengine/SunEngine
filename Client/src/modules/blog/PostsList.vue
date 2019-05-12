<template>
  <div>
    <LoaderWait v-if="!posts.items"/>

    <template v-else>
      <div>
        <div v-for="post in posts.items" :key="post.id">
          <Post :post="post"/>
          <hr class="hr-sep"/>
        </div>
      </div>


    </template>
  </div>
</template>

<script>
  import {LoaderWait} from 'sun'

  export default {
    name: "PostsList",
    components: { LoaderWait},
    data: function () {
      return {
        posts: {}
      }
    },
    beforeCreate() {
      this.$options.components.Post = require('sun.js').Post;
    }
  }
</script>

<style lang="stylus" scoped>

  .hr-sep {
    height: 0;
    border-top: solid #d3eecc 1px !important;
    border-left: none;
  }

</style>
