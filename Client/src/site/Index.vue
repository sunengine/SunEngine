<template>
  <q-page class="flex column middle page-padding">
    <h1 class="text-hot">Demo</h1>
    <div class="p1">
      <p>Каждые несколько часов данные сбрасываются.</p>
      <p>Пользователи: <span class="hl">User1</span>, <span class="hl">User2</span>, <span class="hl">User3</span>, <span class="hl">Moderator</span>, <span
          class="hl">Admin</span></p>
      <p>Пароль: <span class="hl">password</span></p>
      <p>При регистрации можете указывать любой почтовый ящик, например <span class="hl">ooo@ooo.eml</span>.
        <br/>
        Почта будет отправляться на <a target="_blank" href="http://demo.sunengine.site:1444"><b>mailcatcher</b></a>.</p>
      <p class="ver q-mb-xl">Версия: 1.0.0-beta.14</p>
    </div>
  </q-page>
</template>


<script>
  import Page from "Page";

  export default {
    name: 'Index',
    mixins: [Page],
  }
</script>


<style lang="stylus" scoped>
  @import '~quasar-variables';

  h1 {
    margin: 0 0 40px;
    font-family: "BoomBoomRegular";
    letter-spacing: 2px;
    text-align: center;
  }

  .ver {
    font-weight: bold;
    color: $primary;
  }

  .p1 p {
    margin: 4px 0;
    text-align: center;
    color: #616161;
  }

  .hl {
    color: #a38167;
    font-weight: 600;
  }
</style>
