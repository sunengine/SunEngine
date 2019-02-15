<template>
  <q-list class="main-list" no-border link inset-delimiter>
    <div class="logo-div center">
      <router-link to='/'>
        <img src="/statics/SunEngine.svg" class="logo">
      </router-link>
    </div>
    <q-item to='/' exact>
      <q-item-side icon="fas fa-home"/>
      <q-item-main label="Домой" sublabel="Главная страница"/>
    </q-item>
    <q-item to='/activities'>
      <q-item-side icon="far fa-newspaper"/>
      <q-item-main label="Новости" sublabel="Новости сайта"/>
    </q-item>
    <q-item to='/blog'>
      <q-item-side icon="far fa-newspaper"/>
      <q-item-main label="Блог" sublabel="Блог сайта"/>
    </q-item>
    <q-item to='/forum'>
      <q-item-side icon="far fa-comments"/>
      <q-item-main label="Форум" sublabel="Одноуровневый форум"/>
    </q-item>
    <q-item to='/forum2l'>
      <q-item-side icon="far fa-comments"/>
      <q-item-main label="Форум 2" sublabel="Двухуровневый форум"/>
    </q-item>
    <q-item to='/articles'>
      <q-item-side icon="far fa-file-alt"/>
      <q-item-main label="Статьи" sublabel="Раздел статей"/>
    </q-item>
    <q-item to='/testext'>
      <q-item-side icon="fas fa-flask"/>
      <q-item-main label="Lab-Ext" sublabel="Тестируем расширения"/>
    </q-item>
  </q-list>
</template>

<script>
  export default {
    name: "MainMenu"
  }
</script>

<style scoped>
  .logo-div {
    margin: 20px 0;
  }

  .logo {
    width: 220px;
    height: 220px;
    border-radius: 110px;
    box-shadow: 0 0 8px 1px grey;
  }

  .main-list {
    padding: 0 !important;
  }
</style>
