<template>
  <q-list class="admin-menu my-menu" no-border>

    <q-item :to="{name: 'MenuItemsAdmin'}">
      <q-item-section avatar>
        <q-icon name="fas fa-bars"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("menuItemsAdmin") }}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'CategoriesAdmin'}">
      <q-item-section avatar>
        <q-icon name="fas fa-folder-open"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("categoriesAdmin") }}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'RolesPage'}">
      <q-item-section avatar>
        <q-icon name="fas fa-users"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("rolesUsers") }}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'RolesPermissions'}">
      <q-item-section avatar>
        <q-icon name="fas fa-users-cog"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("rolesPermissions") }}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'CacheSettings'}">
      <q-item-section avatar>
        <q-icon name="fa fa-sitemap"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("cacheSettings") }}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'DeletedElements'}">
      <q-item-section avatar>
        <q-icon name="fas fa-trash"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("deletedElements") }}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'ImagesCleaner'}">
      <q-item-section avatar>
        <q-icon name="far fa-image"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("imagesCleaner") }}</q-item-label>
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
