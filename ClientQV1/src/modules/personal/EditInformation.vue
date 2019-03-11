<template>
  <q-page class="flex column middle page-padding">
    <template v-if="userInfo">
      <div class="q-mb-lg text-grey-8">{{$tl("label")}}</div>

      <MyEditor class="q-mb-sm" style="max-width: 100%;"
                :toolbar="[
          ['bold', 'italic', 'strike', 'underline', 'subscript', 'superscript'],
        ['token', 'hr', 'link', 'addImages'],
                [
          {
            label: $q.lang.editor.formatting,
            icon: $q.iconSet.editor.formatting,
            list: 'no-icons',
            options: ['p', 'h2', 'h3', 'h4', 'h5', 'h6', 'code']
          },
          {
            label: $q.lang.editor.fontSize,
            icon: $q.iconSet.editor.fontSize,
            fixedLabel: true,
            fixedIcon: true,
            list: 'no-icons',
            options: ['size-1', 'size-2', 'size-3', 'size-4', 'size-5', 'size-6', 'size-7']
          },
          'removeFormat'
        ],
         ['quote', 'unordered', 'ordered', 'outdent', 'indent',

          {
            //label: $q.lang.editor.align,
            icon: $q.iconSet.editor.align,
            fixedLabel: true,
            options: ['left', 'center', 'right', 'justify']
          }
        ],
        ['undo', 'redo','fullscreen'],
             ]"

                ref="htmlEditor" v-model="userInfo.information"/>
      <q-btn no-caps class="send-btn" color="send" icon="far fa-save" :label="$tl('save')" @click="save"/>
    </template>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
  import MyEditor from "MyEditor";
  import LoaderWait from "LoaderWait";
  import Page from "Page";

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
          }).then(response => {
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
          console.error("error", error);
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
