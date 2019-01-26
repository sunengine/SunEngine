<template>
  <q-page>
    <h2 class="q-title">Забаненые пользователи</h2>
    <div v-if="users">
      <router-link :key="user.id" class="block q-mb-xs" style="font-weight: 600" :to="{name:'User', params: {link: user.link}}" v-for="user in users" >{{user.name}}</router-link>
    </div>
    <LoaderWait v-else />
  </q-page>
</template>

<script>
  import Page from "Page";
  import LoaderWait from "LoaderWait";

  export default {
    name: "MyBanList",
    components: {LoaderWait},
    mixins: [Page],
    data: function() {
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
    async created() {
      this.setTitle("Список забаненых пользователей");
      await this.loadData();
    }
  }
</script>

<style scoped>

</style>
