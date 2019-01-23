<template>
  <div>
    <span class="item-block">
    <span v-if="notRoot" class="q-mr-sm ud">
      <q-btn color="info" :disabled="isFirst" dense flat @click="$emit('up',category)"
             icon="fas fa-chevron-up"/>
      <q-btn color="info" :disabled="isLast" dense flat @click="$emit('down',category)"
             icon="fas fa-chevron-down"/>
    </span>
    <span v-if="notRoot">{{category.title}}</span>
          <span v-else>Корневая категория</span>

    <span class="q-ml-md" v-if="notRoot">
      <q-btn color="info" dense flat @click="$emit('go',category.name)" icon="fas fa-arrow-right"/>
      <q-btn color="info" dense flat @click="$emit('edit',category.id)" icon="fas fa-pencil-alt"/>
    </span>
      </span>
    <div v-if="category.subCategories" :class="[{'padding-c': notRoot}]">
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
    computed: {
      notRoot() {
        return this.category.name != 'Root'
      }
    },
    methods: {},
    data: function () {
      return {}
    }
  }
</script>

<style lang="stylus" scoped>
  .padding-c {
    padding-left: 25px
  }

  .desktop {
    .item-block > .ud {
      visibility: hidden;
    }

    .item-block:hover > .ud {
      visibility: visible;
    }
  }
</style>
