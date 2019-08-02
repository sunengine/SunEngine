<template>
  <div class="sun-engine-footer q-footer--bordered">
    {{$tl("start")}} - <a href="http://sunengine.site">Sun Engine</a>
  </div>
</template>

<script>
  export default {
    name: "SunEngineFooter"
  }
</script>

<style lang="stylus">

  .sun-engine-footer {
    text-align: center;
    color: $primary;
    font-family: "BoomBoomRegular";
    font-size: 17px;
    letter-spacing: 1px;
    background-color: #d0fccf;

    a {
      color: orange;
      text-shadow: 0.5px 0.5px 0.5px #686569;

      &:hover {
        text-decoration: underline;
      }
    }

    .q-icon {
      margin: 0 16px;
    }
  }

</style>
