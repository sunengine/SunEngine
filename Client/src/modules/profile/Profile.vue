<template>
  <q-page>
    <div class="f1" v-if="user">
      <div class="img flex column">
        <img width="300" height="300"  :src="$imagePath(user.photo)"/>
        <div v-if="messageButtons" class="private-messages-block flex q-mt-sm" style="padding-right: 2px; padding-left: 2px; align-items: center; width: 100%">
          <q-btn class="shadow-1" color="lime-4" style="flex-grow: 1" :disable="!canPrivateMessage"
                 :to="{path: '/WritePrivateMessage'.toLowerCase(), query: {userId: user.id, userName: user.name }}"
                 dense icon="far fa-envelope"  label="Написать пользователю"/>
          <q-btn :color="!user.iBannedHim ? 'lime-4' : 'negative'" class="shadow-1 q-ml-sm" dense style="padding-left:10px !important; padding-right: 10px; !important"  v-if="!user.noBunable" icon="fas fa-ellipsis-v">
            <q-popover>
              <div v-close-overlay>
                <q-btn color="negative" dense v-close-overlay v-if="!user.iBannedHim" @click="ban"  icon="fas fa-ban" label="Забанить"/>
                <q-btn color="positive" dense v-close-overlay v-else @click="unBan"  icon="fas fa-smile" label="Разбанить"/>
              </div>
            </q-popover>
          </q-btn>
        </div>
      </div>
      <div>
        <h4>{{user.name}}</h4>
        <div v-html="user.information"></div>
      </div>
    </div>
    <loader-wait v-else/>
  </q-page>
</template>

<script>
  import LoaderWait from "LoaderWait";
  import Page from "Page";

  export default {
    name: "Profile",
    mixins: [Page],
    components: {LoaderWait},
    props: {
      link: {
        type: String,
        required: true
      }
    },
    data: function () {
      return {
        user: null
      }
    },
    computed: {
      canPrivateMessage() {
        const from = this.$store?.state?.auth?.user;
        if (!from) return false;
        if (this.user.heBannedMe || this.user.iBannedHim) return false;
        return from.id != this.user?.id;
      },
      messageButtons() {
        const from = this.$store?.state?.auth?.user;
        if (!from) return false;
        return from.id != this.user?.id;
      }
    },
    watch: {
      'link': 'loadData'
    },
    methods: {
      async ban() {
        await this.$store.dispatch("request",
          {
            url: "/Profile/BanUser",
            data: {
              userId: this.user.id
            }
          }).then(async response => {
          await this.loadData();
          this.$q.notify({
            message: `Пользователь ${this.user.name} теперь не может вам писать`,
            timeout: 5000,
            type: 'positive',
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
          this.$q.notify({
            message: `Пользователь ${this.user.name} теперь может вам писать`,
            timeout: 5000,
            type: 'positive',
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
          this.setTitle(this.user.name);
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
  @import '~variables';


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
