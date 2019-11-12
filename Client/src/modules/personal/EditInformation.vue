<template>
  <q-page class="edit-information page-padding">
    <h2 class="page-title">
      {{title}}
    </h2>

    <template v-if="userInfo">
      <div class="edit-information__label q-mb-lg text-grey-8">{{$tl("label")}}</div>

      <SunEditor class="edit-information__editor q-mb-lg"
                 :toolbar="editorToolbar"
                 ref="htmlEditor" v-model="userInfo.information"/>
      <q-btn no-caps class="send-btn" icon="far fa-save" :label="$tl('save')" @click="save"/>
    </template>

    <LoaderWait v-else/>
  </q-page>
</template>

<script>
    import {Page} from 'mixins'
    import {editInformationToolbar} from 'sun'


    export default {
        name: 'EditInformation',
        mixins: [Page],
        data() {
            return {
                userInfo: {
                    information: null,
                }
            }
        },
        methods: {
            save() {
                this.$request(
                    this.$Api.Personal.SetMyProfileInformation,
                    {
                        html: this.userInfo.information
                    }
                ).then(() => {
                    this.$router.push({name: 'Personal'});
                    this.$successNotify();
                }).catch(error => {
                    this.$errorNotify(error)
                });
            }
        },
        beforeCreate() {
            this.editorToolbar = editInformationToolbar;
            this.$options.components.LoaderWait = require('sun').LoaderWait;
            this.$options.components.SunEditor = require('sun').SunEditor;
        },
        created() {
            this.title = this.$tl('title');
            this.$request(
                this.$Api.Personal.GetMyProfileInformation
            ).then(response => {
                this.userInfo = response.data;
            }).catch(error => {
                console.error('error', error)
            });
        }
    }

</script>

<style lang="scss">

  .edit-information__send-btn {
    width: 270px;
  }

</style>
