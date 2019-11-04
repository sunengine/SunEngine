<template>
  <q-page class="roles-permissions page-padding">

    <h2 class="page-title">{{title}}</h2>

    <div class="q-gutter-y-lg" v-if="json !== null">
      <q-input input-class="roles-permissions__json-input" v-model="json" type="textarea"
               :label="$tl('textAreaLabel')"/>

      <div class="roles-permissions__btn-block q-gutter-x-md">
        <q-btn no-caps class="send-btn" icon="far fa-save" @click="send" :label="$tl('saveToServerBtn')"/>
        <q-btn no-caps class="refresh-btn" color="info" icon="fas fa-sync-alt" @click="loadDataRefresh"
               :label="$tl('getFromServer')"/>
      </div>

      <div class="roles-permissions__json-error" v-if="error">
        <q-icon name="fas fa-exclamation-triangle" size="24px" class="q-mr-md q-mt-xs float-left"/>
        <div class="roles-permissions__message" v-html="error.message"></div>
        <div class="roles-permissions__stack" v-html="error.text"></div>
      </div>
    </div>

    <LoaderWait v-else/>

  </q-page>
</template>

<script>
    import {Page} from 'mixins'

    export default {
        name: 'RolesPermissions',
        mixins: [Page],
        computed: {},
        data() {
            return {
                json: null,
                error: null
            }
        },
        methods: {
            async loadDataRefresh() {
                await this.loadData();
                this.$successNotify(this.$tl('getSuccessNotify'));
            },
            loadData() {
                this.json = null;
                this.$request(
                    this.$AdminApi.RolesPermissionsAdmin.GetJson
                ).then(response => {
                        this.error = null;
                        this.json = response.data.json
                    }
                ).catch(error => {
                    this.$errorNotify(error)
                });
            },
            send() {
                this.$request(
                    this.$AdminApi.RolesPermissionsAdmin.UploadJson,
                    {
                        json: this.json
                    }).then(async () => {
                        this.error = null;
                        this.$successNotify(this.$tl('saveSuccessNotify'));
                    }
                ).catch(error => {
                    this.error = error.response.data.errors[0];
                    this.$errorNotify(error, this.error.message);
                });
            }
        },
        beforeCreate() {
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        },
        async created() {
            this.title = this.$tl('title');
            await this.loadData();
        }
    }

</script>

<style lang="scss">

  .roles-permissions__alert-backup {
    border-radius: 10px;
    background-color: #eef4af;
    border: 1px solid silver;
    color: #ec5f00;
    padding: 15px;
  }

  .roles-permissions__json-input {
    font-size: 0.9em !important;
    max-height: 600px !important;
    min-height: 400px !important;
    line-height: unset !important;
  }

  .roles-permissions__json-error {
    color: #bd0000;
    border-radius: 5px;
    border: 1px solid silver;
    background-color: #edf6ff;
    padding: 12px;
  }

  .roles-permissions__message {
    font-weight: 600;
  }

  .roles-permissions__stack {
    word-wrap: break-word;
    max-height: 600px;
    overflow-y: scroll;
  }

</style>
