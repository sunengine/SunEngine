<template>
  <q-item :to="to"
          :class="['page-padding', {'mat-hidden': article.isHidden}, {'mat-deleted': article.isDeleted}]">
    <q-item-section>
      <q-item-label class="my-header">
        <q-icon name="far fa-eye-slash" v-if="article.isHidden" class="q-mr-sm"/>
        <q-icon name="fas fa-trash" color="maroon" class="q-mr-sm" v-if="article.isDeleted"/>
        {{article.title}}
        <span class="q-ml-sm" v-if="article.isHidden">
          [{{$tl("hidden")}}]
        </span>
        <span class="q-ml-sm" v-if="article.isDeleted">
          [{{$tl("deleted")}}]
        </span>
      </q-item-label>
      <q-item-label v-if="description" class="info-block" caption>
        <div v-html="description">
        </div>
      </q-item-label>
      <q-item-label class="info-block" caption>
       <span>
        <q-icon name="far fa-user"/>
          {{article.authorName}}
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
      category() {
        return this.$store.getters.getCategory(this.article.categoryName);
      },
      to() {
        return this.category.getMaterialRoute(this.article.name ?? this.article.id);
      }
    }
  }
</script>

<style lang="stylus" scoped>


</style>
