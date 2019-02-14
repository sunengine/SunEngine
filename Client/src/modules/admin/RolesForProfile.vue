<template>
  <div>
    <div v-if="userRoles">
      <div class="user-groups">
        <div>Группы пользователя:</div>
        <div style="margin: 7px 0; background-color: #f7fbc9; padding: 10px;">
        <span class="one-group" v-for="role in userRoles"
              :class="'group-'+role.name.toLowerCase()">{{role.title}}</span>
        </div>
      </div>
      <div>
        <q-select v-if="availableRoles"
                  v-model="roleToAdd"
                  float-label="Добавить группу"
                  :options="addOptions" @input="addToRoleConfirm"
        />
        <q-select v-if="userRoles"
                  v-model="roleToRemove"
                  float-label="Удалить группу"
                  :options="removeOptions" @input="removeFromRoleConfirm"
        />
        <br/>
      </div>
    </div>
    <LoaderWait v-else/>
  </div>
</template>

<script>
  import LoaderWait from "LoaderWait";

  export default {
    name: "RolesForProfile",
    components: {LoaderWait},
    props: {
      userId: {
        type: Number,
        required: true
      }
    },
    data: function () {
      return {
        allRoles: null,
        userRoles: null,
        availableRoles: null,
        roleToAdd: null,
        roleToRemove: null
      }
    },
    computed: {
      addOptions() {
        return this.availableRoles?.map(x => {
          return {
            label: x.title, value: x.name
          }
        });
      },
      removeOptions() {
        return this.userRoles?.map(x => {
          return {
            label: x.title, value: x.name
          }
        });
      }
    },
    methods: {
      async addToRoleConfirm(roleName) {
        let role = this.allRoles.find(x => x.name === roleName);

        this.$q.dialog({
          //title: 'Confirm',
          message: `Добавить группу '${role.title}'?`,
          ok: 'Добавить',
          cancel: 'Отмена'
        }).then(async () => {
          await this.addToRole(roleName);
          this.roleToAdd = null;
        }).catch(() => {
          this.roleToAdd = null;
        })
      },
      async removeFromRoleConfirm(roleName) {
        let role = this.allRoles.find(x => x.name === roleName);

        this.$q.dialog({
          //title: 'Confirm',
          message: `Удалить группу '${role.title}'?`,
          ok: 'Удалить',
          cancel: 'Отмена'
        }).then(async () => {
          await this.removeFromRole(roleName);
          this.roleToRemove = null;
        }).catch(() => {
          this.roleToRemove = null;
        })
      },
      async addToRole(roleName) {
        await this.$store.dispatch("request",
          {
            url: "/UserRoles/AddUserToRole",
            data: {
              userId: this.userId,
              roleName: roleName
            }
          })
          .then(async () => {
              await this.loadUserRoles();
            }
          );
      },
      async removeFromRole(roleName) {
        await this.$store.dispatch("request",
          {
            url: "/UserRoles/RemoveUserFromRole",
            data: {
              userId: this.userId,
              roleName: roleName
            }
          })
          .then(async () => {
              await this.loadUserRoles();
            }
          );
      },
      async loadUserRoles() {
        await this.$store.dispatch("request",
          {
            url: "/UserRoles/GetUserRoles",
            data: {
              userId: this.userId
            }
          })
          .then(response => {
              this.userRoles = response.data;
              this.availableRoles = this.allRoles.filter(x => !this.userRoles.some(y => y.name === x.name));
            }
          );
      },
      async loadAllRoles() {
        await this.$store.dispatch("request",
          {
            url: "/UserRoles/GetAllUserRoles"
          })
          .then(response => {
              this.allRoles = response.data;
            }
          );
      }
    },
    async created() {
      await this.loadAllRoles();
      await this.loadUserRoles();
    }
  }

</script>

<style lang="stylus" scoped>

  .user-groups .one-group:not(:last-child):after {
    content: ", ";
    color: initial;
    font-weight: initial;
  }

</style>
