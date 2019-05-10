<template>
  <q-page class="flex column middle page-padding">
    <template v-if="userInfo">
      <div class="q-mb-lg text-grey-8">{{$tl("label")}}</div>

      <MyEditor class="q-mb-sm" style="max-width: 100%;"
                :toolbar="editInformationToolbar"
                ref="htmlEditor" v-model="userInfo.information"/>
      <q-btn no-caps class="send-btn" color="send" icon="far fa-save" :label="$tl('save')" @click="save"/>
    </template>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
  import {MyEditor} from 'sun'
  import {LoaderWait} from 'sun'
  import {Page} from 'sun'
  import {editInformationToolbar as editInformationToolbar} from 'sun'

  export default {
    name: "EditInformation",
    mixins: [Page],
    components: {LoaderWait, MyEditor},
    data: function () {
      return {
        userInfo: {
          information: null,
        }
      }
    },
    methods: {
      async save() {
        await this.$store.dispatch("request",
          {
            url: "/Personal/SetMyProfileInformation",
            data: {
              html: this.userInfo.information
            }
          }).then(() => {
          this.$router.push({name: 'Personal'});
          const msg = this.$tl("editedSuccessNotify");
          this.$q.notify({
            message: msg,
            timeout: 2800,
            color: 'positive',
            icon: 'fas fa-check-circle',
            position: 'top'
          });
        }).catch(error => {
          this.$errorNotify(error);
        });
      }
    },
    async created() {
      this.title = this.$tl("title");
      await this.$store.dispatch("request",
        {
          url: "/Personal/GetMyProfileInformation",
        }).then(response => {
        this.userInfo = response.data;
      }).catch(error => {
        console.error("error", error);
      });
    }
  }
</script>

<style scoped>
  .send-btn {
    width: 270px;
  }
</style>
