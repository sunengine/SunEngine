<template>
  <q-page class="flex middle">
    <div>
      <h4 class="text-lime-8">Расширения</h4>
      <div class="q-mb-lg text-orange-8">Тестируем тут.</div>

      <Ext name="place1"> <span class="text-green-7"> Hello </span> </Ext>
      <Ext tag="span" name="place2"> <span class="text-green-7"> Hi </span> </Ext>

    </div>
  </q-page>
</template>

<script>
  import Ext from 'Ext'
  import Page from "Page";

  export default {
    name: 'TestExt',
    ext: true,
    components: {Ext},
    mixins: [Page],
    data: function () {
      return {myData: "MyDataData"}
    },
    async created() {
      this.setTitle("ExtTest");
    }
  }

</script>

<style>
</style>
