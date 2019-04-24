<template>
  <q-page class="page-padding">
    <div class="header-with-button">
      <h2 class="q-title">{{$tl("title")}}</h2>
      <div class="clear"></div>
    </div>
    <div>
      <div class="q-mt-lg">
        <q-select
          v-model="policy"
          :label="$tl('CachePolicy')"
          :options="PolicyTypes"
        />
        <br />
        <q-input v-if="policy !== null && policy.id !== 1" v-model="cacheSettings.invalidateCacheTime" type="number" :label="$tl('CacheLifetime')" />
      </div>

      <br>
      <div class="btn-block">
        <q-btn icon="fas fa-plus" class="btn-send" :loading="loading"
          no-caps :label="$tl('SaveChanges')" color="send"
          @click="save">
          <LoaderSent slot="loading"/>
        </q-btn>
      </div>
    </div>
  </q-page>
</template>

<script>
import Page from "Page";
import LoaderSent from "LoaderSent";

export default {
  name: "CacheSettings",
  components: {LoaderSent},
  mixins: [Page],
  i18nPrefix: "admin",
  data: function() {
    return {
      cacheSettings: null,
      policy: null,
      loading: false,
      PolicyTypes: [
        { id: 0, label: this.$tl("AlwaysPolicy"), value: "AlwaysPolicy" },
        { id: 1, label: this.$tl("NeverPolicy"), value: "NeverPolicy" },
        { id: 2, label: this.$tl("CustomPolicy"), value: "CustomPolicy" }
      ]
    };
  },
  methods: {
    async loadCurrentPolicy() {
      await this.$store
        .dispatch("request", {
          url: "/Admin/AdminCacheSettings/GetCurrentCacheSettings"
        })
        .then(res => {
          this.policy = this.PolicyTypes[res.data.currentCachePolicy];
          this.cacheSettings = res.data;
          this.loading = false;
        });
    },
    async save() {
      if(this.policy.id === 1) this.cacheSettings.invalidateCacheTime = null;
      this.loading = true;
      await this.$store.dispatch("request", {
        url: "/Admin/AdminCacheSettings/ChangeCachePolicy",
        data: { 
            selectedPolicy: this.policy.id,
            invalidateCacheTime: this.cacheSettings.invalidateCacheTime
          },
      })
      .then(() => {
        const msg = this.$tl("successNotify");
        this.$q.notify({
          message: msg,
          timeout: 5000,
          color: 'positive',
          icon: 'far fa-check-circle',
          position: 'top'
         });
        this.loading = false;
      }).catch(error => {
          this.$q.notify({
            message: this.$tl("error"),
            timeout: 5000,
            color: 'negative',
            position: 'top'
          });
        this.loading = false;
      });
    }
  },
  async created() {
      this.title = this.$tl("title");
      await this.loadCurrentPolicy();
  }
};
</script>

<style lang="stylus" scoped>
</style>