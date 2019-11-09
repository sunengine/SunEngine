<template>
  <q-page class="configuration-admin">
    <h2 class="page-title  page-padding">
      {{title}}
    </h2>

    <div v-if="configurationItems">
      <q-table hide-header hide-bottom :pagination="{rowsPerPage :0 }"
               :data="configurationItems"
               :columns="columns"
               row-key="name"
      >
        <template v-slot:body="props">
          <q-tr :props="props">
            <q-td  class="configuration-admin__name-column">{{props.row.name }}</q-td>
            <q-td >
              <q-checkbox v-if="props.row.type === 'Boolean'" v-model="props.row.value"/>
              <q-input dense v-else :type="getTypeType(props.row.type)" v-model="props.row.value"/>
            </q-td>
          </q-tr>
        </template>
      </q-table>

      <div class="configuration-admin__btn-block  page-padding flex q-mt-lg q-gutter-md">
        <q-btn class="send-btn" @click="uploadConfiguration" no-caps icon="fas fa-save" :label="$tl('saveBtn')"/>
        <div class="grow"></div>
        <q-btn class="reset-btn" @click="resetConfiguration" no-caps icon="fas fa-sync-alt" :label="$tl('resetBtn')"/>
      </div>
    </div>
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
        computed: {
            columns() {
                return [
                    {name: 'name', label: 'Name', field: 'name', classes: 'configuration-admin__name-column'},
                    {name: 'value', label: 'Value', field: 'value'}
                ]
            }
        },
        methods: {
            getTypeType(type) {
                switch (type) {
                    case 'String':
                        return 'text';
                    case 'LongString':
                        return 'textarea';
                    case 'Number':
                        return 'number';
                }
            },
            resetConfiguration() {
                this.loadConfiguration()
                    .then(_ => {
                        this.$successNotify(this.$tl('resetSuccessNotify'));
                    });
            },
            loadConfiguration() {
               return this.$request(this.$AdminApi.ConfigurationAdmin.LoadConfiguration)
                    .then(response => {
                            this.configurationItems = response.data;
                        }
                    );
            },
            uploadConfiguration() {
                const data = new FormData();

                for (const ci of this.configurationItems)
                    data.append(ci.name, ci.value);

                this.$request(this.$AdminApi.ConfigurationAdmin.UploadConfiguration, data)
                    .then(_ => {
                            this.$successNotify();
                            this.loadConfiguration();
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

<style lang="scss">

  .configuration-admin__name-column {
    width: 150px !important;
  }

</style>
