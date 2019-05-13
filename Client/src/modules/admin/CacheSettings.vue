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
          :options="OptionTypes"/>
        <br/>
        <div v-if="policy !== null && policy.id !== 1">
          <q-input v-if="!withoutTime" ref="cacheTime" type="number"
                   v-model="cacheSettings.invalidateCacheTime"
                   :label="$tl('CacheLifetime')"
                   :rules="rules.invalidateCacheTime"/>

          <q-checkbox v-model="withoutTime" v-on:input="withoutTimeChanged" :label="$tl('WithoutInvalidationTime')"/>
        </div>
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
  import {Page} from 'sun'
  import {cachePolicies} from 'sun'

  function createRules() {
    return {
      invalidateCacheTime: [
        value => !!value || this.$tl("validation.invalidateCacheTime.required"),
        value => value >= 0 || this.$tl("validation.invalidateCacheTime.invalidValue")
      ]
    }
  }

  export default {
    name: "CacheSettings",
    mixins: [Page],
    data() {
      return {
        cacheSettings: null,
        policy: null,
        loading: false,
        withoutTime: false,
        rules: null,
        OptionTypes: [
          {id: cachePolicies.Always, label: this.$tl("AlwaysPolicy"), value: "AlwaysPolicy"},
          {id: cachePolicies.Never, label: this.$tl("NeverPolicy"), value: "NeverPolicy"},
          {id: cachePolicies.Custom, label: this.$tl("CustomPolicy"), value: "CustomPolicy"}
        ],
      };
    },
    methods: {
      async loadCurrentPolicy() {
        await this.$store
          .dispatch("request", {
            url: "/Admin/AdminCacheSettings/GetCurrentCacheSettings"
          })
          .then(res => {
            this.policy = this.OptionTypes[res.data.currentCachePolicy];
            this.cacheSettings = res.data;
            this.loading = false;
            if (this.cacheSettings.invalidateCacheTime === 0) this.withoutTime = true;
          });
      },
      async save() {
        if (this.policy.id !== CachePolicies.Never && !this.withoutTime) {
          const cacheTime = this.$refs.cacheTime;
          cacheTime.validate();
          if (cacheTime.hasError)
            return;
        }

        this.loading = true;
        await this.$store.dispatch("request", {
          url: "/Admin/AdminCacheSettings/ChangeCachePolicy",
          data: {
            selectedPolicy: this.policy.id,
            invalidateCacheTime: !this.withoutTime ? this.cacheSettings.invalidateCacheTime : 0
          },
        })
          .then(() => {
            this.$q.notify({
              message: this.$tl("successNotify"),
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
      },
      async withoutTimeChanged(value) {
        if (!value && this.cacheSettings.invalidateCacheTime === 0)
          this.cacheSettings.invalidateCacheTime = 15;
      }
    },
    beforeCreate() {
      this.$options.components.LoaderSent = require('sun').LoaderSent;
    },
    async created() {
      this.title = this.$tl("title");
      this.rules = createRules.call(this);
      await this.loadCurrentPolicy();
    }
  }

</script>

<style lang="stylus" scoped>
</style>
