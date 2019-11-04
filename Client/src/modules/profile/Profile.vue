<template>
  <q-page class="profile page-padding page-padding-top">
    <div class="profile__container" v-if="user">
      <div class="profile__img-block flex column">
        <img class="profile__photo" width="300" height="300" :src="$imagePath(user.photo)"/>
        <div v-if="messageButtons" class="profile__private-messages-block flex q-mt-sm">
          <q-btn no-caps class="shadow-1 grow" color="lime-4" :disable="!canPrivateMessage"
                 :to="{path: '/SendPrivateMessage'.toLowerCase(), query: {userId: user.id, userName: user.name }}"
                 dense icon="far fa-envelope" :label="$tl('sendPrivateMessageBtn')"/>
          <q-btn :color="!user.iBannedHim ? 'lime-4' : 'negative'" class="profile__bun-btn shadow-1 q-ml-sm" dense

                 v-if="!user.noBannable"
                 icon="fas fa-ellipsis-v">
            <q-menu>
              <q-btn no-caps v-close-popup color="negative" v-close-overlay v-if="!user.iBannedHim" @click="ban"
                     icon="fas fa-ban" :label="$tl('banBtn')"/>
              <q-btn no-caps v-close-popup color="positive" v-close-overlay v-else @click="unBan" icon="fas fa-smile"
                     :label="$tl('unBanBtn')"/>
            </q-menu>
          </q-btn>
        </div>
      </div>
      <div>
        <h3 class="profile__user-name">{{user.name}}</h3>
        <div class="q-mb-lg" v-html="user.information"></div>

        <div class="profile__footer-info">
          <div class="profile__registered grow">

            {{$tl("registered")}}: {{$formatDateOnly(user.registeredDate)}}
          </div>
          <div class="profile__visits">
            <q-icon name="far fa-eye" class="q-mr-sm"/>
            {{user.profileVisitsCount}}
          </div>
        </div>

        <q-expansion-item class="profile__expansion-item-roles overflow-hidden" v-if="canEditRoles"
                          @show="showRolesAdmin"
                          icon="fas fa-cog"
                          :label="$tl('roles')"
                          header-style="background-color: #e4e4e4">
          <ProfileRoles class="profile__roles q-pa-md" :userId="user.id" v-if="isShowRolesAdmin"/>
        </q-expansion-item>
      </div>
    </div>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
    import {Page} from 'mixins'


    export default {
        name: 'Profile',
        mixins: [Page],
        props: {
            link: {
                type: String,
                required: true
            }
        },
        data() {
            return {
                user: null,
                isShowRolesAdmin: false
            }
        },
        computed: {
            canPrivateMessage() {
                const from = this.$store?.state?.auth?.user;
                if (!from) return false;
                if (this.user.heBannedMe || this.user.iBannedHim) return false;
                return from.id !== this.user?.id;
            },
            messageButtons() {
                const from = this.$store?.state?.auth?.user;
                if (!from) return false;
                return from.id !== this.user?.id;
            },
            canEditRoles() {
                return this.$store?.state?.auth?.roles?.some(x => x === 'Admin');
            }
        },
        watch: {
            'link': 'loadData'
        },
        methods: {
            showRolesAdmin() {
                this.isShowRolesAdmin = true;
            },
            ban() {
                this.$request(
                    this.$Api.Profile.BanUser,
                    {
                        userId: this.user.id
                    }
                ).then(async response => {
                    await this.loadData();
                    const msg = this.$tl('banNotify', [this.user.name]);
                    this.$successNotify(msg);
                });
            },
            unBan() {
                this.$request(
                    this.$Api.Profile.UnBanUser,
                    {
                        userId: this.user.id
                    }
                ).then(async response => {
                    await this.loadData();
                    const msg = this.$tl('unBanNotify', [this.user.name]);
                    this.$successNotify(msg);
                });
            },
            async loadData() {
                await this.$request(
                    this.$Api.Profile.GetProfile,
                    {
                        link: this.link
                    }
                ).then(response => {
                    this.user = response.data;
                    this.title = this.user.name;
                });
            }
        },
        beforeCreate() {
            this.$options.components.ProfileRoles = require('sun').ProfileRoles;
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        },
        created() {
            this.loadData();
        }
    }

</script>

<style lang="scss">

  .profile__container {
    display: flex;
  }

  .profile__img-block {
    margin-right: 20px;
  }

  .profile__user-name {
    margin: 0 0 14px 0;
    font-size: 2.2rem;
  }

  .profile__private-messages-block {
    padding-right: 2px;
    padding-left: 2px;
    align-items: center;
    width: 300px;
  }

  .profile__bun-btn {
    padding-left: 10px !important;
    padding-right: 10px !important;
  }

  .profile__footer-info {
    display: flex;
    font-style: italic;
    color: $grey-8;

    div {
      display: flex;
      align-items: center;
    }
  }

  .profile__expansion-item-roles {
    border-radius: 12px;
    margin-top: 30px;
    border: 1px solid silver
  }

  @media (max-width: 900px) {
    .profile__container {
      flex-direction: column;
    }

    .profile__img-block {
      align-content: center;
    }
  }

</style>
