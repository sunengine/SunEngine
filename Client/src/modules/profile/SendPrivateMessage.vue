<template>
  <q-page class="page-padding">
    <h2 class="q-title"> {{$tl("titleStart")}}
      <q-icon name="far fa-user" color="grey-7"/>
      {{userName}}
    </h2>

    <q-editor class="q-mb-md"
              :toolbar="editorToolbar"
              ref="htmlEditor" v-model="text"/>

    <q-btn no-caps icon="fas fa-arrow-circle-right" class="q-mr-sm" @click="send" color="send" :loading="loading"
           :label="$tl('sendBtn')">
      <loader-sent slot="loading"/>
    </q-btn>
    <q-btn no-caps icon="fas fa-times" @click="$router.back()" color="warning" :label="$t('Global.btn.cancel')"/>
  </q-page>
</template>


<script>
  import {Page} from 'sun'
  import {sendPrivateMessageToolbar} from 'sun'

  const editorToolbar = sendPrivateMessageToolbar;

  export default {
    name: "SendPrivateMessage",
    mixins: [Page],
    props: {
      userId: {
        type: String,
        required: true
      },
      userName: {
        type: String,
        required: true
      }
    },
    data() {
      return {
        text: "",
        loading: false
      }
    },
    methods: {
      async send() {
        await this.$store.dispatch("request",
          {
            url: "/Profile/SendPrivateMessage",
            data: {
              userId: this.userId,
              text: this.text
            }
          })
          .then( () => {
            this.$successNotify();
            this.loading = false;
              this.$router.$goBack();
            }
          ).catch(error => {
            this.$errorNotify(error);
            this.loading = false;
          });

      }
    },
    beforeCreate() {
      this.$options.components.LoaderSent = require('sun.js').LoaderSent;
    },
    created() {
      this.title = this.$tl("title");
    }
  }
</script>

<style scoped>

</style>
