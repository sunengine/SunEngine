<template>

</template>

<script>

    export default {
        name: "ArticlesList",
        props: {
            articles: {
                type: Object,
                required: true
            }
        },
        beforeCreate() {
            this.$options.components.Article = require('sun').Article;
        }
    }
</script>

<style lang="stylus">

</style>
