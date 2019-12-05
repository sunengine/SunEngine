<template>
  <q-page class="skins-admin page-padding">
    <h2 class="page-title">
      {{title}}
    </h2>
    <div>
      <q-tabs  class="bg-grey-2" align="justify" inline-label>
        <q-route-tab :to="{name: 'MainSkinsAdmin'}" no-caps name="skins" icon="fas fa-user-astronaut" :label="$tl('mainSkins')"/>
        <q-route-tab :to="{name: 'PartialSkinsAdmin'}" no-caps name="partial-skins" icon="fas fa-puzzle-piece" :label="$tl('partialSkins')"/>
      </q-tabs>
    </div>

    <div class="q-mb-xl"></div>

    <router-view />

  </q-page>
</template>

<script>
    import {Page} from 'mixins';

    export default {
        name: "SkinsAdmin",
        mixins: [Page],
        data() {
            return {
                tab: "skins"
            }
        },
        beforeCreate() {
            this.$options.components.MainSkinsAdmin = require('sun').MainSkinsAdmin;
            this.$options.components.PartialSkinsAdmin = require('sun').PartialSkinsAdmin;
        },
        created() {
            this.title = this.$tl('title');
        }
    }
</script>

<style lang="scss">

  .skins-admin {
    .q-tab__label {
      font-weight: bold;
    }
  }

  .skins-admin__tabs-panel {
    padding: 0;
  }

</style>
