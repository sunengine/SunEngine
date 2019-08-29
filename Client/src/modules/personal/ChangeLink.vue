
<template>
  <q-page class="change-link flex flex-center">
    <div class="center-form">
      <div class="text-grey-7 q-mb-lg" style="text-align: justify">
        {{$tl('linkValidationInfo')}}
      </div>
      <q-input ref="link" v-model="link" :label="$tl('link')"
               @keyup="checkLinkInDb" :rules="linkRules">
        <template v-slot:prepend>
          <q-icon name="fas fa-link"/>
        </template>
      </q-input>
      <q-btn no-caps class="q-mt-lg send-btn" icon="far fa-save"
             :label="$tl('saveBtn')" @click="save" :loading="submitting">
        <LoaderSent slot="loading"/>
      </q-btn>
    </div>
  </q-page>
</template>

<script>
  import {Page} from 'sun'
  import {store} from 'sun'


  function allowMyIdOrEmpty(id) {
    return !id || store.state.auth.user.id == id;
  }

  function createLinkRules() {
    return [
      value => (value.length >= 3 || allowMyIdOrEmpty.call(this, value)) || this.$tl('validation.minLength'),  // minLength or myId
      value => /^[a-zA-Z0-9-]*$/.test(value) || this.$tl('validation.allowedChars'), // allowed chars
      value => (/[a-zA-Z]/.test(value) || allowMyIdOrEmpty.call(this, value)) || this.$tl('validation.numberNotAllow'), // need char or myId
      value => !this.linkInDb || this.$tl('validation.linkInDb'), // link in db
    ];
  }

  export default {
    name: 'ChangeLink',
    mixins: [Page],
    data() {
      return {
        link: this.$store.state.auth.user.link,
        linkInDb: false,
        submitting: false
      }
    },
    linkRules: null,
    methods: {
      checkLinkInDb() {
        clearTimeout(this.timeout);
        if (this.link.toLowerCase() === this.$store.state.auth.user.link.toLowerCase())
          return;
        this.timeout = setTimeout(this.checkLinkInDbDo, 500);
      },
      checkLinkInDbDo() {
        this.$store.dispatch('request',
          {
            url: '/Personal/CheckLinkInDb',
            data: {
              link: this.link
            }
          }).then(response => {
          this.linkInDb = response.data.yes;
          this.$refs.link.validate();
        })
      },

      async save() {

        this.$refs.link.validate();

        if (this.$refs.link.hasError)
          return;


        this.submitting = true;

        await this.$store.dispatch('request',
          {
            url: '/Personal/SetMyLink',
            data: {
              link: this.link
            }
          }).then(response => {
          this.$store.commit('setUserInfo', response.data);
          this.$successNotify();
          this.$router.push({name: 'Personal'});

        }).catch(error => {
          this.$errorNotify(error);
          this.submitting = false;
        });
      }
    },
    beforeDestroy() {
      clearTimeout(this.timeout);
    },
    beforeCreate() {
      this.$options.components.LoaderSent = require('sun').LoaderSent;
    },
    async created() {
      this.title = this.$tl('title');

      this.linkRules = createLinkRules.call(this);
    }
  }

</script>

<style lang="stylus">

</style>
