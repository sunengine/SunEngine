<template>
  <QList no-border>
    <q-item :to="{name: 'CategoriesAdmin'}" :multiline="false" link>
      <q-icon name="far fa-folder-open" size="16px" class="on-left"/>
      Категории
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
