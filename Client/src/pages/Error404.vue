<template>
  <q-page class="flex column flex-center">

      <p>
        <img
          src="~assets/sad.svg"
          style="width:30vw;max-width:150px;"
        >
      </p>
      <p class="text-faded">
        {{$tl("info")}}
        <!--Sorry, nothing here... -->
        <strong>(404)</strong></p>
      <q-btn
        color="secondary"
        style="width:200px;"
        @click="$router.push('/')"
      ><!--Go back-->{{$tl("goBackBtn")}}
      </q-btn>

  </q-page>
</template>

<script>
  import {Page} from 'sun'

  export default {
    name: 'Error404',
    mixins: [Page],
    created() {
      this.title = this.$tl("title");
    }
  }
</script>
