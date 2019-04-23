<template>
  <div>
    <span class="item-block">
      <span v-if="notRoot" class="q-mr-sm ud">
        <q-btn :disabled="isFirst" @click="$emit('up',category)" color="info" dense size="10px" flat icon="fas fa-chevron-up"/>
        <q-btn :disabled="isLast" @click="$emit('down',category)" color="info" dense size="10px" flat icon="fas fa-chevron-down"/>
      </span>
      <span v-if="notRoot">{{category.title}}</span>
      <span v-else>{{$tl("rootCategory")}}</span>

      <span v-if="notRoot" class="q-ml-md">
        <q-btn @click="$emit('go',category.name)" icon="fas fa-arrow-right" color="info" dense size="10px" flat/>
        <q-btn @click="$emit('edit',category.id)" icon="fas fa-pencil-alt" color="info" dense size="10px" flat/>
      </span>
    </span>
    <div v-if="category.subCategories" :class="[{'padding-c': notRoot}]">
      <category-item :category="sub" :isFirst="index === 0" :isLast="index === lastIndex"
                     :key="sub.id" v-for="(sub,index) in category.subCategories" v-on="$listeners"/>
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
    i18nPrefix: "Admin",
    computed: {
      notRoot() {
        return this.category.name !== 'Root'
      },
      lastIndex() {
        return this.category.subCategories.length - 1;
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
