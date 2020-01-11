<template>
    <q-page class="blog-multi-cat-page">
        <div class="page-title-block page-padding">
            <h1 class="page-title">
                {{title}}
            </h1>
            <q-btn v-if="canPost" no-caps class="post-btn" :label="addButtonLabel" icon="fas fa-plus"
                   @click="$router.push( {name:'CreateMaterial',params:{categoriesNames: component.settings.CategoriesNames}})"/>
        </div>

        <div class="page-padding">
            <div v-if="component.settings.SubTitle" class="page-sub-title">
                {{component.settings.SubTitle}}
            </div>
        </div>

        <div v-if="component.settings.Header" class="q-mb-lg text-grey-9"
             v-html="component.settings.Header"></div>

        <PostsMultiCat :componentName="componentName"/>

    </q-page>
</template>

<script>
    import {Page} from 'mixins'


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
                if (!this.component.settings.CategoriesNames)
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
            this.$options.centered = true;
            this.$options.components.PostsMultiCat = require('sun').PostsMultiCat;
        },
        created() {
            this.title = this.component.settings.Title;
        }
    }

</script>


<style lang="scss">

    .blog-multi-cat-page {

    }

</style>
