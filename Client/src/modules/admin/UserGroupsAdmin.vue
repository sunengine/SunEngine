<template>
  <q-page>
    <q-input class="json-input" v-model="json" type="textarea" float-label="UsersGroups Config Json" max-height="600"/>
    <q-btn class="q-my-md" color="send" icon="far fa-save" @click="send" label="Загрузить"/>
    <div class="json-error" v-if="error">
      <q-icon name="fas "/>
      <div class="msg">{{error.message}}</div>
      <div class="stack" style="max-height: 600px; overflow-y: scroll;" v-html="error.text"></div>
    </div>
  </q-page>
</template>

<script>
  import Page from "Page";
  import LoaderWait from "LoaderWait";

  export default {
    name: "UserGroupsAdmin",
    components: {LoaderWait},
    mixins: [Page],
    computed: {},
    data: function () {
      return {
        json: "",
        error: null
      }
    },
    methods: {
      async send() {
        await this.$store.dispatch("request",
          {
            url: "/GroupsAdmin/LoadJson",
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
              message: x.response.data.errorName,
              text: x.response.data.errorText.replace(/\n/g, '<br/>')
            };
            console.log("error", x);
          });
      }
    },
  }
</script>

<style lang="stylus" scoped>

  .json-input {
    font-size: 0.85em;

    >>> .q-input-target {
      line-height: unset !important;
    }
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


</style>
