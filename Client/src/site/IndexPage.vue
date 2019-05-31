<template>
  <q-page class="flex column middle page-padding index-page">
    <h1 class="text-hot">{{$tl("demo")}}</h1>
    <div class="main-block">
      <p>{{$tl("dataReset")}}</p>
      <p>{{$tl("users")}}:
        <span class="hl">User1</span>,
        <span class="hl">User2</span>,
        <span class="hl">User3</span>,
        <span class="hl">Moderator</span>,
        <span class="hl">Admin</span>.</p>
      <p>{{$tl("password")}}: <span class="hl">password</span>.</p>
      <p>{{$tl("anyMailToRegister")}} <a target="_blank" href="http://demo.sunengine.site:1444"><b>mailcatcher</b></a>.
      </p>
      <p class="ver q-mb-xl">{{$tl("version")}}: 1.3.8</p>
    </div>
  </q-page>
</template>


<script>
  import {Page} from 'sun'

  export default {
    name: 'IndexPage',
    mixins: [Page],
  }
</script>


<style lang="stylus">

  .index-page {
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

    .main-block {
      p {
        margin: 4px 0;
        text-align: center;
        color: #616161;
      }

      .hl {
        color: #a38167;
        font-weight: 600;
      }
    }
  }

</style>
