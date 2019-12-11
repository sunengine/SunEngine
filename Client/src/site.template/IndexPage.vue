<template>
  <q-page class="index-page middle flex flex-center">
    <div>
      <h1 class="index-page__header index-page__demo-header">
        SunEngine
      </h1>
      <div class="index-page__info-block">
        <p>{{$tl("users")}}:
          <span class="index-page__info-block-accent">User1</span>,
          <span class="index-page__info-block-accent">User2</span>,
          <span class="index-page__info-block-accent">User3</span>,
          <span class="index-page__info-block-accent">Moderator</span>,
          <span class="index-page__info-block-accent">Admin</span>.</p>
        <p>{{$tl("password")}}: <span class="index-page__info-block-accent">password</span>.</p>
        <p class="index-page__version">{{$tl("version")}}: <span class="num">{{version}}</span></p>
      </div>
    </div>
  </q-page>
</template>


<script>
    import {Page} from 'mixins'


    export default {
        name: 'IndexPage',
        mixins: [Page],
        computed: {
            version() {
                return process.env.PACKAGE_JSON.version;
            }
        }
    }

</script>


<style lang="scss">

  .index-page {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding-bottom: 0 !important;
  }

  .index-page__demo-header {
    margin: 0 0 40px;
    font-size: 6em;
    font-family: "Jomolhari";
    font-weight: bold;
    letter-spacing: 0.08em;
    line-heigh: 0;
    text-align: center;
    color: #00acef;
    text-shadow: 1px 1px 5px #b1b1b1;
  }

  .index-page__info-block {
    font-weight: 500;
    font-size: 1.2em;
    max-width: 600px;

    p {
      text-align: center;
      color: #616161;
    }
  }

  .index-page__info-block-accent {
    font-weight: 600;
    color: #a38167;
  }

  .index-page__version {
    .num {
      font-weight: 600;
      color: #330f53;
    }
  }

</style>
