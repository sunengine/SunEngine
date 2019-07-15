<template>
  <q-list class="admin-menu my-menu" no-border>

    <q-item :to="{name: 'MenuItemsAdmin'}">
      <q-item-section avatar>
        <q-icon name="fas fa-bars"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("menuItems") }}</q-item-label>
        <q-item-label v-if="menuItemsCaption" caption>{{menuItemsCaption}}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'CategoriesAdmin'}">
      <q-item-section avatar>
        <q-icon name="fas fa-folder"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("categories") }}</q-item-label>
        <q-item-label v-if="categoriesCaption" caption>{{categoriesCaption}}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'RolesPage'}">
      <q-item-section avatar>
        <q-icon name="fas fa-users"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("rolesUsers") }}</q-item-label>
        <q-item-label v-if="rolesUsersCaption" caption>{{rolesUsersCaption}}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'RolesPermissions'}">
      <q-item-section avatar>
        <q-icon name="fas fa-users-cog"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("rolesPermissions") }}</q-item-label>
        <q-item-label v-if="rolesPermissionsCaption" caption>{{rolesPermissionsCaption}}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'CacheSettings'}">
      <q-item-section avatar>
        <q-icon name="fa fa-sitemap"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("cacheSettings") }}</q-item-label>
        <q-item-label v-if="cacheSettingsCaption" caption>{{cacheSettingsCaption}}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'ImagesCleaner'}">
      <q-item-section avatar>
        <q-icon name="fas fa-image"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("imagesCleaner") }}</q-item-label>
        <q-item-label v-if="imagesCleanerCaption" caption>{{imagesCleanerCaption}}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'CypherSecrets'}">
      <q-item-section avatar>
        <q-icon name="fas fa-key"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("cypherSecrets") }}</q-item-label>
        <q-item-label v-if="cypherSecretsCaption" caption>{{cypherSecretsCaption}}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'DeletedElements'}">
      <q-item-section avatar>
        <q-icon name="fas fa-trash"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("deletedElements") }}</q-item-label>
        <q-item-label v-if="deletedElementsCaption" caption>{{deletedElementsCaption}}</q-item-label>
      </q-item-section>
    </q-item>

    <div class="text-grey-7 q-mt-xl text-center" v-if="version">
      {{$tl("version")}}: {{version}}
    </div>
  </q-list>

</template>

<script>

  export default {
    name: 'AdminMenu',
    data() {
      return {
        version: null
      }
    },
    computed: {
      menuItemsCaption() {
        return this.$tl("menuItemsCaption") ?? null;
      },
      categoriesCaption() {
        return this.$tl("categoriesCaption") ?? null;
      },
      rolesUsersCaption() {
        return this.$tl("rolesUsersCaption") ?? null;
      },
      rolesPermissionsCaption() {
        return this.$tl("rolesPermissionsCaption") ?? null;
      },
      cacheSettingsCaption() {
        return this.$tl("cacheSettingsCaption") ?? null;
      },
      imagesCleanerCaption() {
        return this.$tl("imagesCleanerCaption") ?? null;
      },
      deletedElementsCaption() {
        return this.$tl("deletedElementsCaption") ?? null;
      },
      cypherSecretsCaption() {
        return this.$tl("cypherSecretsCaption") ?? null;
      }
    },
    methods: {
      async getVersion() {
        await this.$store
          .dispatch('request', {
            url: '/Pulse/Version'
          })
          .then(response => {
            this.version = response.data.version;
          });
      }
    },
    async created() {
      await this.getVersion();
    }
  }

</script>

<style lang="stylus">

</style>
