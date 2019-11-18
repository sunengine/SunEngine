<template>
  <q-page class="load-photo flex column middle page-padding">
    <h2 class="page-title">
      {{title}}
    </h2>

    <img class="load-photo__photo" v-if="photo" width="300" :src="photo"/>
    <br/>
    <input ref="file" type="file" accept="image/*" class="hidden" @change="handleFile"/>
    <q-btn no-caps class="load-photo__send-btn send-btn q-mb-xl" :loading="loading" icon="far fa-user-circle"
           :label="$tl('uploadNewPhotoBtn')"
           @click="upload"/>
    <q-btn no-caps v-if="!isDefault && !loading" class="load-photo__delete-btn delete-btn" icon="fas fa-trash-alt" :label="$tl('resetBtn')"
           @click="resetAvatar"/>
  </q-page>
</template>

<script>
    import {Page} from 'mixins';


    const defaultAvatar = variables.Misc.DefaultAvatar;

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
                if (this.$store && this.$store.state && this.$store.state.auth && this.$store.state.auth.user.photo)
                    return this.$store.state.auth.user.photo;
                return null;
            },
            isDefault() {
                if (this.$store && this.$store.state && this.$store.state.auth && this.$store.state.auth.user.photo)
                    return this.$store.state.auth.user.photo.endsWith(defaultAvatar);
            }
        },
        methods: {
            handleFile() {
                if (!this.$refs.file.files.length)
                    return;

                let formData = new FormData();
                formData.append('file', this.$refs.file.files[0]);

                this.loading = true;

                this.$request(
                    this.$Api.UploadImages.UploadUserPhoto,
                    formData
                ).then(async () => {
                        await this.$store.dispatch('loadMyUserInfo');
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
            resetAvatar() {
                this.$request(
                    this.$Api.Personal.RemoveMyAvatar
                ).then(async () => {
                        await this.$store.dispatch('loadMyUserInfo');
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
        created() {
            this.title = this.$tl('title');
        }
    }

</script>

<style lang="scss">

</style>
