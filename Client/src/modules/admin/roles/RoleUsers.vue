<template>
  <div class="role-users ">
    <div class="xs-col-12 col-8">
      <div class="local-header">
        <q-icon name="fas fa-user" class="q-mr-sm"/>
        {{$tl("users")}}
      </div>
      <q-input outlined dense class="q-my-sm" v-model="filter" :label="$tl('filter')" @input="filterValueChanged">
        <template v-slot:prepend>
          <q-icon name="fas fa-search" size="0.75em"/>
        </template>
      </q-input>

      <div v-if="users" class="local-content">
        <div :key="user.id" v-for="user in users">
          <router-link :to="`/user/${user.link}`">{{user.name}}</router-link>
        </div>
        <div v-if="users.length === 0" style="color: gray;">{{$tl("noResults")}}</div>
        <div v-if="users.length === maxUsersTake" style="color: gray;">{{$tl("filterLimitReached",maxUsersTake)}}</div>
      </div>
      <div v-else class="xs-col-12 col-8">
        <loader-wait/>
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
        await this.$store.dispatch('request',
          {
            url: '/Admin/UserRolesAdmin/GetRoleUsers',
            data: {
              roleName: this.roleName,
              userNamePart: this.filter
            }
          })
          .then(response => {
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

<style lang="stylus">

  .role-users {
    .local-header {
      background-color: #cfd8dc;
      padding: 10px;

      div {
        margin: 2px 0;
      }
    }

    .local-content {
      padding: 0 10px;

      div {
        margin: 3px 0;
      }
    }
  }

</style>
