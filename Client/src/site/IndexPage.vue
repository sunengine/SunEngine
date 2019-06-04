<template>
  <q-page class="index-page flex column middle page-padding">
   <!-- <div class="q-mb-lg q-mt-lg">
      <img src="statics/SunEngine.svg" width="220" />
    </div>-->
    <div class="logo-div">
      <div class="logo text-center">
        <router-link :to="{name: 'Home'}">
          <h3>{{$tl("demo")}}</h3>
        </router-link>
      </div>
    </div>
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
      <p class="ver q-mb-xl">{{$tl("version")}}: 1.4.2</p>
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
      color: grey;
      text-shadow: 1px 1px 1px black;
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

    .logo-div {
      text-align: center;


      .logo {

        margin: 28px 0;
        font-family: "BoomBoomRegular";
        //background-color: #d9eaf3;
        //border-radius: 120px;
        padding: 34px 25px;
        display: inline-block;
        //border: 1px dotted grey;
        //box-shadow: 0 0 2px 0px grey;
        text-shadow: 1px 1px 5px black;
        //text-shadow: 1px 1px 2px 0px grey;
        //background-color: #f6fff4;

        h3 {
          color: orange;
          margin: 0;
          font-size: 4.8rem;
        }

        h4 {
          color: #69d45a;
          margin: 0;
          font-size: 2.3rem;
        }
      }

    }
  }

</style>
