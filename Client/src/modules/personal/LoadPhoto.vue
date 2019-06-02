<template>
  <q-page class="load-photo flex column middle">
    <img v-if="photo" width="300" :src="photo"/>
    <br/>
    <input ref="file" type="file" accept="image/*" style="display:none" @change="handleFile"/>
    <q-btn no-caps color="send" class="q-mb-xl" :loading="loading" icon="far fa-user-circle"
           :label="$tl('uploadNewPhotoBtn')"
           @click="upload"/>
    <q-btn no-caps v-if="!isDefault && !loading" color="negative" icon="fas fa-trash-alt" :label="$tl('resetBtn')"
           @click="resetAvatar"/>
  </q-page>
</template>

<script>
  import {Page} from 'sun';


  const defaultAvatar = config.Misc.DefaultAvatar;

  export default {

    name: 'LoadPhoto',
    mixins: [Page],
    data() {
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
        if (this.$store && this.$store.state && this.$store.state.auth && this.$store.state.auth.userInfo.photo)
          return this.$store.state.auth.userInfo.photo.endsWith(defaultAvatar);
      }
    },
    methods: {
      async handleFile() {
        if (!this.$refs.file.files.length)
          return;


        let formData = new FormData();
        formData.append('file', this.$refs.file.files[0]);

        this.loading = true;
        await this.$store.dispatch('request',
          {
            url: '/UploadImages/UploadUserPhoto',
            data: formData
          })
          .then(async () => {
              await this.$store.dispatch('getMyUserInfo');
              this.loading = false;
              this.$successNotify(this.$tl('avatarChangedSuccessNotify'));
            }
          ).catch(error => {
            this.$errorNotify(error);
          });
      },
      upload() {
        this.$refs.file.click();
      },
      async resetAvatar() {
        await this.$store.dispatch('request',
          {
            url: '/Personal/RemoveMyAvatar'
          })
          .then(async () => {
              await this.$store.dispatch('getMyUserInfo');
              this.loading = false;
              this.$successNotify(this.$tl('avatarDeletedSuccessNotify'));
            }
          ).catch(x => {
            this.loading = false;
            console.log('error', x);
            const msg = this.$t('Global.errorNotify');
            this.$q.notify({
              message: msg,
              timeout: 2000,
              color: 'negative',
              position: 'top'
            });
          });
      }
    },
    async created() {
      this.title = this.$tl('title');
    }
  }

</script>

<style lang="stylus">

</style>
