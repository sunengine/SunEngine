<template>
  <q-page class="roles-permissions page-padding">
    <h2 class="q-title">{{title}}</h2>
    <div class="q-my-md alert-backup">
      <q-icon name="fas fa-exclamation-circle" size="24px" class="q-mr-sm"/>
      {{$tl("backupWarning")}}
    </div>
    <template v-if="json !== null">
      <q-input input-class="json-input" v-model="json" type="textarea" :label="$tl('textAreaLabel')"/>
      <div class="q-my-md">
        <q-btn no-caps class="send-btn" icon="far fa-save" @click="send" :label="$tl('saveToServerBtn')"/>
        <q-btn no-caps class="q-ml-md" color="info" icon="fas fa-sync-alt" @click="loadDataRefresh"
               :label="$tl('getFromServer')"/>
      </div>
      <div class="json-error" v-if="error">
        <q-icon name="fas "/>
        <div class="msg" v-html="error.message"></div>
        <div class="stack" style="max-height: 600px; overflow-y: scroll;" v-html="error.text"></div>
      </div>
    </template>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
  import {Page} from 'sun'

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
      async loadData() {
        this.json = null;
        await this.$store.dispatch('request',
          {
            url: '/Admin/RolesPermissionsAdmin/GetJson'
          })
          .then(response => {
              this.error = null;
              this.json = response.data.json
            }
          ).catch(error => {
            this.$errorNotify(error);
          });
      },
      async send() {
        await this.$store.dispatch('request',
          {
            url: '/Admin/RolesPermissionsAdmin/UploadJson',
            data: {
              json: this.json
            }
          })
          .then(async () => {
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

<style lang="stylus">

  .roles-permissions {
    .alert-backup {
      border-radius: 10px;
      background-color: #eef4af;
      border: 1px solid silver;
      color: #ec5f00;
      padding: 15px;
    }

    .json-input {
      font-size: 0.9em !important;
      max-height: 600px !important;
      min-height: 400px !important;
      line-height: unset !important;
    }

    .json-error {
      color: maroon;

      .msg {
        font-weight: 600;
        margin-bottom: 20px;
      }

      .stack {
        word-wrap: break-word;
      }
    }
  }

</style>
