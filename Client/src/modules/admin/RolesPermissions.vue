<template>
  <q-page class="page-padding">
    <h2 class="q-title">{{title}}</h2>
    <div class="q-my-md alert-backup">
      <q-icon name="fas fa-exclamation-circle" size="24px" class="q-mr-sm"/>
      {{$tl("backupWarning")}}
    </div>
    <template v-if="json !== null">
      <q-input input-class="json-input" v-model="json" type="textarea" :label="$tl('textAreaLabel')"/>
      <div class="q-my-md">
        <q-btn no-caps color="send" icon="far fa-save" @click="send" :label="$tl('saveToServerBtn')"/>
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
  import Page from "Page";
  import LoaderWait from "LoaderWait";

  export default {
    name: "RolesPermissions",
    components: {LoaderWait},
    mixins: [Page],
    i18nPrefix: "admin",
    computed: {},
    data: function () {
      return {
        json: null,
        error: null
      }
    },
    methods: {

      async loadDataRefresh() {
        await this.loadData();
        const msg = this.$tl('getFromServerSuccessNotify');
        this.$q.notify({
          message: msg,
          timeout: 5000,
          color: 'info',
          position: 'top'
        });
      },
      async loadData() {
        this.json = null;
        await this.$store.dispatch("request",
          {
            url: "/Admin/AdminRolesPermissions/GetJson"
          })
          .then(response => {
              this.error = null;
              this.json = response.data.json
            }
          ).catch(x => {
            console.log("error", response);
          });
      },
      async send() {
        await this.$store.dispatch("request",
          {
            url: "/Admin/AdminRolesPermissions/UploadJson",
            data: {
              json: this.json
            }
          })
          .then(async () => {
              this.error = null;
              const msg = this.$tl("saveToServerSuccessNotify");
              this.$q.notify({
                message: msg,
                timeout: 4000,
                color: 'positive',
                icon: 'far fa-check-circle',
                position: 'top'
              });
            }
          ).catch(x => {
            this.error = {
              message: x.response.data.errorName.replace(/\n/g, '<br/>'),
              text: x.response.data.errorText.replace(/\n/g, '<br/>')
            };
            console.log("error", x);
          });
      }
    },
    async created() {
      this.title = this.$tl("title");
      await this.loadData();
    }
  }
</script>

<style lang="stylus" scoped>

  .alert-backup {
    border-radius: 10px;
    background-color: #eef4af;
    border: 1px solid silver;
    color: #ec5f00;
    padding: 15px;
    //font-weight: 500;
  }

  >>> .json-input {
    font-size: 0.9em !important;
    max-height: 600px !important;
    min-height: 400px !important;
    line-height: unset !important;
  }

  .json-error {
    color: maroon;

    >>> .msg {
      font-weight: 600;
      margin-bottom: 20px;
    }

    >>> .stack {
      word-wrap: break-word;
    }
  }


</style>
