<template>
  <div>
    <div class="xs-col-12 col-8">
      <div class="local-header">Пользователи</div>
      <q-input v-model="filter" label="Фильтр" @input="filterValueChanged" />

      <div v-if="users" class="local-content">
        <div :key="user.id" v-for="user in users">
          <router-link :to="`/user/${user.link}`">{{user.name}}</router-link>
        </div>
        <div v-if="users.length === 0" style="color: gray;">Нет результатов</div>
        <div v-if="users.length === 40" style="color: gray;">Выведены первые 40 результатов</div>
      </div>
      <div v-else class="xs-col-12 col-8">
        <loader-wait/>
      </div>
    </div>
  </div>
</template>

<script>

  import LoaderWait from "LoaderWait";

  export default {
    name: "UsersFromRole",
    components: {LoaderWait},
    props: {
      roleName: {
        type: String,
        required: true
      }
    },
    data: function () {
      return {
        users: null,
        filter: ""
      }
    },
    watch: {
      'roleName': 'loadRoleUsers'
    },
    methods: {
      filterValueChanged() {
        this.timeout && clearTimeout(this.timeout);
        this.timeout = setTimeout(this.loadRoleUsers, 600);
      },
      async loadRoleUsers() {
        this.users = null;
        await this.$store.dispatch("request",
          {
            url: "/Admin/AdminUserRoles/GetRoleUsers",
            data: {
              roleName: this.roleName,
              userNamePart: this.filter
            }
          })
          .then(response => {
              this.users = response.data;
            }
          );
      },
    },
    async created() {
      await this.loadRoleUsers();
    }
  }

</script>

<style lang="stylus" scoped>
  .local-header {
    background-color: #cfd8dc;
    padding: 10px;

    div {
      margin: 2px 0;
    }
  }

  .local-content {
    padding: 10px;

    div {
      margin: 3px 0;
    }
  }
</style>
