<template>
  <div>
    <span>{{category.title}}</span>
    <span class="q-ml-md" v-if="category.name != 'Root'">
      <q-btn color="info" :class="{hidden: isFirst}" dense flat @click="$emit('up',category)"
             icon="fas fa-chevron-up"/>
      <q-btn color="info" :class="{hidden: isLast}" dense flat @click="$emit('down',category)"
             icon="fas fa-chevron-down"/>
      <q-btn color="info" dense flat @click="$emit('edit',category)" icon="fas fa-pencil-alt"/>
    </span>
    <div v-if="category.subCategories" style="padding-left: 25px">
      <category-item v-on="$listeners" :isFirst="index == 0" :isLast="index == category.subCategories.length - 1"
                     :key="sub.id" :category="sub" v-for="(sub,index) in category.subCategories"/>
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
      },
      isFirst: Boolean,
      isLast: Boolean
    },
    methods: {},
    data: function () {
      return {}
    }
  }
</script>

<style scoped>

</style>
