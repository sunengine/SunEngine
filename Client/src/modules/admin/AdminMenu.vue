<template>
  <q-list class="my-menu" no-border>
    <q-item :to="{name: 'CategoriesAdmin'}">
      <q-item-section avatar>
        <q-icon name="far fa-folder-open"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("categoriesAdmin") }}</q-item-label>
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

    <q-item :to="{name: 'RolesPage'}">
      <q-item-section avatar>
        <q-icon name="fas fa-users"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("rolesUsers") }}</q-item-label>
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
  </q-list>
</template>

<script>
  export default {
    name: "AdminMenu",
    i18nPrefix: "Admin"
  };

</script>

<style scoped>
</style>
