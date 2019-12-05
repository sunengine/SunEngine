<template>
  <q-page class="skins-admin">
    <h2 class="page-title page-padding">
      {{title}}
    </h2>
    <div class="page-padding">
      <q-tabs v-model="tab"  class="bg-grey-2"  align="justify" inline-label>
        <q-tab name="skins" icon="fas fa-user-astronaut" label="Основные темы">
        </q-tab>
        <q-tab name="partial-skins" icon="fas fa-puzzle-piece" label="Дополнительные темы">
        </q-tab>
      </q-tabs>
    </div>

    <div class="q-mb-xl"></div>

    <q-tab-panels class="page-padding"
      v-model="tab"
      animated
      transition-prev="jump-up"
      transition-next="jump-up"
    >
      <q-tab-panel class="skins-admin__tabs-panel" name="skins">
        <MainSkinsAdmin/>
      </q-tab-panel>
      <q-tab-panel class="skins-admin__tabs-panel" name="partial-skins">
        <PartialSkinsAdmin/>
      </q-tab-panel>
    </q-tab-panels>

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
