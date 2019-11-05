<template>
  <q-page class="skins-admin q-page page-padding">

    <div class="page-title-block">
      <h2 class="page-title">
        {{title}}
      </h2>
      <q-btn no-caps icon="fas fa-cloud-upload-alt" @click="showUploadDialog" class="send-btn q-mb-lg"
             :label="$tl('upload')"/>
    </div>

    <input type="file" @change="uploadSkin" class="hidden" ref="file"/>
    <div v-if="skins">
      <q-list>
        <q-item v-for="skin of skins">
          <q-item-section class="skins-admin__skin-name">
            {{skin}}
          </q-item-section>
          <q-item-section avatar>
            <q-btn no-caps @click="changeSkin(skin)" class="send-btn" icon-right="fas fa-play" :label="$tl('set')"/>
          </q-item-section>
          <q-item-section avatar>
            <q-btn no-caps @click="changeSkin(skin)" class="delete-btn" icon="fas fa-ban"/>
          </q-item-section>
        </q-item>
      </q-list>
    </div>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
    import {Page} from 'mixins';

    export default {
        name: "SkinsAdmin",
        mixins: [Page],
        data() {
            return {
                skins: null
            }
        },
        methods: {
            showUploadDialog() {
                this.$refs.file.click();
            },
            uploadSkin() {
                const file = this.$refs.file.files[0];
                const formData = new FormData();
                formData.append('file', file);
                this.$request(this.$AdminApi.SkinsAdmin.UploadSkin,
                    formData
                ).then(response => {
                    this.$successNotify();
                });
            },
            changeSkin(name) {
                this.$request(this.$AdminApi.SkinsAdmin.ChangeSkin,
                    {
                        name: name
                    }
                ).then(response => {
                    this.$successNotify();
                });
            },
            getAllSkins() {
                this.$request(this.$AdminApi.SkinsAdmin.GetAllSkins)
                    .then(response => {
                        this.skins = response.data
                    });
            }
        },
        beforeCreate() {
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        },
        created() {
            this.title = this.$tl('title');
            this.getAllSkins();
        }
    }
</script>

<style lang="scss">

  .skins-admin__skin-name {
    font-weight: 600;
    font-size: 1.2em;
  }

</style>
