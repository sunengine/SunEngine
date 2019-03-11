<template>
  <q-list class="my-menu" no-border>

    <q-item :to="{name: 'CategoriesAdmin'}">
      <q-item-section avatar>
        <q-icon name="far fa-folder-open" />
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $ta("categoriesAdmin") }}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'RolesPermissions'}">
      <q-item-section avatar>
        <q-icon name="fas fa-users-cog" />
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $ta("rolesPermissions") }}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'RolesUsers'}">
      <q-item-section avatar>
        <q-icon name="fas fa-users" />
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $ta("rolesUsers") }}</q-item-label>
      </q-item-section>
    </q-item>

  </q-list>
</template>

<script>
  export default {
    name: "AdminMenu"
  }
</script>

<style scoped>

</style>
