<template>
  <div class="row">
    <div v-if="roles" class="xs-col-12 col-4">
      <div class="local-header">Группы</div>
      <div class="local-content">
        <div :key="role.id" v-for="role in roles">
          <router-link :to="{name: 'UsersFromRole', params: {roleName: role.name}}">{{role.title}}</router-link>
        </div>
      </div>
    </div>
    <div v-else class="xs-col-12 col-4">
      <loader-wait/>
    </div>
    <router-view style="margin-left: 6px;"></router-view>
  </div>
</template>

<script>

  import LoaderWait from "LoaderWait";

  export default {
    name: "RolesUsers",
    components: {LoaderWait},
    data: function () {
      return {
        roles: null,
        currentRole: null,
      }
    },
    methods: {
      async loadAllGroups() {
        await this.$store.dispatch("request",
          {
            url: "/UserRoles/GetAllUserRoles"
          })
          .then(response => {
              this.roles = response.data;
            }
          );
      }
    },
    async created() {
      await this.loadAllGroups();
    }
  }

</script>

<style lang="stylus" scoped>
  @import '~variables'

  .local-header {
    background-color: #cfd8dc;
    padding: 10px;

    a {
      padding: 2px;
    }
  }

  .local-content {
    padding: 10px 0;

    div {
      margin: 1px 0;

      a {
        display: block;
        padding: 3px 10px;
      }
    }
  }

  .router-link-exact-active {
    background: #e1e1e1;
    border-radius: 6px;
  }
</style>
