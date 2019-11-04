<template>
  <div class="role-users">
    <div class="xs-col-12 col-8">
      <div class="role-users__header">
        <q-icon name="fas fa-user" class="q-mr-sm"/>
        {{$tl("users")}}
      </div>

      <q-input class="role-users__filter q-my-sm" outlined dense v-model="filter" :label="$tl('filter')"
               @input="filterValueChanged">
        <template v-slot:prepend>
          <q-icon name="fas fa-search" size="0.75em"/>
        </template>
      </q-input>

      <div v-if="users" class="role-users__list">
        <div class="role-users__user" :key="user.id" v-for="user in users">
          <router-link class="role-users__user-link" :to="`/user/${user.link}`">{{user.name}}</router-link>
        </div>
        <div v-if="users.length === 0" class="text-grey">{{$tl("noResults")}}</div>
        <div v-if="users.length === maxUsersTake" class="text-grey">{{$tl("filterLimitReached",maxUsersTake)}}</div>
      </div>

      <div v-else class="xs-col-12 col-8">
        <LoaderWait/>
      </div>

    </div>
  </div>
</template>

<script>

    export default {
        name: 'RoleUsers',
        props: {
            roleName: {
                type: String,
                required: true
            }
        },
        data() {
            return {
                users: null,
                filter: ''
            }
        },
        watch: {
            'roleName': 'loadRoleUsers'
        },
        maxUsersTake: null,
        methods: {
            filterValueChanged() {
                this.timeout && clearTimeout(this.timeout);
                this.timeout = setTimeout(this.loadRoleUsers, 600);
            },
            async loadRoleUsers() {
                this.users = null;
                await this.$request(
                    this.$AdminApi.UserRolesAdmin.GetUserRoles,
                    {
                        roleName: this.roleName,
                        userNamePart: this.filter
                    }).then(response => {
                        this.users = response.data;
                    }
                );
            }
        },
        beforeCreate() {
            this.maxUsersTake = config.Misc.AdminRoleUsersMaxUsersTake;
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        },
        async created() {
            await this.loadRoleUsers();
        }
    }

</script>

<style lang="scss">


  .role-users__header {
    background-color: #cfd8dc;
    padding: 10px;
  }

  .role-users__list {
    padding: 0 10px;
  }

  .role-users_user-div {
    margin: 3px 0;
  }

</style>
