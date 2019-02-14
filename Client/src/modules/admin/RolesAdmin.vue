<template>
  <q-page>
    <h2 class="q-title">Загрузка Json прав для групп</h2>
    <div class="q-my-md alert-backup">
      <q-icon name="fas fa-exclamation-circle" size="24px" class="q-mr-sm"/>
      Перед загрузкой необходимо сделать backup базы.
    </div>
    <template v-if="json !== null">
      <q-input class="json-input" v-model="json" type="textarea" float-label="UsersGroups Config Json"
               max-height="600"/>
      <div class="q-my-md">
        <q-btn color="send" icon="far fa-save" @click="send" label="Сохранить на сервер"/>
        <q-btn class="q-ml-md" color="info" icon="fas fa-sync-alt" @click="loadDataRefresh"
               label="Загрузить с сервера"/>
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
    name: "RolesAdmin",
    components: {LoaderWait},
    mixins: [Page],
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
        this.$q.notify({
          message: 'Данные загружены с сервера',
          timeout: 5000,
          type: 'info',
          position: 'top'
        });
      },
      async loadData() {
        this.json = null;
        await this.$store.dispatch("request",
          {
            url: "/Roles/GetJson"
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
            url: "/Roles/UploadJson",
            data: {
              json: this.json
            }
          })
          .then(
            async response => {
              this.error = null;
              this.$q.notify({
                message: 'Настройки групп успешно установлены',
                timeout: 4000,
                type: 'positive',
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
      this.setTitle("Загрузка Json прав для групп");
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

  .json-input {
    font-size: 0.85em;

    >>> .q-input-target {
      line-height: unset !important;
    }
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
