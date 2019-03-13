<template>
  <q-list>
    <div class="logo-div text-center">
      <router-link to='/'>
        <img src="/statics/SunEngine.svg" class="logo">
      </router-link>
    </div>
    <q-item to='/' exact>
      <q-item-section avatar>
        <q-icon name="fas fa-home"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>Домой</q-item-label>
        <!--
                <q-item-label caption>Главная страница</q-item-label>
        -->
      </q-item-section>
    </q-item>
    <q-item to='/news2col'>
      <q-item-section avatar>
        <q-icon name="fas fa-home"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>Главная</q-item-label>
        <q-item-label caption>Пример страницы</q-item-label>
      </q-item-section>
    </q-item>
    <q-item to='/forum'>
      <q-item-section avatar>
        <q-icon name="far fa-comments"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>Форум</q-item-label>
        <q-item-label caption>Одноуровневый форум</q-item-label>
      </q-item-section>
    </q-item>
    <q-item to='/forum2l'>
      <q-item-section avatar>
        <q-icon name="far fa-comments"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>Форум</q-item-label>
        <q-item-label caption>Двухуровневый форум</q-item-label>
      </q-item-section>
    </q-item>
    <q-item to='/news'>
      <q-item-section avatar>
        <q-icon name="far fa-newspaper"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>Активность</q-item-label>
        <!--
                <q-item-label caption>Новое на сайте</q-item-label>
        -->
      </q-item-section>
    </q-item>
    <q-item to='/blog'>
      <q-item-section avatar>
        <q-icon name="fas fa-blog"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>Блог</q-item-label>
      </q-item-section>
    </q-item>
    <q-item to='/articles'>
      <q-item-section avatar>
        <q-icon name="far fa-file-alt"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>Статьи</q-item-label>
        <!--   <q-item-label caption>Раздел статей</q-item-label>-->
      </q-item-section>
    </q-item>


    <!-- <q-item to='/testext' >
       <q-item-section avatar>
         <q-icon name="fas fa-flask"/>
       </q-item-section>
       <q-item-section>
         <q-item-label>Lab-Ext</q-item-label>
         <q-item-label caption>Тестируем расширения</q-item-label>
       </q-item-section>
     </q-item>-->

  </q-list>
</template>

<script>
  export default {
    name: "MainMenu"
  }
</script>

<style lang="stylus" scoped>
  @import "~quasar-variables";

  .logo {
    width: 220px !important;
    height: 220px;
    border-radius: 110px;
    box-shadow: 0 0 8px 1px grey;
  }

  .logo-div {
    margin: 20px 0 12px;
  }

  .q-item__section--avatar {
    margin: 0 !important;
    padding: 0 !important;
    width: 60px !important;
    color: $grey-7;
    justify-items: center;
    align-items: center;
  }

  .q-item__section--main {
    font-size: 1.15em;
    //font-weight : 600;
  }


</style>
