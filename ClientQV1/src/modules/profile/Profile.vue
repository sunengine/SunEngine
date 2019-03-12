<template>
  <q-page class="page-padding page-padding-top">
    <div class="f1" v-if="user">
      <div class="img flex column">
        <img width="300" height="300" :src="$imagePath(user.photo)"/>
        <div v-if="messageButtons" class="private-messages-block flex q-mt-sm"
             style="padding-right: 2px; padding-left: 2px; align-items: center; width: 100%">
          <q-btn no-caps class="shadow-1" color="lime-4" style="flex-grow: 1" :disable="!canPrivateMessage"
                 :to="{path: '/SendPrivateMessage'.toLowerCase(), query: {userId: user.id, userName: user.name }}"
                 dense icon="far fa-envelope" :label="$tl('sendPrivateMessageBtn')"/>
          <q-btn :color="!user.iBannedHim ? 'lime-4' : 'negative'" class="shadow-1 q-ml-sm" dense
                 style="padding-left:10px !important; padding-right: 10px; !important" v-if="!user.noBannable"
                 icon="fas fa-ellipsis-v">
            <q-menu>
              <q-btn no-caps v-close-menu color="negative"  v-close-overlay v-if="!user.iBannedHim" @click="ban"
                     icon="fas fa-ban" :label="$tl('banBtn')"/>
              <q-btn no-caps v-close-menu color="positive"  v-close-overlay v-else @click="unBan" icon="fas fa-smile"
                     :label="$tl('unBanBtn')"/>
            </q-menu>
          </q-btn>
        </div>
      </div>
      <div>
        <h4>{{user.name}}</h4>
        <div v-html="user.information"></div>
         <q-expansion-item  v-if="canEditRoles" @show="showRolesAdmin" icon="fas fa-cog" label="Группы" style="border-radius: 12px; margin-top: 30px; border: 1px solid silver" header-style="background-color: #e4e4e4">
           <RolesForProfile class="q-pa-md"  :userId="user.id" v-if="isShowRolesAdmin" />
         </q-expansion-item>
      </div>
    </div>
    <loader-wait v-else/>
  </q-page>
</template>

<script>
  import LoaderWait from "LoaderWait";
  import Page from "Page";
  import RolesForProfile from "admin/RolesForProfile";

  export default {
    name: "Profile",
    mixins: [Page],
    components: {RolesForProfile, LoaderWait},
    props: {
      link: {
        type: String,
        required: true
      }
    },
    data: function () {
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
        return this.$store?.state?.auth?.roles?.some(x => x === "Admin");
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
        await this.$store.dispatch("request",
          {
            url: "/Profile/BanUser",
            data: {
              userId: this.user.id
            }
          }).then(async response => {
          await this.loadData();

          const msg = this.$tl("banNotify",[this.user.name]);
          this.$q.notify({
            message: msg,
            timeout: 5000,
            color: 'info',
            position: 'top'
          });
        }).catch(error => {
          console.log("error", error);
        });
      },
      async unBan() {
        await this.$store.dispatch("request",
          {
            url: "/Profile/UnBanUser",
            data: {
              userId: this.user.id
            }
          }).then(async response => {
          await this.loadData();
          const msg = this.$tl("unBanNotify",[this.user.name]);
          this.$q.notify({
            message: msg,
            timeout: 5000,
            color: 'positive',
            position: 'top'
          });
        }).catch(error => {
          console.log("error", error);
        });
      },
      async loadData() {
        await this.$store.dispatch("request",
          {
            url: "/Profile/GetProfile",
            data: {
              link: this.link
            }
          }).then(response => {
          this.user = response.data;
          this.title = this.user.name;
        }).catch(error => {
          console.log("error", error);
        });
      }
    },
    async created() {
      await this.loadData();
    }
  }
</script>

<style lang="stylus" scoped>


  .f1 {
    display: flex;
    flex-wrap: wrap;

    .img {
      margin-right: 15px;
    }
  }

  @media (max-width: 600px) {
    .f1 .img {
      //width: 100%;
      text-align: center;
    }
  }
</style>
