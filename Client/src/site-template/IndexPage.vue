<template>
  <q-page class="index-page flex middle">

    <div class="main-block">
      <h1>
        {{$tl("demo")}}
      </h1>
      <p>{{$tl("users")}}:
        <span class="hl">User1</span>,
        <span class="hl">User2</span>,
        <span class="hl">User3</span>,
        <span class="hl">Moderator</span>,
        <span class="hl">Admin</span>.</p>
      <p>{{$tl("password")}}: <span class="hl">password</span>.</p>
      <p class="ver">{{$tl("version")}}: <span class="num">1.10.4</span></p>
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

    padding: 0 !important;

    h1 {
      margin: 0 0 40px;
      font-family: "BoomBoomRegular";
      letter-spacing: 2px;
      text-align: center;
      color: orange;
      text-shadow: 1px 1px 5px black;
    }

    .ver {
      color: $primary;

      .num {
        font-weight: 600;
        color: #330f53;
      }
    }

    .main-block {
      font-weight: 500;
      font-size: 1.15em;
      max-width: 600px;

      p {
        text-align: center;
        color: #616161;
      }

      .hl {
        color: #a38167;
      }
    }
  }

</style>
