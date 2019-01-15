<template>
  <q-list
    no-border
    link
    inset-delimiter
  >
    <div class="center q-mb-md">
      <router-link to='/'>
        <img src="/statics/SunEngine.svg" class="logo">
      </router-link>
    </div>
    <q-item to='/' exact>
      <q-item-side icon="fas fa-home"/>
      <q-item-main label="Домой" sublabel="Главная страница"/>
    </q-item>
    <q-item to='/blog'>
      <q-item-side icon="far fa-newspaper"/>
      <q-item-main label="Блог" sublabel="Новости сайта"/>
    </q-item>
    <q-item to='/forum'>
      <q-item-side icon="far fa-comments"/>
      <q-item-main label="Форум" sublabel="Форум одноуровневый"/>
    </q-item>
    <q-item to='/forum2l'>
      <q-item-side icon="far fa-comments"/>
      <q-item-main label="Форум 2" sublabel="Форум двухуровневый"/>
    </q-item>
    <q-item to='/articles'>
      <q-item-side icon="far fa-file-alt"/>
      <q-item-main label="Статьи" sublabel="Читаем здесь"/>
    </q-item>
    <q-item to='/testext'>
      <q-item-side icon="fas fa-flask"/>
      <q-item-main label="Lab-Ext" sublabel="Тестируем расширения"/>
    </q-item>
  </q-list>
</template>

<script>
  export default {
    categoryName: "MainMenu"
  }
</script>

<style scoped>
  .q-item-side {
    text-align: center;
  }

  .logo {
    width: 220px;
    height: 220px;
    border-radius: 110px;
    box-shadow: 2px 2px 4px #3392ff;
  }
</style>
