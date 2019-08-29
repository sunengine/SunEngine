<template>
  <q-page class="images-cleaner page-padding">
    <div class="header-with-button">
      <h2 class="q-title">{{title}}</h2>
      <q-btn icon="fas fa-trash" class="send-btn q-mr-lg" :disable="!images" @click="clear()" no-caps
             :label="$tl('clearBtn')"/>

      <q-btn no-caps class="q-ml-md" color="info" icon="fas fa-sync-alt" @click="reloadImages()"
             :label="$tl('refreshBtn')"/>
    </div>

    <q-banner rounded class="bg-amber-2 q-mb-md">
      {{$tl("info")}}
    </q-banner>

    <div v-if="images" class="img flex row q-col-gutter-sm">
      <img v-for="image in images" :src="$imagePath(image)" height="80" width="80" class="clean-img"/>
    </div>
  </q-page>
</template>

<script>
  import {Page} from 'sun';


  export default {
    name: 'ImagesCleaner',
    mixins: [Page],
    data() {
      return {
        imagesDeleted: null,
        images: null
      }
    },

    async created() {
      this.title = this.$tl('title');
      await this.loadImages();
    },
    methods: {

      async clear() {
        await this.$store
          .dispatch('request', {
            url: '/Admin/ImagesCleaner/DeleteImages',
            data: {}
          })
          .then(response => {
            this.imagesDeleted = response.data.imagesDeleted;
            this.$successNotify(this.$tl('clearCount') + this.imagesDeleted);
            this.loadImages();
          })
          .catch(error => {
            this.$errorNotify(error);
          });
      },

      async loadImages() {
        await this.$store
          .dispatch('request', {
            url: '/Admin/ImagesCleaner/GetAllImages',
            data: {}
          })
          .then(response => {
            if (response.data.length !== 0)
              this.images = response.data;
            else
              this.images = null;
          })
          .catch(error => {
            this.$errorNotify(error);
          });
      },
      async reloadImages() {
        await this.loadImages();
        this.images ? this.$successNotify() : this.$successNotify(this.$tl('emptyResult'));
      }
    }
  }

</script>
<style lang="stylus">

  .images-cleaner {
    .clean-img {
      object-fit: cover;
      width: 100px;
      height: 110px;
    }
  }

</style>
