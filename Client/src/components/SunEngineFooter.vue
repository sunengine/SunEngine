<template>
  <div class="sun-engine-footer">
    {{$tl("start")}} &nbsp;–&nbsp; <a href="https://sunengine.site">Sun Engine</a>
  </div>
</template>

<script>
  export default {
    name: "SunEngineFooter"
  }
</script>

<style lang="scss">

  .sun-engine-footer {
    text-align: center;

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
