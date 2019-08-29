<template>
  <q-page class="cache-settings page-padding">
    <div class="header-with-button">
      <h2 class="q-title">{{$tl("title")}}</h2>
      <div class="clear"></div>
    </div>
    <div>
      <div class="q-mt-lg">
        <q-select
          v-model="policy"
          :label="$tl('cachePolicy')"
          :options="optionTypes">
          <q-icon slot="prepend" name="fa fa-sitemap" class="q-mr-sm" />
        </q-select>
        <br/>
        <div v-if="policy !== null && policy.id !== 1">
          <q-input v-if="!withoutTime" ref="cacheTime" type="number"
                   v-model="cacheSettings.invalidateCacheTime"
                   :label="$tl('cacheLifetime')"
                   :rules="rules.invalidateCacheTime">
            <template v-slot:prepend>
              <q-icon name="fas fa-clock"  class="q-mr-sm" />
            </template>
          </q-input>

          <q-checkbox v-model="withoutTime" v-on:input="withoutTimeChanged" :label="$tl('withoutInvalidationTime')"/>
        </div>
      </div>
      <br>
      <div class="btn-block">
        <q-btn icon="fas fa-save" class="send-btn" :loading="loading"
               no-caps :label="$tl('saveChangesBtn')"
               @click="save">
          <LoaderSent slot="loading"/>
        </q-btn>
      </div>
    </div>
  </q-page>
</template>

<script>
  import {Page} from 'sun'

  const cachePolicies = {
    Always: 0,
    Never: 1,
    Custom: 2
  };


  function createRules() {
    return {
      invalidateCacheTime: [
        value => !!value || this.$tl('validation.invalidateCacheTime.required'),
        value => value >= 0 || this.$tl('validation.invalidateCacheTime.invalidValue')
      ]
    }
  }

  export default {
    name: 'CacheSettings',
    mixins: [Page],
    data() {
      return {
        cacheSettings: null,
        policy: null,
        loading: false,
        withoutTime: false,
        optionTypes: [
          {id: cachePolicies.Always, label: this.$tl('alwaysPolicy'), value: 'AlwaysPolicy'},
          {id: cachePolicies.Never, label: this.$tl('neverPolicy'), value: 'NeverPolicy'},
          {id: cachePolicies.Custom, label: this.$tl('customPolicy'), value: 'CustomPolicy'}
        ],
      };
    },
    methods: {
      async loadCurrentPolicy() {
        await this.$store
          .dispatch('request', {
            url: '/Admin/AdminCacheSettings/GetCurrentCacheSettings'
          })
          .then(res => {
            this.policy = this.optionTypes[res.data.currentCachePolicy];
            this.cacheSettings = res.data;
            this.loading = false;
            if (this.cacheSettings.invalidateCacheTime === 0) this.withoutTime = true;
          });
      },
      async save() {
        if (this.policy.id !== cachePolicies.Never && !this.withoutTime) {
          const cacheTime = this.$refs.cacheTime;
          cacheTime.validate();
          if (cacheTime.hasError)
            return;
        }

        this.loading = true;
        await this.$store.dispatch('request', {
          url: '/Admin/AdminCacheSettings/ChangeCachePolicy',
          data: {
            selectedPolicy: this.policy.id,
            invalidateCacheTime: !this.withoutTime ? this.cacheSettings.invalidateCacheTime : 0
          },
        })
          .then(() => {
            this.$successNotify();
            this.loading = false;
          }).catch(error => {
            this.$errorNotify(error);
            this.loading = false;
          });
      },
      async withoutTimeChanged(value) {
        if (!value && this.cacheSettings.invalidateCacheTime === 0)
          this.cacheSettings.invalidateCacheTime = 15;
      }
    },
    beforeCreate() {
      this.rules = createRules.call(this);
      this.$options.components.LoaderSent = require('sun').LoaderSent;
    },
    async created() {
      this.title = this.$tl('title');
      await this.loadCurrentPolicy();
    }
  }

</script>

<style lang="stylus">

</style>
