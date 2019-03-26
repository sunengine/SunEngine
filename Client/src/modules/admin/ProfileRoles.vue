<template>
  <div>
    <div v-if="userRoles">
      <div class="user-groups">
        <div>{{$tl("roles")}}</div>
        <div class="q-my-md" style="background-color: #f7fbc9; padding: 10px;">
        <span class="one-group" v-for="role in userRoles"
              :class="'group-'+role.name.toLowerCase()">{{role.title}}</span>
        </div>
      </div>
      <div class="flex q-gutter-sm">
        <q-btn style="flex-grow: 1" @click="add = true" no-caps color="positive" icon="fas fa-plus" :label="$tl('addRoleBtn')"/>

        <q-btn style="flex-grow: 1"  @click="remove = true" no-caps color="negative" icon="fas fa-minus"  :label="$tl('removeRoleBtn')"/>

        <q-dialog  v-model="add">

          <q-list class="bg-white">
            <q-toolbar class="bg-positive text-white shadow-2">
              <q-toolbar-title>
                <q-icon name="fas fa-plus" class="q-mr-sm" />
                {{$tl('addRoleBtn')}}
              </q-toolbar-title>
            </q-toolbar>
            <q-item key="role.name"  clickable @click="addToRoleConfirm(role)" v-for="role in availableRoles">
              <q-item-section>
                <q-item-label class="text-blue">
                  {{role.title}}
                </q-item-label>
              </q-item-section>
            </q-item>
          </q-list>
        </q-dialog>

        <q-dialog  v-model="remove">
          <q-list class="bg-white">
            <q-toolbar class="bg-negative text-white shadow-2">
              <q-toolbar-title>
                <q-icon name="fas fa-minus" class="q-mr-sm" />
                {{$tl('removeRoleBtn')}}
              </q-toolbar-title>
            </q-toolbar>
            <q-item key="role.name" clickable @click="removeFromRoleConfirm(role)" v-for="role in userRoles">
              <q-item-section>
                <q-item-label>
                  {{role.title}}
                </q-item-label>
              </q-item-section>
            </q-item>
          </q-list>
        </q-dialog>

      </div>
    </div>
    <LoaderWait v-else/>
  </div>
</template>

<script>
  import LoaderWait from "LoaderWait";

  export default {
    name: "ProfileRoles",
    components: {LoaderWait},
    props: {
      userId: {
        type: Number,
        required: true
      }
    },
    i18nPrefix: "admin",
    data: function () {
      return {
        allRoles: null,
        userRoles: null,
        availableRoles: null,
        add: false,
        remove: false
      }
    },
    methods: {
      async addToRoleConfirm(role) {
        this.add = false;
        const msg = this.$tl("addRoleConfirm",role.title);
        debugger;
        const addRoleConfirmOkBtn = this.$tl("addRoleConfirmOkBtn");
        const cancelBtn = this.$t("global.btn.cancel");

        this.$q.dialog({
          //title: 'Confirm',
          message: msg,
          ok: addRoleConfirmOkBtn,
          cancel: cancelBtn
        }).onOk(async () => {
          await this.addToRole(role);
        })
      },
      async removeFromRoleConfirm(role) {
        this.remove = false;
        const msg = this.$tl("removeRoleConfirm",role.title);
        const removeRoleConfirmOkBtn = this.$tl("removeRoleConfirmOkBtn");
        const cancelBtn = this.$t("global.btn.cancel");
        this.$q.dialog({
          //title: 'Confirm',
          message: msg,
          ok: removeRoleConfirmOkBtn,
          cancel: cancelBtn
        }).onOk(async () => {
          await this.removeFromRole(role);
        })
      },
      async addToRole(role) {
        debugger;
        await this.$store.dispatch("request",
          {
            url: "/Admin/AdminUserRoles/AddUserToRole",
            data: {
              userId: this.userId,
              roleName: role.name
            }
          })
          .then(async () => {
              await this.loadUserRoles();
            }
          );
      },
      async removeFromRole(role) {
        await this.$store.dispatch("request",
          {
            url: "/Admin/AdminUserRoles/RemoveUserFromRole",
            data: {
              userId: this.userId,
              roleName: role.name
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
            url: "/Admin/AdminUserRoles/GetUserRoles",
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
            url: "/Admin/AdminUserRoles/GetAllUserRoles"
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
