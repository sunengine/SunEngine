<template>
  <q-page class="configuration-admin page-padding">
    <h2 class="page-title ">
      {{title}}
    </h2>

    <div v-if="configurationGroups">
      <table class="configuration-admin__table">
        <template v-for="group of configurationGroups">
          <tr class="configuration-admin__group-header-tr">
            <td colspan="2" class="configuration-admin__group-header-td">
              <div class="configuration-admin__group-header-div">
                {{group.name}}
              </div>
            </td>
          </tr>
          <tr v-for="item of group.items">
            <td class="configuration-admin__name-column">{{item.name}}</td>
            <td class="configuration-admin__value-column">
              <q-checkbox v-if="item.item.type === 'Boolean'" v-model="item.item.value"/>
              <q-select v-else-if="item.item.type === 'Enum'" :options="enums[item.item.enumName]" v-model="item.item.value" />
              <q-input dense v-else :type="getTypeType(item.item.type)" v-model="item.item.value"/>
            </td>
          </tr>
          <div class="q-pb-lg"></div>
        </template>
      </table>

      <div class="configuration-admin__btn-block flex q-mt-lg q-gutter-md">
        <q-btn class="send-btn" @click="uploadConfiguration" no-caps icon="fas fa-save" :loading="loading"
               :label="$tl('saveBtn')">
          <template v-slot:loading>
            <LoaderSent/>
          </template>
        </q-btn>
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
                configurationGroups: null,
                configurationItems: null,
                enums: null,
                loading: false
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
            getEnum(name) {
              return this.enums[name];
            },
            getEnums() {
                return this.$request(this.$AdminApi.ConfigurationAdmin.GetEnums)
                    .then(response => {
                            this.enums = response.data;
                        }
                    );
            },
            resetConfiguration() {
                return this.loadConfiguration()
                    .then(_ => {
                        this.$successNotify(this.$tl('resetSuccessNotify'), "info");
                    });
            },
            loadConfiguration() {
                return this.$request(this.$AdminApi.ConfigurationAdmin.LoadConfiguration)
                    .then(response => {
                            const toks0 = GetTokens(response.data[0].name);

                            const groups = [{
                                name: toks0[0],
                                items: [{
                                    name: toks0[1],
                                    item: response.data[0]
                                }]
                            }];

                            response.data.reduce(function (previousValue, currentValue, index, array) {
                                const toks1 = GetTokens(previousValue.name);
                                const toks2 = GetTokens(currentValue.name);

                                const newItem = {
                                    name: toks2[1],
                                    item: currentValue
                                };

                                if (toks1[0] === toks2[0]) {
                                    groups[groups.length - 1].items.push(newItem);
                                } else {
                                    groups.push({
                                        name: toks2[0],
                                        items: [newItem]
                                    });
                                }

                                return currentValue;
                            });

                            function GetTokens(name) {
                                let arr = name.split(":");
                                return [arr[0], arr.splice(1).join(":")];
                            }

                            this.configurationItems = response.data;
                            this.configurationGroups = groups;
                        }
                    );
            },
            uploadConfiguration() {
                const data = new FormData();

                for (const ci of this.configurationItems)
                    data.append(ci.name, ci.value);

                this.loading = true;

                return this.$request(this.$AdminApi.ConfigurationAdmin.UploadConfiguration, data)
                    .then(_ => {
                            this.$successNotify();
                            this.loading = false;
                            this.loadConfiguration();
                        }
                    );
            }
        },
        beforeCreate() {
            this.$options.components.LoaderWait = require('sun').LoaderWait;
            this.$options.components.LoaderSent = require('sun').LoaderSent;
        },
        async created() {
            this.title = this.$tl("title");
            await this.getEnums();
            await this.loadConfiguration();
        }
    }
</script>

<style lang="scss">

  .configuration-admin__table {
    width: 100%;
  }

  .configuration-admin__group-header-tr {

  }

  .configuration-admin__group-header-td {
    padding: 10px 0;
    text-align: center;
  }

  .configuration-admin__group-header-div {
    padding: 6px;
    text-align: center;
    background-color: $grey-3;
    border-radius: 4px;
  }

  .configuration-admin__name-column {
    width: 150px !important;
    padding: 10px 10px 10px 3px;
  }

  .configuration-admin__value-column {
    textarea {
      height: 70px;
    }

    padding-right: 3px;
  }

</style>
