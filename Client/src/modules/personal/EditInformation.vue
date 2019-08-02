<template>
  <q-page class="edit-information flex column middle page-padding">
    <template v-if="userInfo">
      <div class="q-mb-lg text-grey-8">{{$tl("label")}}</div>

      <SunEditor class="q-mb-sm" style="max-width: 100%;"
                :toolbar="editorToolbar"
                ref="htmlEditor" v-model="userInfo.information"/>
      <q-btn no-caps class="send-btn" icon="far fa-save" :label="$tl('save')" @click="save"/>
    </template>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
  import {Page} from 'sun'
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
      async save() {
        await this.$store.dispatch('request',
          {
            url: '/Personal/SetMyProfileInformation',
            data: {
              html: this.userInfo.information
            }
          }).then(() => {
          this.$router.push({name: 'Personal'});
          this.$successNotify();
        }).catch(error => {
          this.$errorNotify(error);
        });
      }
    },
    beforeCreate() {
      this.editorToolbar = editInformationToolbar;
      this.$options.components.LoaderWait = require('sun').LoaderWait;
      this.$options.components.SunEditor = require('sun').SunEditor;
    },
    async created() {
      this.title = this.$tl('title');
      await this.$store.dispatch('request',
        {
          url: '/Personal/GetMyProfileInformation',
        }).then(response => {
        this.userInfo = response.data;
      }).catch(error => {
        console.error('error', error);
      });
    }
  }

</script>

<style lang="stylus">

  .edit-information {

    .send-btn {
      width: 270px;
    }
  }

</style>
