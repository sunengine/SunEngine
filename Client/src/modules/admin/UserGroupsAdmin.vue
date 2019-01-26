<template>
  <q-page>
    <q-input
      v-model="json"
      type="textarea"
      float-label="Textarea"
      rows="7"/>
    <q-btn @click="send" label="Загрузить"/>
  </q-page>
</template>

<script>
  import Page from "Page";
  import LoaderWait from "LoaderWait";

  export default {
    name: "UserGroupsAdmin",
    components: {LoaderWait},
    mixins: [Page],
    computed: {},
    data: function () {
      return {
        json: ""
      }
    },
    methods: {
      async send() {
        await this.$store.dispatch("request",
          {
            url: "/GroupsAdmin/LoadJson",
            data: {
              json: this.json
            }
          })
          .then(
            async response => {
              this.$q.notify({
                message: 'Настройки групп успешно установлены',
                timeout: 4000,
                type: 'positive',
                position: 'top'
              });
            }
          ).catch(x => {
            console.log("error", x);
          });
      }
    },
  }
</script>

<style scoped>

</style>
