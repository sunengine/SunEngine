<template>
  <div class="row">
    <div v-if="groups" class="xs-col-12 col-4">
      <div class="local-header">Группы</div>
      <div class="local-content">
        <div :key="group.id" v-for="group in groups">
          <router-link :to="{name: 'GroupUsers', params: {groupName: group.name}}">{{group.title}}</router-link>
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
    name: "Groups",
    components: {LoaderWait},
    data: function () {
      return {
        groups: null,
        currentGroup: null,
      }
    },
    methods: {
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
