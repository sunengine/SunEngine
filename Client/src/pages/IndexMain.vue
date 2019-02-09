<template>
  <q-page class="flex column middle">
    <h1>Sun Engine</h1>

    <h5>
      Движок для сайтов с функциональностью форума, статей, блога.
    </h5>
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
    margin : 0 0 20px 0;
    font-family: "BoomBoomRegular";
  }

  h5 {
    color: $secondary;
    font-family: "BoomBoomRegular";
  }

</style>
