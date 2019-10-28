<template>
  <q-page class="page-padding">
    <h2 class="page-title">
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
                this.$request(this.$AdminApi.CypherSecretsAdmin.ResetCypher,
                    {
                        name: 'ShortJwt'
                    }
                ).then(_ => this.$successNotify()
                );
            },
            long2Jwt() {
                this.$request(this.$AdminApi.CypherSecretsAdmin.ResetCypher,
                    {
                        name: 'Long2Jwt'
                    }
                ).then(_ => this.$successNotify())
            },
            emailChange() {
                this.$request(this.$AdminApi.CypherSecretsAdmin.ResetCypher,
                    {
                        name: 'EmailChange'
                    }
                ).then(_ => this.$successNotify())
            }
        },
        created() {
            this.title = this.$tl('title');
        }
    }

</script>

<style lang="stylus">

</style>
