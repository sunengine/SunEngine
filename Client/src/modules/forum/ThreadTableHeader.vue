<template>
  <div class="thread__table-header margin-back bg-grey-2 gt-xs text-grey-6 ">

    <hr class="thread__sep"/>

    <div class="row">
      <div class="col-xs-12 col-sm-8" style="padding: 2px 0px 2px 76px; ">
        {{$tl("topic")}}
      </div>
      <div class="col-xs-12 col-sm-2" style="padding: 2px 0px 2px 60px;">
        {{$tl("last")}}
      </div>
    </div>
  </div>
</template>

<script>
    export default {
        name: "ThreadTableHeader"
    }
</script>

<style scoped>

</style>
