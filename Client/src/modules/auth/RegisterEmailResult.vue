<template>
  <q-page class="flex middle page-padding">
    <q-banner v-if="success" class="bg-positive text-white">
      <template v-slot:avatar>
        <q-icon name="fas fa-check-circle" size="2em"/>
      </template>
      {{$tl("success")}}
      <router-link :to="{name: 'Login'}"> {{$tl("enter")}}</router-link>
      .
    </q-banner>
    <q-banner v-else class="bg-negative text-white">
      <template v-slot:avatar>
        <q-icon name="fas fa-exclamation-circle" size="2em"/>
      </template>
      {{$tl("error")}}
    </q-banner>
  </q-page>
</template>

<script>
  import {Page} from 'sun'

  export default {
    name: "RegisterEmailResult",
    mixins: [Page],
    computed: {
      success() {
        return this.$route.query["result"] === "ok";
      }
    },
    created() {
      this.title = this.$tl("title");
    }
  }
</script>

<style scoped>

</style>
