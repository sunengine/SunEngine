<template>
  <div class="category-item">
    <span class="item-block">
      <span v-if="notRoot" class="q-mr-sm ud">
        <q-btn :disabled="isFirst" @click="$emit('up',category)" color="positive" dense size="10px" flat
               icon="fas fa-chevron-up"/>
        <q-btn :disabled="isLast" @click="$emit('down',category)" color="positive" dense size="10px" flat
               icon="fas fa-chevron-down"/>
      </span>
      <span v-if="notRoot">
          <router-link v-if="category.isMaterialsContainer"
                       :to="{name:'CatView', params: {categoryName: category.name}}">{{category.title}}</router-link>
        <template v-else>
          {{category.title}}
        </template>
      </span>
      <span v-else>{{$tl("rootCategory")}}</span>
      <span v-if="notRoot" class="q-ml-md">
        <q-btn :to="{name: 'EditCategory', params: {categoryId:category.id}}" icon="fas fa-wrench" color="info" dense
               size="10px" flat/>
        <q-btn :to="{name: 'CreateCategory', params: {parentCategoryId: category.id}}" icon="fas fa-folder-plus"
               color="info" dense size="10px" flat/>
        <q-btn :disabled="!route" :to="route" icon="fas fa-arrow-right" color="info" dense size="10px" flat/>
      </span>

      <span v-if="category.materialsCount" class="text-grey-8 q-ml-md"> <q-icon color="grey-5" name="far fa-file-alt"/> {{category.materialsCount}}</span>
    </span>
    <div v-if="category.subCategories" :class="[{'padding-shift': notRoot}]">
      <category-item :category="sub" :isFirst="index === 0" :isLast="index === lastIndex"
                     :key="sub.id" v-for="(sub,index) in category.subCategories" v-on="$listeners"/>
    </div>
  </div>
</template>

<script>

    export default {
        name: 'CategoryItem',
        props: {
            category: {
                type: Object,
                required: true
            },
            isFirst: Boolean,
            isLast: Boolean
        },
        computed: {
            route() {
                return this.$store.getters.getCategory(this.category.name)?.getRoute()
            },
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

<style lang="stylus">

  .category-item {
    .padding-shift {
      padding-left: 25px
    }

    .q-btn:disabled {
      filter: grayscale(1);
    }

    .desktop {
      .item-block > .ud {
        visibility: hidden;
      }

      .item-block:hover > .ud {
        visibility: visible;
      }
    }
  }

</style>
