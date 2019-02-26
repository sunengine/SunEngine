<template>
  <q-page class="flex flex-center">
    <div>
      <p>
        <img
          src="~assets/sad.svg"
          style="width:30vw;max-width:150px;"
        >
      </p>
      <p class="text-faded">Sorry, nothing here...<strong>(404)</strong></p>
      <q-btn
        color="secondary"
        style="width:200px;"
        @click="$router.push('/')"
      >Go back
      </q-btn>
    </div>
  </q-page>
</template>

<script>
  export default {
    name: 'Error404'
  }
</script>
