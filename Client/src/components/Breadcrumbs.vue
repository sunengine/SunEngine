<template>
    <div class="breadcrumbs">
        <q-breadcrumbs v-if="category" class="text-grey" active-color="purple">
            <q-breadcrumbs-el :key="cat.id" :exact="true" v-for="cat of breadCrumbsCategories"
                              :class="{'breadcrumbs--no-route' : !cat.route}" :to="cat.route"
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
                    if (current.showInBreadcrumbs)
                        rez.push(current);
                    current = current.parent;
                }
                rez = rez.reverse();

                if (rez[0].name === 'Root') {
                    rez[0] = {
                        title: this.$tl('home'),
                        id: 0,
                        route: {
                            name: 'Home'
                        }
                    };
                }

                rez.push({});
                return rez;
            },
        }
    }

</script>

<style lang="scss">

    .breadcrumbs--no-route {
        color: $grey-6;
    }

    .breadcrumbs .q-breadcrumbs .q-breadcrumbs__separator:nth-last-of-type(-n+2) {
        display: none;
    }

</style>
