<template>
  <q-page class="page-padding">
    <div class="header-with-button">
      <h2 class="q-title">{{title}}</h2>
      <q-btn icon="fas fa-trash" color="send" class="q-mr-lg" @click="clear()" no-caps
        :label="$tl('clearBtn')"/>
      <q-btn no-caps class="q-ml-md" color="info" icon="fas fa-sync-alt" @click="loadImages()" :label="$tl('refreshBtn')"/>
    </div>    
    <div v-if="json" class="img flex row">
      <img v-for="image in images" :src="$imagePath(image)" height="80" width="80" class="cleanImg"/>
    </div>    
  </q-page>
</template>

<script>
import { Page } from "sun";

export default {
  name: "ImagesCleaner",
  mixins: [Page],
  data() {
    return {
      result : null,
      json: null,
      error: null,
      images:[]      
    }
  },

  async created() {
    this.title = this.$tl("title");
    await this.loadImages();
  },
  methods: {

    async clear() {
      await this.$store
        .dispatch("request", {
          url: "/Admin/ImagesCleaner/DeleteImages",
          data: {}
        })
        .then(response => {          
          this.result = response.data.json;
          this.$successNotify(this.$tl("clearCount") + this.result);
          this.loadImages();
        })
        .catch(error => {
          this.$errorNotify(error);
        });
    },

    async loadImages() {
      await this.$store
        .dispatch("request", {
          url: "/Admin/ImagesCleaner/GetAllImages",
          data: {}
        })
        .then(response => {   
          this.error = null;       
          this.json = response.data.json;            
          this.images = this.json.split('\n');                 
        })
        .catch(error => {
          this.$errorNotify(error);
        });
    }   
  }
};
</script>
<style lang="stylus" scoped>
  .cleanImg{
    padding-right : 5px;    
  }
</style>
