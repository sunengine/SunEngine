<template>
  <q-page class="roles-page page-padding">
    <h2 class="page-title">
      {{title}}
    </h2>

    <div class="row">
      <div class="col-4">
        <template v-if="roles">
        <div class="roles-page__header">
          <q-icon name="fas fa-users" class="q-mr-sm"/>
          {{$tl("roles")}}
        </div>

        <div class="roles-page__list m1">
          <div class="roles-page__role" :key="role.id" v-for="role in roles">
            <router-link class="roles-page__role-link link" :to="{name: 'RoleUsers', params: {roleName: role.name}}">{{role.title}}</router-link>
          </div>
        </div>
        </template>
        <div v-else>
          <LoaderWait/>
        </div>
      </div>

      <router-view class="roles-page__router-view col-8"></router-view>
    </div>
  </q-page>
</template>

<script>

    import {Page} from 'mixins'

    export default {
        name: 'RolesPage',
        mixins: [Page],
        data() {
            return {
                roles: null,
                currentRole: null,
            }
        },
        methods: {
            loadAllRoles() {
                this.$request(
                    this.$AdminApi.UserRolesAdmin.GetAllRoles
                ).then(response => {
                    this.roles = response.data;
                });
            }
        },
        beforeCreate() {
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        },
        created() {
            this.title = this.$tl('title');
            this.loadAllRoles();
        }
    }

</script>

<style lang="scss">

  .roles-page__header {
    background-color: #cfd8dc;
    padding: 10px;
  }

  .roles-page__list {
  //  padding: 10px 0;

    .router-link-exact-active {
      background: #e1e1e1;
      border-radius: 6px;
    }
  }

  .roles-page__role {
    margin: 1px 0;
  }

  .roles-page__role-link {
    display: block;
    padding: 3px 10px;
  }

  .roles-page__router-view {
  }

  .m1 {
    padding-right: 16px;
  }

</style>
