<template>
  <QPage class="flex full-center">
    <div class="container">
      <div class="text-grey-7 q-mb-lg" style="text-align: justify">
        Link должен состоять не менее чем из 3 символов <span class="text-grey-7">'a-z', 'A-Z', '-', '0-9'</span>. И
        содержать хотя бы одну букву.
      </div>
      <q-field :error="$v.link.$invalid" :error-label="errorMessage">
        <q-input color="positive"  v-model="link" float-label="Link" @keyup="checkLinkInDb"
                 :after="[{
        icon: 'far fa-check-circle',
        color: 'green',
        condition: !$v.link.$invalid},
        ]"
        />
      </q-field>
      <QBtn class="q-mt-lg" icon="far fa-save" label="Сохранить" color="send" @click="save" :loading="submitting">
        <LoaderSent slot="loading"/>
      </QBtn>
    </div>
  </QPage>
</template>

<script>
  import Page from "Page";
  import {minLength, helpers, or} from 'vuelidate/lib/validators'
  import LoaderSent from "LoaderSent";

  const allowedChars = helpers.regex('allowedChars', /^[a-zA-Z0-9-]*$/)
  const needChar = helpers.regex('numberNotAllow', /[a-zA-Z]/)

  function allowMyId(x) {
    return this.$store.state.auth.user.id == x
  }


  export default {
    name: "ChangeLink",
    mixins: [Page],
    components: {LoaderSent},
    data: function () {
      return {
        link: this.$store.state.auth.user.link,
        linkInDb: true,
        submitting: false
      }
    },
    validations: {
      link: {
        minLength: or(minLength(3), allowMyId),
        allowedChars,
        numberNotAllow: or(needChar, allowMyId),
        linkInDb: function () {
          return this.linkInDb
        }
      }
    },
    computed: {
      errorMessage() {
        if (!this.$v.link.minLength)
          return "Длинна link должна быть не менее 3";
        if (!this.$v.link.allowedChars)
          return "Допустимы только буквы английского алфавита и цифры";
        if (!this.$v.link.numberNotAllow)
          return "Необходимо что бы в link входили буквы";
        if (!this.$v.link.linkInDb)
          return "Этот link уже занят";
      }
    },
    methods: {
      checkLinkInDb() {
        clearTimeout(this.timeout);
        if (this.link.toLowerCase() == this.$store.state.auth.user.link.toLowerCase())
          return;
        this.timeout = setTimeout(this.checkLinkInDbDo, 1000);
      },
      checkLinkInDbDo() {
        this.$store.dispatch("request",
          {
            url: "/Personal/CheckLinkInDb",
            data: {
              link: this.link
            }
          }).then(response => {
          console.log("xxx");
          this.linkInDb = !response.data.yes;
        })
      },

      async save() {

        this.$v.$touch();
        if (this.$v.$invalid) {
          return;
        }

        this.submitting =  true;

        await this.$store.dispatch("request",
          {
            url: "/Personal/SetMyLink",
            data: {
              link: this.link
            }
          }).then(response => {
          this.$store.commit('setUserInfo', response.data);
          this.$q.notify({
            message: `Link отредактирован`,
            timeout: 2000,
            type: 'info',
            position: 'top'
          });
          this.$router.push({name: 'Personal'});

        }).catch(error => {
          this.$q.notify({
            message: error.response.data.errorText,
            timeout: 5000,
            type: 'negative',
            position: 'top'
          });
          this.submitting = false;
        });
      }
    },
    beforeDestroy() {
      clearTimeout(this.timeout);
    },
    async created() {
      this.setTitle("Редактировать Link пользователя");
    }
  }
</script>

<style lang="stylus" scoped>
  .q-field {
    height: 78px;
  }

  .container {
    display flex;
    flex-direction: column;
    width: 270px;
    justify-content: stretch;
  }

</style>
