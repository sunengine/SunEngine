<template>
  <q-page class="flex column full-center center">
    <img v-if="photo" width="300" :src="photo"/>
    <br/>
    <input ref="file" type="file" accept="image/*" style="display:none" @change="handleFile"/>
    <QBtn color="send" class="q-mb-xl" :loading="loading" icon="far fa-user-circle" label="Выбрать фотографию"
          @click="upload"/>
    <QBtn v-if="!isDefault && !loading" color="negative" icon="fas fa-trash-alt" label="Сбросить фотографию"
          @click="resetAvatar"/>
  </q-page>
</template>

<script>
  import Page from "Page";

  const defaultAvatar = "_/default-avatar.svg";

  export default {

    name: "LoadPhoto",
    mixins: [Page],
    data: function () {
      return {
        loading: false
      }
    },
    computed: {
      photo() {
        if (this.$store && this.$store.state && this.$store.state.auth && this.$store.state.auth.userInfo.photo)
          return this.$store.state.auth.userInfo.photo;
        return null;
      },
      isDefault() {
        if (this.$store && this.$store.state && this.$store.state.auth && this.$store.state.auth.user.photo)
          return this.$store.state.auth.user.photo.endsWith(defaultAvatar);
      }
    },
    methods: {
      async handleFile() {
        if (!this.$refs.file.files.length)
          return;


        let formData = new FormData();
        formData.append('file', this.$refs.file.files[0]);

        this.loading = true;
        await this.$store.dispatch("request",
          {
            url: "/Images/UploadUserPhoto",
            data: formData
          })
          .then(
            async response => {
              await this.$store.dispatch('getMyUserInfo');
              this.loading = false;
              this.$q.notify({
                message: `Аватар успешно обновлён`,
                timeout: 2000,
                type: 'info',
                position: 'top'
              });
            }
          ).catch(x => {
            console.log("error", x);
            this.$q.notify({
              message: `Ошибка`,
              timeout: 2000,
              type: 'negative',
              position: 'top'
            });
          });
      },
      upload() {
        this.$refs.file.click();
      },
      async resetAvatar() {
        await this.$store.dispatch("request",
          {
            url: "/Personal/RemoveMyAvatar"
          })
          .then(
            async response => {
              await this.$store.dispatch('getMyUserInfo');
              this.loading = false;
              this.$q.notify({
                message: `Аватар успешно удалён`,
                timeout: 2000,
                type: 'info',
                position: 'top'
              });
              //this.$router.push({name: "Personal"});
            }
          ).catch(x => {
            this.loading = false;
            console.log("error", x);
            this.$q.notify({
              message: `Ошибка`,
              timeout: 2000,
              type: 'negative',
              position: 'top'
            });
          });
      }
    }
    ,
    async created() {
      this.setTitle("Изменить фотографию пользователя");
    }
  }
</script>

<style scoped>

</style>
