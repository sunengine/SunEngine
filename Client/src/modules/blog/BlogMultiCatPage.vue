<template>
  <q-page class="blog-multi-cat-page">
    <div class="header-with-button page-padding">
      <h2 class="q-title">
        {{title}}
      </h2>
      <q-btn v-if="canPost" no-caps class="post-btn"
             @click="$router.push( {name:'CreateMaterial',params:{categoriesNames: component.settings.CategoriesNames}})"
             :label="addButtonLabel" icon="fas fa-plus"/>
    </div>

    <div v-if="component.settings.SubTitle" class="page-padding q-mb-lg text-grey-9" style="margin-top: -14px">
      {{component.settings.SubTitle}}
    </div>

    <div v-if="component.settings.Header" class="q-mb-lg text-grey-9" style="margin-top: -14px"
         v-html="component.settings.Header"></div>

    <PostsMultiCat :componentName="componentName" />

  </q-page>
</template>

<script>
    import {Page} from 'sun'


    export default {
        name: 'BlogMultiCatPage',
        mixins: [Page],
        props: {
            componentName: {
                type: String,
                required: true,
            }
        },
        watch: {
            'componentName': 'loadData',
            '$route': 'loadData',
        },
        computed: {
            canPost() {
                if(!this.component.settings.CategoriesNames)
                    return false;

                if (this.component.settings.RolesCanAdd) {
                    const rolesCanAdd = this.component.settings.RolesCanAdd.split(",");
                    if (!this.$store.state.auth.roles.some(x => rolesCanAdd.some(y => y === x)))
                        return false;
                }

                let categories = this.component.settings.CategoriesNames.split(',').map(x => x.trim());
                for (let catName of categories) {
                    let cat = this.$store.getters.getCategory(catName);
                    if (cat?.canSomeChildrenWriteMaterial)
                        return true;
                }
                return false;
            },
            addButtonLabel() {
                return this.component.settings.AddButtonLabel ?? this.$tl("addButtonLabel");
            },
            currentPage() {
                return this.$route.query?.page ?? 1;
            },
            component() {
                return this.$store.getters.getComponent(this.componentName);
            }
        },
        methods: {
            pageChanges(newPage) {
                if (this.currentPage !== newPage) {
                    let req = {path: this.$route.path};
                    if (newPage !== 1)
                        req.query = {page: newPage};

                    this.$router.push(req);
                }
            }
        },
        beforeCreate() {
            this.$options.components.PostsMultiCat = require('sun').PostsMultiCat;
        },
        async created() {
            this.title = this.component.settings.Title;
        }
    }

</script>


<style lang="stylus">

  .blog-multi-cat-page {

  }

</style>
