<template>
  <QPage class="flex column full-center">
    <template v-if="userInfo">
      <div class="q-mb-lg text-grey-8">Информация о вас на странице вашего профиля.</div>

      <MyEditor class="q-mb-sm" style="max-width: 100%;"
        :toolbar="[
          ['bold', 'italic', 'strike', 'underline', 'subscript', 'superscript'],
        ['token', 'hr', 'link', 'addImages'],

                [
          {
            label: $q.i18n.editor.formatting,
            icon: $q.icon.editor.formatting,
            list: 'no-icons',
            options: ['p', 'h2', 'h3', 'h4', 'h5', 'h6', 'code']
          },
          {
            label: $q.i18n.editor.fontSize,
            icon: $q.icon.editor.fontSize,
            fixedLabel: true,
            fixedIcon: true,
            list: 'no-icons',
            options: ['size-1', 'size-2', 'size-3', 'size-4', 'size-5', 'size-6', 'size-7']
          },
          'removeFormat'
        ],
         ['quote', 'unordered', 'ordered', 'outdent', 'indent',

          {
            //label: $q.i18n.editor.align,
            icon: $q.icon.editor.align,
            fixedLabel: true,
            options: ['left', 'center', 'right', 'justify']
          }
        ],
        ['undo', 'redo','fullscreen'],
             ]"

        ref="htmlEditor" v-model="userInfo.information"/>
      <QBtn class="send-btn" color="send" icon="far fa-save" label="Сохранить" @click="save"/>
    </template>
    <LoaderWait v-else/>
  </QPage>
</template>

<script>
  import MyEditor from "MyEditor";
  import LoaderWait from "LoaderWait";
  import Page from "Page";

  export default {
    name: "EditUserProfileInformation",
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
          this.$router.push({name:'Personal'});
          this.$q.notify({
            message: `Информация успешно отредактирована`,
            timeout: 2000,
            type: 'info',
            position: 'top'
          });
        }).catch(error => {
          console.error("error", error);
        });
      }
    },
    async created() {
      this.setTitle("Редактировать информацию о себе");
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
