﻿<template>
  <div class="activity">
    <q-item :to="route" class="page-padding">
      <q-item-section class="activity__icon" avatar>
        <img class="activity__avatar" :src="$avatarPath(activity.authorAvatar)"/>
      </q-item-section>
      <q-item-section class="activity__icon" avatar>
        <q-icon color="grey-6" :name="activity.commentId ? 'far fa-comment' : 'far fa-file-alt'"/>
      </q-item-section>
      <q-item-section >
        <q-item-label class="activity__title">
          <span class="q-mr-sm">{{activity.commentId ? $tl("comment") : $tl("material") }}</span><span>{{activity.title}}</span>
        </q-item-label>
        <q-item-label v-if="activity.description" class="activity__description text-grey-7">
          {{activity.description}}
        </q-item-label>
        <q-item-label class="activity__footer material-header-info-block" caption>
          <span>
               {{activity.authorName}}
             </span>
          <span>
               <q-icon name="far fa-folder"/>
               {{category.title}}
             </span>
          <span>
               <q-icon name="far fa-clock"/>
               {{$formatDate(activity.publishDate)}}
             </span>
        </q-item-label>
      </q-item-section>
    </q-item>
  </div>
</template>

<script>

    export default {
        name: 'Activity',
        props: {
            activity: {
                type: Object,
                required: true
            }
        },
        computed: {
            route() {
                let route = this.category.getMaterialRoute(this.activity.materialId);
                if (this.activity.commentId)
                    route.hash = `#comment-${this.activity.commentId}`;
                return route;
            },
            category() {
                return this.$store.getters.getCategory(this.activity.categoryName);
            }
        }
    }

</script>

<style lang="scss">

  .activity__icon {
    min-width: 10px;
    padding-right: 10px;
  }

  .activity__avatar {
    width: 35px;
    height: 35px;
    border-radius: 50%;
  }

  .activity__title {
    margin-bottom: 4px;
  }

  .activity__description {
    margin-bottom: 4px;
    font-size: 0.9em;
  }

</style>
