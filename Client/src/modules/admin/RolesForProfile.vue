<template>
  <div>
    <div v-if="userRoles">
      Группы пользователя: <span v-for="role in userRoles" :class="'group-'+role.name.toLowerCase()">{{role.title}}</span>
    </div>
    <loader-wait v-else />
  </div>
</template>

<script>
  import LoaderWait from "../../components/LoaderWait";
  export default {
    name: "RolesForProfile",
    components: {LoaderWait},
    props: {
      userId: {
        type: Number,
        required: true
      }
    },
    data: function() {
      return {
        allRoles: null,
        userRoles: null,
        availableRoles: null
      }
    },
    methods: {
      async loadData() {

      }
    },
    async created() {
      await this.loadData();
    }
  }
</script>

<style scoped>

</style>
