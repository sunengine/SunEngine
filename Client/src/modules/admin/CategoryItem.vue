<template>
  <div>
    <span>{{category.title}}</span>
    <span class="q-ml-md" v-if="category.name != 'Root'">
      <q-btn dense flat icon="fas fa-chevron-up" />
      <q-btn dense flat  icon="fas fa-chevron-down" />
    </span>
    <div v-if="category.subCategories" style="padding-left: 25px">
      <category-item :key="sub.id" :category="sub" v-for="sub in category.subCategories" />
    </div>
  </div>
</template>

<script>
  export default {
    name: "CategoryItem",
    props: {
      category: {
        type: Object,
        required: true
      }
    },
    methods: {

    },
    data: function () {
      return {}
    }
  }
</script>

<style scoped>

</style>
