<template>
  <q-page class="page-padding">
    <h2 class="q-title">
      {{title}}
    </h2>
    <div class=" q-gutter-md">
      <q-btn no-caps icon="fas fa-key" @click="shortJwt" color="warning" label="ShortJwt"/>
      <q-btn no-caps icon="fas fa-key" @click="long2Jwt" color="warning" label="Long2Jwt"/>
      <q-btn no-caps icon="fas fa-key" @click="emailChange" color="warning" label="EmailChange"/>
    </div>
  </q-page>
</template>

<script>

  export default {
    name: "CypherSecrets",
    methods: {
      shortJwt() {
        this.$store
          .dispatch('request', {
            url: '/Admin/CypherSecretsAdmin/ResetCypher',
            data: {
              name: 'ShortJwt'
            }
          })
          .then(() => {
            this.$successNotify();
          });
      },
      long2Jwt() {
        this.$store
          .dispatch('request', {
            url: '/Admin/CypherSecretsAdmin/ResetCypher',
            data: {
              name: 'Long2Jwt'
            }
          })
          .then(() => {
            this.$successNotify();
          });
      },
      emailChange() {
        this.$store
          .dispatch('request', {
            url: '/Admin/CypherSecretsAdmin/ResetCypher',
            data: {
              name: 'EmailChange'
            }
          })
          .then(() => {
            this.$successNotify();
          });
      }
    },
    created() {
      this.title = this.$tl('title');
    }
  }

</script>

<style lang="stylus">

</style>
