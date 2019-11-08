<template>
  <q-page class="configuration-admin">
    <h2 class="page-title">
      {{title}}
    </h2>

   <div v-if="configurationItems">
     <div v-for="cItem of configurationItems">
       Name: {{cItem.name}} - Value: {{cItem.value}} - Type: {{cItem.type}}
     </div>
   </div>
  </q-page>
</template>

<script>
    export default {
        name: "ConfigurationAdmin",
        data() {
            return {
                configurationItems: null
            }
        },
        methods: {
            loadConfiguration() {
              this.$request(this.$AdminApi.ConfigurationAdmin.LoadConfiguration)
                  .then(response => {
                      this.configurationItems = response.data;
                      }
                  );
            }
        },
        created() {
            this.loadConfiguration();
        }
    }
</script>

<style scoped>

</style>
