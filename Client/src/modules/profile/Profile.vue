<template>
  <div class="q-pa-md">
    <div class="f1" v-if="user">
      <div class="img">
        <img style="" width="300" height="300" :src="$imagePath(user.photo)"/>

      </div>
      <div>
        <h4>{{user.name}}</h4>
        <div v-html="user.information"></div>
      </div>
    </div>
    <loader-wait v-else/>
  </div>
</template>

<script>
  import LoaderWait from "LoaderWait";
  import Page from "Page";

  export default {
    name: "Profile",
    mixins: [Page],
    components: {LoaderWait},
    props: {
      link: {
        type: String,
        required: true
      }
    },
    data: function () {
      return {
        user: null
      }
    },
    watch: {
      'link': 'loadData'
    },
    methods: {
      async loadData() {
        await this.$store.dispatch("request",
          {
            url: "/Profile/GetProfile",
            data: {
              link: this.link
            }
          }).then(response => {
          this.user = response.data;
          this.setTitle(this.user.name);
        }).catch(error => {
          console.log("error", error);
        });
      }
    },
    async created() {
      await this.loadData();
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~variables';

  .f1 {
    display: flex;
    flex-wrap: wrap;

    .img img {
      margin-right: 15px;
    }
  }

  @media (max-width: 600px) {
    .f1 .img {
      width: 100%;
      text-align: center;
    }
  }
</style>
