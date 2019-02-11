<template>

  <div v-if="currentGroup" class="xs-col-12 col-8">
    <div class="local-header">Пользователи</div>
    <div class="local-content">
      <div :key="user.id" v-for="user in users">
        <router-link :to="`/user/${user.link}`">{{user.name}}</router-link>
      </div>
    </div>
  </div>
  <div v-if="startedLoadGroupUsers && !currentGroup" class="xs-col-12 col-8">
    <loader-wait/>
  </div>

</template>

<script>

  import LoaderWait from "LoaderWait";

  export default {
    name: "GroupUsers",
    components: {LoaderWait},
    data: function () {
      return {
        groups: null,
        users: null,
        currentGroup: null,
        startedLoadGroupUsers: false
      }
    },
    methods: {
      async loadGroupUsers(groupName) {
        this.startedLoadGroupUsers = true;
        this.currentGroup = null;
        await this.$store.dispatch("request",
          {
            url: "/GroupsUsers/GetGroupUsers",
            data: {
              groupName: groupName
            }
          })
          .then(response => {
              this.currentGroup = this.groups.find(x => x.name === groupName);
              this.users = response.data;
              this.startedLoadGroupUsers = false;
            }
          );
      },
      async loadAllGroups() {
        await this.$store.dispatch("request",
          {
            url: "/GroupsUsers/GetAllUserGroups"
          })
          .then(response => {
              this.groups = response.data;
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
      margin: 2px 0;
    }
  }
</style>
