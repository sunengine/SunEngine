<template>
  <div class="loader-wait">
    <template v-if="!failed">
      <q-spinner size="20px" class="q-mr-xs"/>
      {{$tl("loading")}}
    </template>
    <template v-else>
      <q-icon name="fas fa-exclamation-triangle" size="20px" class="q-mr-xs"/>
      {{$tl("loadError")}}
    </template>
  </div>
</template>

<script>

    export default {
        name: 'LoaderWait',
        data() {
            return {
                failed: false
            }
        },
        methods: {
            fail() {
                this.failed = true;
            }
        }
    }

</script>

<style lang="scss">

  .loader-wait {
    font-size: 1.05rem;
    text-align: center;
    color: gray;
  }

</style>
