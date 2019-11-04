<template>
  <div class="profile-roles">

    <div v-if="userRoles">

      <div class="profile-roles__user-groups">
        <div class="profile-roles__roles-title">{{$tl("roles")}}</div>

        <div class="profile-roles__roles-block q-my-md">
          <span class="profile-roles__group" v-for="role in userRoles"
                :class="'group-'+role.name.toLowerCase()">{{role.title}}</span>
        </div>
      </div>

      <div class="flex q-gutter-md">
        <q-btn class="grow" @click="add = true" no-caps color="positive" icon="fas fa-plus"
               :label="$tl('addRoleBtn')"/>

        <q-btn class="grow" @click="remove = true" no-caps color="negative" icon="fas fa-minus"
               :label="$tl('removeRoleBtn')"/>

        <q-dialog class="profile-roles__dialog-add" v-model="add">

          <q-list class="bg-white">
            <q-toolbar class="bg-positive text-white shadow-2">
              <q-toolbar-title>
                <q-icon name="fas fa-plus" class="q-mr-sm"/>
                {{$tl('addRoleBtn')}}
              </q-toolbar-title>
            </q-toolbar>
            <q-item key="role.name" clickable @click="addToRoleConfirm(role)" v-for="role in availableRoles">
              <q-item-section>
                <q-item-label class="text-blue">
                  {{role.title}}
                </q-item-label>
              </q-item-section>
            </q-item>
          </q-list>
        </q-dialog>

        <q-dialog class="profile-roles__dialog-remove" v-model="remove">
          <q-list class="bg-white">
            <q-toolbar class="bg-negative text-white shadow-2">
              <q-toolbar-title>
                <q-icon name="fas fa-minus" class="q-mr-sm"/>
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

    export default {
        name: 'ProfileRoles',
        props: {
            userId: {
                type: Number,
                required: true
            }
        },
        data() {
            return {
                allRoles: null,
                userRoles: null,
                availableRoles: null,
                add: false,
                remove: false
            }
        },
        methods: {
            addToRoleConfirm(role) {
                this.add = false;

                const title = this.$tl('addRoleConfirmTitle', role.title);
                const message = this.$tl('addRoleConfirmMessage', role.title);
                const addRoleConfirmOkBtn = this.$tl('addRoleConfirmOkBtn');
                const cancelBtn = this.$tl('addRoleConfirmCancelBtn');

                this.$q.dialog({
                    title: title,
                    message: message,
                    ok: addRoleConfirmOkBtn,
                    cancel: cancelBtn
                }).onOk(async () => {
                    await this.addToRole(role);
                })
            },
            removeFromRoleConfirm(role) {
                this.remove = false;

                const title = this.$tl('removeRoleConfirmTitle', role.title);
                const message = this.$tl('removeRoleConfirmMessage', role.title);
                const removeRoleConfirmOkBtn = this.$tl('removeRoleConfirmOkBtn');
                const cancelBtn = this.$tl('removeRoleConfirmCancelBtn');

                this.$q.dialog({
                    title: title,
                    message: message,
                    ok: removeRoleConfirmOkBtn,
                    cancel: cancelBtn
                }).onOk(() => {
                    this.removeFromRole(role)
                })
            },
            addToRole(role) {
                return this.$request(
                    this.$AdminApi.UserRolesAdmin.AddUserToRole,
                    {
                        userId: this.userId,
                        roleName: role.name
                    }
                ).then(() => {
                    this.loadUserRoles()
                });
            },
            removeFromRole(role) {
                return this.$request(
                    this.$AdminApi.UserRolesAdmin.RemoveUserFromRole,
                    {
                        userId: this.userId,
                        roleName: role.name
                    }
                ).then(() => {
                    this.loadUserRoles();
                });
            },
            loadUserRoles() {
                return this.$request(
                    this.$AdminApi.UserRolesAdmin.GetUserRoles,
                    {
                        userId: this.userId
                    }
                ).then(response => {
                        this.userRoles = response.data;
                        this.availableRoles = this.allRoles.filter(x => !this.userRoles.some(y => y.name === x.name));
                    }
                );
            },
            loadAllRoles() {
                return this.$request(this.$AdminApi.UserRolesAdmin.GetAllRoles)
                    .then(response => {
                        this.allRoles = response.data;
                    });
            }
        },
        beforeCreate() {
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        },
        async created() {
            await this.loadAllRoles();
            await this.loadUserRoles();
        }
    }

</script>

<style lang="scss">

  .profile-roles__user-groups .profile-roles__group:not(:last-child):after {
    content: ", ";
    color: initial;
    font-weight: initial;
  }

  .profile-roles__roles-block {
    background-color: #f7fbc9;
    padding: 10px;
  }

</style>
