<template>
  <q-page class="send-private-message page-padding">
    <h2 class="page-title"> {{$tl("titleStart")}}
      <q-icon name="far fa-user" color="grey-7"/>
      {{userName}}
    </h2>

    <q-editor class="send-private-message__editor q-mb-md" :toolbar="editorToolbar" ref="htmlEditor" v-model="text"/>

    <q-btn class="send-btn q-mr-sm" no-caps icon="fas fa-arrow-circle-right"
           @click="send" :loading="loading" :label="$tl('sendBtn')">
      <loader-sent slot="loading"/>
    </q-btn>
    <q-btn class="cancel-btn" no-caps icon="fas fa-times" @click="$router.back()"
           :label="$t('Global.btn.cancel')"/>
  </q-page>
</template>


<script>
    import {Page} from 'mixins'
    import {sendPrivateMessageToolbar} from 'sun'

    const editorToolbar = sendPrivateMessageToolbar;


    export default {
        name: 'SendPrivateMessage',
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
                text: '',
                loading: false
            }
        },
        methods: {
            send() {
                this.$request(
                    this.$Api.Profile.SendPrivateMessage,
                    {
                        userId: this.userId,
                        text: this.text
                    }
                ).then(_ => {
                        this.$successNotify();
                        this.loading = false;
                        this.$router.back();
                    }
                ).catch(error => {
                    this.$errorNotify(error);
                    this.loading = false;
                })
            }
        },
        beforeCreate() {
            this.$options.components.LoaderSent = require('sun').LoaderSent
        },
        created() {
            this.title = this.$tl('title')
        }
    }

</script>

<style lang="scss">

</style>
