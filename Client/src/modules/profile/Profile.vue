<template>
  <q-page class="profile page-padding page-padding-top">
    <div class="container" v-if="user">
      <div class="img flex column">
        <img width="300" height="300" :src="$imagePath(user.photo)"/>
        <div v-if="messageButtons" class="private-messages-block flex q-mt-sm">
          <q-btn no-caps class="shadow-1" color="lime-4" style="flex-grow: 1" :disable="!canPrivateMessage"
                 :to="{path: '/SendPrivateMessage'.toLowerCase(), query: {userId: user.id, userName: user.name }}"
                 dense icon="far fa-envelope" :label="$tl('sendPrivateMessageBtn')"/>
          <q-btn :color="!user.iBannedHim ? 'lime-4' : 'negative'" class="shadow-1 q-ml-sm" dense
                 style="padding-left:10px !important; padding-right: 10px; !important" v-if="!user.noBannable"
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
        <h3>{{user.name}}</h3>
        <div class="q-mb-lg" v-html="user.information"></div>

        <div class="footer-info">
          <div class="registered">

            {{$tl("registered")}}: {{$formatDateOnly(user.registeredDate)}}
          </div>
          <div class="visits">
            <q-icon name="far fa-eye" class="q-mr-sm"/>
            {{user.profileVisitsCount}}
          </div>
        </div>

        <q-expansion-item class="roles overflow-hidden" v-if="canEditRoles" @show="showRolesAdmin" icon="fas fa-cog"
                          :label="$tl('roles')" style="border-radius: 12px; margin-top: 30px; border: 1px solid silver"
                          header-style="background-color: #e4e4e4">
          <ProfileRoles class="q-pa-md" :userId="user.id" v-if="isShowRolesAdmin"/>
        </q-expansion-item>
      </div>
    </div>
    <loader-wait v-else/>
  </q-page>
</template>

<script>
  import {Page} from 'sun'


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
      async ban() {
        await this.$store.dispatch('request',
          {
            url: '/Profile/BanUser',
            data: {
              userId: this.user.id
            }
          }).then(async response => {
          await this.loadData();
          const msg = this.$tl('banNotify', [this.user.name]);
          this.$successNotify(msg);
        });
      },
      async unBan() {
        await this.$store.dispatch('request',
          {
            url: '/Profile/UnBanUser',
            data: {
              userId: this.user.id
            }
          }).then(async response => {
          await this.loadData();
          const msg = this.$tl('unBanNotify', [this.user.name]);
          this.$successNotify(msg);
        });
      },
      async loadData() {
        await this.$store.dispatch('request',
          {
            url: '/Profile/GetProfile',
            data: {
              link: this.link
            }
          }).then(response => {
          this.user = response.data;
          this.title = this.user.name;
        });
      }
    },
    beforeCreate() {
      this.$options.components.ProfileRoles = require('sun').ProfileRoles;
      this.$options.components.LoaderWait = require('sun').LoaderWait;
    },
    async created() {
      await this.loadData();
    }
  }

</script>

<style lang="stylus">

  .profile {
    h3 {
      margin: 0 0 14px 0;
      font-size: 2.2rem;
    }

    .private-messages-block {
      padding-right: 2px;
      padding-left: 2px;
      align-items: center;
      width: 300px;
    }

    .container {
      display: flex;

      .img {
        margin-right: 20px;
      }
    }

    .footer-info {
      display: flex;
      font-style: italic;
      color: $grey-8;

      div {
        display: flex;
        align-items: center;
      }

      .registered {
        flex-grow: 1;
      }
    }

    @media (max-width: 900px) {
      .f1 {
        flex-direction: column;

        .img {
          align-content: center;
        }
      }
    }
  }

</style>
