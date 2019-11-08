<template>
  <q-page class="configuration-admin page-padding">
    <h2 class="page-title">
      {{title}}
    </h2>

    <table v-if="configurationItems">
      <tr v-for="cItem of configurationItems">
        <td>{{cItem.name}}</td>
        <td>
          <q-input dense class="inline-block full-width" v-if="cItem.type === 'Number' ||cItem.type === 'String' "
                   :type="cItem.type === 'Number' ? 'number' : 'text'" v-model="cItem.value"/>
          <q-checkbox v-else class="inline-block" v-model="cItem.value"/>
        </td>
      </tr>

      <div class="configuration-admin__btn-block flex q-mt-lg q-gutter-md">
        <q-btn class="send-btn" @click="uploadConfiguration" no-caps icon="fas fa-save" :label="$tl('saveBtn')"/>
        <div class="grow"></div>
        <q-btn class="reset-btn" @click="loadConfiguration" no-caps icon="fas fa-sync-alt" :label="$tl('resetBtn')"/>
      </div>
    </table>
    <LoaderWait v-else/>

  </q-page>
</template>

<script>
    import {Page} from 'mixins';


    export default {
        name: "ConfigurationAdmin",
        mixins: [Page],
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
            },
            uploadConfiguration() {
                this.$request(this.$AdminApi.ConfigurationAdmin.UploadConfiguration,
                    {}
                ).then(response => {
                        this.configurationItems = response.data;
                    }
                );
            }
        },
        beforeCreate() {
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        },
        created() {
            this.title = this.$tl("title");
            this.loadConfiguration();
        }
    }
</script>

<style scoped>

</style>
