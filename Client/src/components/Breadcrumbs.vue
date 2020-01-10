<template>
    <div>
        <q-breadcrumbs v-if="category" class="breadcrumbs text-grey" active-color="purple">
            <q-breadcrumbs-el :key="cat.id" :exact="true" v-for="cat of breadCrumbsCategories" :to="cat.route"
                              :label="cat.title"/>
        </q-breadcrumbs>
    </div>
</template>

<script>

    export default {
        name: 'Breadcrumbs',
        props: {
            category: {
                type: Object,
                required: true
            }
        },
        computed: {
            breadCrumbsCategories() {
                if (!this.category)
                    return null;

                let rez = [];
                rez.push(this.category);
                let current = this.category.parent;
                while (current) {
                    if(current.showInBreadcrumbs)
                        rez.push(current);
                    current = current.parent;
                }
                rez = rez.reverse();
                rez[0] = {
                    title: this.$tl('home'),
                    id: 0,
                    route: {
                        name: 'Home'
                    }
                };
                rez.push({});
                return rez;
            },
        }
    }

</script>
