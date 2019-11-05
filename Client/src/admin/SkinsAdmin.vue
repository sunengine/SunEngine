<template>
  <q-page class="skins-admin q-page page-padding">

    <div class="page-title-block">
      <h2 class="page-title">
        {{title}}
      </h2>
      <q-btn no-caps icon="fas fa-cloud-upload-alt" @click="showUploadDialog" class="post-btn q-mb-lg"
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
            <q-banner rounded class="bg-info"   v-if="skin === current">
              Current <q-icon size="24px" class="q-ml-sm" name="fas fa-check"/>
            </q-banner>
            <q-btn v-if="skin !== current" no-caps @click="changeSkin(skin)" class="send-btn skins-admin__send-btn" icon-right="fas fa-play"
                   :label="$tl('set')"/>
          </q-item-section>
          <q-item-section avatar>
            <q-btn v-if="skin !== current" no-caps @click="deleteSkin(skin)" class="delete-btn" icon="fas fa-ban"/>
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
                skins: null,
                current: null
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
                    this.$successNotify(this.$tl("uploadSuccessNotify"));
                    this.getAllSkins();
                });
            },
            deleteSkin(name) {
                const deleteMsg = this.$tl('deleteMsg');
                const btnDeleteOk = this.$tl('btnDeleteOk');
                const btnDeleteCancel = this.$tl('btnDeleteCancel');

                this.$q.dialog({
                    message: deleteMsg,
                    ok: btnDeleteOk,
                    cancel: btnDeleteCancel
                }).onOk(() => {
                    this.$request(this.$AdminApi.SkinsAdmin.DeleteSkin,
                        {
                            name: name
                        }
                    ).then(_ => {
                        this.$successNotify(this.$tl("deleteSuccessNotify"));
                        this.getAllSkins();
                    });
                });
            },
            changeSkin(name) {
                this.$request(this.$AdminApi.SkinsAdmin.ChangeSkin,
                    {
                        name: name
                    }
                ).then(_ => {
                    this.$successNotify();
                    this.getAllSkins();
                });
            },
            getAllSkins() {
                this.$request(this.$AdminApi.SkinsAdmin.GetAllSkins)
                    .then(response => {
                        this.skins = response.data.all;
                        this.current = response.data.current;
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
