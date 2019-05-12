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
  import {Post} from 'sun'

  export default {
    name: "PostsList",
    components: {Post, LoaderWait},
    data: function () {
      return {
        posts: {}
      }
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
