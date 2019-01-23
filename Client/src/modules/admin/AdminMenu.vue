<template>

  <QList no-border>
    <h4 class="h-title text-grey-8"><QIcon name="fas fa-cog" size="1.4em" class="q-mr-sm" /> Админка</h4>

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
  .h-title {
    text-align: center;
    margin: 0;
    padding: 7px 0 4px 0;
    text-transform: uppercase;
    font-size: 0.9rem;
    font-weight: bolder;
    background-color: #f3ffdd;
    border-bottom: 1px solid #cacaca;
  }
</style>
