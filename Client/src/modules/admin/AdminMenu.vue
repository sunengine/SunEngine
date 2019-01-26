<template>
  <QList no-border>
    <q-item :to="{name: 'CategoriesAdmin'}" :multiline="false" link>
      <q-icon name="far fa-folder-open" size="16px" class="on-left"/>
      Категории
    </q-item>
    <q-item :to="{name: 'UserGroupsAdmin'}" :multiline="false" link>
      <q-icon name="fas fa-users" size="16px" class="on-left"/>
      Права групп пользователей
    </q-item>
  </QList>
</template>

<script>
    export default {
        name: "AdminMenu"
    }
</script>

<style scoped>

</style>
