<template>
  <q-page class="my-ban-list page-padding">
    <h2 class="page-title">{{$tl("title")}}</h2>
    <div v-if="users">
      <router-link :key="user.id" class="my-ban-list__user-link block q-mb-xs"
                   :to="{name:'User', params: {link: user.link}}" v-for="user in users">{{user.name}}
      </router-link>
    </div>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
    import {Page} from 'mixins'


    export default {
        name: 'MyBanList',
        mixins: [Page],
        data() {
            return {
                users: null
            }
        },
        methods: {
            loadData() {
                this.$request(
                    this.$Api.Personal.GetMyBanList
                ).then(
                    response => {
                        this.users = response.data;
                    }
                )
            }
        },
        beforeCreate() {
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        },
        created() {
            this.title = this.$tl('title');
            this.loadData();
        }
    }

</script>

<style lang="scss">

  .my-ban-list__user-link {
    font-weight: 600;
  }

</style>
