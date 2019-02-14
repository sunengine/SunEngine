<template>
  <q-page class="flex column middle">
    <h1>Demo</h1>
    <div class="p1">
      <p>Каждые несколько часов данные сбрасываются.</p>
      <p>Пользователи: <span class="hl">User1</span>, <span class="hl">User2</span>, <span class="hl">User3</span>, <span class="hl">User4</span>, <span class="hl">User5</span>, <span class="hl">Moderator</span>, <span class="hl">Admin</span></p>
      <p>Пароль: <span class="hl">password</span></p>
      <p>При регистрации и других операциях с почтой можете указывать любой почтовый ящик, например '<span class="hl">ooo@ooo.ru</span>', почта будет отправляться на mailcatcher <a target="_blank" href="http://demo.sunengine.site:1444">сюда</a>.</p>
    </div>
  </q-page>
</template>


<script>
  import Page from "Page";

  export default {
    name: 'PageIndex',
    mixins: [Page],
    async created() {
      this.title = config.SiteName;
    }
  }
</script>


<style csoped lang="stylus">
  @import '~variables';


  h1 {
    color: $primary;
    margin: 0 0 30px 0;
    font-family: "BoomBoomRegular";
    letter-spacing: 2px;
  }


  .p1 p {
    margin: 4px 0;
    text-align: center;
    color: #616161;
  }

  .hl {
    color: #a38167;
    font-weight : 600;
  }


</style>
