<template>
  <q-list class="my-menu" no-border>

    <q-item :to="{name: 'CategoriesAdmin'}">
      <q-item-section avatar>
        <q-icon name="far fa-folder-open" />
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $i18n.t("admin.adminMenu.categoriesAdminLabel") }}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'RolesPermissions'}">
      <q-item-section avatar>
        <q-icon name="far fa-users-cog" />
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $i18n.t("admin.adminMenu.rolesPermissionsLabel") }}</q-item-label>
      </q-item-section>
    </q-item>

    <q-item :to="{name: 'RolesUsers'}">
      <q-item-section avatar>
        <q-icon name="far fa-users" />
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $i18n.t("admin.adminMenu.rolesUsersLabel") }}</q-item-label>
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
