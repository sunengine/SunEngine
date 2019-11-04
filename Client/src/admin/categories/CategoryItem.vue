<template>
  <div class="category-item">
    <span class="category-item__item-block">
      <span v-if="notRoot" class="q-mr-sm category-item__up-down">
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
        <q-btn class="category-item__btn-edit" :to="{name: 'EditCategory', params: {categoryId:category.id}}" icon="fas fa-wrench" color="info" dense
               size="10px" flat/>
        <q-btn class="category-item__btn-create" :to="{name: 'CreateCategory', params: {parentCategoryId: category.id}}" icon="fas fa-folder-plus"
               color="info" dense size="10px" flat/>
        <q-btn class="category-item__btn-to-route" :disabled="!route" :to="route" icon="fas fa-arrow-right" color="info" dense size="10px" flat/>
      </span>

      <span class="category-item__materails-count text-grey-8 q-ml-md" v-if="category.materialsCount"> <q-icon color="grey-5" name="far fa-file-alt"/> {{category.materialsCount}}</span>
    </span>
    <div  class="category-item__sub-categories-block" v-if="category.subCategories" :class="[{'padding-shift': notRoot}]">
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

<style lang="scss">

  .category-item {
    .padding-shift {
      padding-left: 25px
    }

    .q-btn:disabled {
      filter: grayscale(1);
    }

    .desktop {
      .category-item__item-block > .category-item__up-down {
        visibility: hidden;
      }

      .category-item__item-block:hover > .category-item__up-down {
        visibility: visible;
      }
    }
  }

</style>
