<template>
  <q-page class="flex column full-center">
    <h1>Demo</h1>
    <div class="p1">
      <p>Каждые два часа данные сбрасываются.</p>
      <p>Пользователи: User1, User2, User3, User4, User5, Moderator, Admin</p>
      <p>Пароль: password</p>
      <p>При регистрации и других операциях с почтой можете указывать любой почтовый ящик, например `o1111@ooo.ru`, почта будет отправляться на mailcatcher <a target="_blank" href="http://demo.sunengine.site:1444">сюда</a>.</p>
    </div>
  </q-page>
</template>


<script>
  import Page from "Page";

  export default {
    name: 'PageIndex',
    mixins: [Page],
    async created() {
      this.title = "SunEngine";
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

</style>
