<template>
  <q-page class="images-cleaner page-padding">

    <h2 class="page-title">{{title}}</h2>

    <q-banner rounded class="bg-amber-2 q-mb-md">
      {{$tl("info")}}
    </q-banner>

    <div class="images-cleaner__btn-block q-gutter-md q-mb-xl">
      <q-btn icon="fas fa-trash" class="send-btn" :disable="!images" @click="clear()" no-caps
             :label="$tl('clearBtn')"/>

      <q-btn no-caps class="refresh-btn q-ml" color="info" icon="fas fa-sync-alt" @click="reloadImages()"
             :label="$tl('refreshBtn')"/>
    </div>

    <div v-if="images" class="images-cleaner__img-block img flex row q-col-gutter-sm">
      <img v-for="image in images" :src="$imagePath(image)" height="80" width="80" class="images-cleaner__clean-img"/>
    </div>
  </q-page>
</template>

<script>
    import {Page} from 'mixins';


    export default {
        name: 'ImagesCleaner',
        mixins: [Page],
        data() {
            return {
                imagesDeleted: null,
                images: null
            }
        },
        methods: {
            clear() {
                this.$request(this.$AdminApi.ImagesCleaner.DeleteImages
                ).then(response => {
                    this.imagesDeleted = response.data.imagesDeleted;
                    this.$successNotify(this.$tl('clearCount') + this.imagesDeleted);
                    this.loadImages();
                }).catch(error => {
                    this.$errorNotify(error)
                });
            },

            loadImages() {
                this.$request(this.$AdminApi.ImagesCleaner.GetAllImages
                ).then(response => {
                    if (response.data.length !== 0)
                        this.images = response.data;
                    else
                        this.images = null;
                }).catch(error => {
                    this.$errorNotify(error)
                });
            },
            async reloadImages() {
                await this.loadImages();
                this.images ? this.$successNotify() : this.$successNotify(this.$tl('emptyResult'));
            }
        },
        created() {
            this.title = this.$tl('title');
            this.loadImages();
        },
    }

</script>
<style lang="scss">

  .images-cleaner__clean-img {
    object-fit: cover;
    width: 100px;
    height: 110px;
  }

</style>
