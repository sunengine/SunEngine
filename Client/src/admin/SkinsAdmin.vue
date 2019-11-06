<template>
  <q-page class="skins-admin q-page page-padding">

    <div class="page-title-block">
      <h2 class="page-title">
        {{title}}
      </h2>
      <q-btn no-caps icon="fas fa-cloud-upload-alt" @click="showUploadDialog"
             class="skins-admin__post-btn post-btn q-mb-lg"
             :label="$tl('upload')"/>
    </div>

    <input type="file" @change="uploadSkin" class="hidden" accept=".zip" ref="file"/>

    <div v-if="skins" class="row q-gutter-lg">
      <q-card :key="skin.name" class="skins-admin__card" v-for="skin in skins">

        <div style="height:172px">
          <q-img :class="{hidden: skin.showInfo}" :src="$buildPath(skinsDir,skin.name,'preview.png')"
                 class="skins-admin__skin-img"/>
          <q-card-section :class="{hidden: !skin.showInfo}">
            <div> {{$tl("author")}} {{skin.author}}</div>

            <div v-if="skin.contacts">
              {{$tl("contacts")}}
              <span :key="index" v-for="(contact,index) of skin.contacts">
                <a v-if="contact.startsWith('http')" :href="contact">{{contact}}</a>
                <template v-else>
                  {{contact}}
                </template>
                <template v-if="index != skin.contacts.length-1">, </template>
              </span>
            </div>
            <div>
              {{$tl("version")}} {{skin.version}}
            </div>
            <div>
              <a target="_blank" :href="skin.sourceUrl">{{$tl("link")}}</a>
            </div>
            <div>
              {{$tl("description")}} {{skin.description}}
            </div>
          </q-card-section>
        </div>
        <q-card-section class="skins-admin__skin-name">
          {{skin.name}}
        </q-card-section>

        <q-card-actions align="around">
          <q-btn v-if="skin.current" class="skins-admin__current-btn" flat no-caps disable :label="$tl('current')"
                 icon="fas fa-check"/>

          <q-btn flat v-if="!skin.current" no-caps @click="changeSkin(skin.name)" icon="fas fa-play"
                 class="skins-admin__send-btn" :label="$tl('set')"/>

          <q-btn flat icon="fas fa-search" class="skins-admin__preview-btn">
            <q-tooltip>
              <img :src="$buildPath(skinsDir,skin.name,'preview.png')" width="600"/>
            </q-tooltip>
          </q-btn>
          <q-btn class="skins-admin__delete-btn" no-caps @click="showSkinInfo(skin)" flat
                 icon="fas fa-info"/>
          <q-btn v-if="!skin.current" class="skins-admin__info-btn" no-caps @click="deleteSkin(skin.name)" flat
                 icon="fas fa-trash-alt"/>
        </q-card-actions>
      </q-card>
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
        computed: {
            skinsDir() {
                return config.SkinsDir;
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
                    this.$refs.file.value = "";
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
                        for (const skin of response.data)
                            skin.showInfo = false;
                        this.skins = response.data;
                    });
            },
            showSkinInfo(skin) {
                skin.showInfo = !!!skin.showInfo;
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
    text-align: center;
    font-family: "Roboto";
    //font-weight: 600;
    font-size: 1.30em;
    letter-spacing: 0.1px;
    padding: 4px !important;
    background-color: rgba(0, 0, 0, 0.15);
    color: $grey-8;
  }

  .skins-admin__card {
    width: 330px;
  }

  .skins-admin__delete-btn, .skins-admin__preview-btn {
    .q-icon {
      color: $grey-8;
    }
  }

  .skins-admin__current-btn {
    .q-icon {
      color: $green-5;
    }
  }

  .skins-admin__send-btn {
    background-color: $green-1;
    // color: white;
    .q-icon {
      color: $green-5;
    }
  }


</style>
