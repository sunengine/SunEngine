<template>
  <q-item class="page-padding" :to="to">
    <q-item-section>
      <q-item-label class="my-header">{{article.title}}</q-item-label>
      <q-item-label v-if="description" class="info-block" caption>
        <div v-html="description">
        </div>
      </q-item-label>
      <q-item-label class="info-block" caption>
       <span>
        <q-icon name="far fa-user"/>
        {{article.authorName}} &nbsp;
          </span>
        <span>
        <q-icon name="far fa-clock"/>
          {{$formatDate(this.article.publishDate)}}
        </span>
        <span v-if="article.commentsCount > 0">
          <q-icon name="far fa-comment"/>
          {{article.commentsCount}}
        </span>
      </q-item-label>
    </q-item-section>
  </q-item>
</template>

<script>

  export default {
    name: "Article",
    props: {
      article: Object,
      required: true
    },
    computed: {
      description() {
        return this.article.description?.replace(/\n/g, "<br/>");
      },
      to() {
        return this.category.getMaterialRoute(this.article.name ?? this.article.id);
      },
      category() {
        return this.$store.getters.getCategory(this.article.categoryName);
      }
    }
  }
</script>

<style lang="stylus" scoped>


</style>
