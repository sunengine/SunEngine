<template>
  <q-page class="page-padding">
    <h2 class="q-title">{{$tl("title")}}</h2>
    <div v-if="users">
      <router-link :key="user.id" class="block q-mb-xs" style="font-weight: 600"
                   :to="{name:'User', params: {link: user.link}}" v-for="user in users">{{user.name}}
      </router-link>
    </div>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
  import {Page} from 'sun'


  export default {
    name: "MyBanList",
    mixins: [Page],
    data() {
      return {
        users: null
      }
    },
    methods: {
      async loadData() {
        await this.$store.dispatch("request",
          {
            url: "/Personal/GetMyBanList",
          })
          .then(
            response => {
              this.users = response.data;
            }
          )
      }
    },
    beforeCreate() {
      this.$options.components.LoaderWait = require('sun').LoaderWait;
    },
    async created() {
      this.title = this.$tl("title");
      await this.loadData();
    }
  }
</script>

<style scoped>

</style>
