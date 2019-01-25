<template>
  <div>
    <h2 class="q-title"> Написать
      <q-icon name="far fa-user"/>
      {{userName}}
    </h2>

    <q-editor :toolbar="[
          ['bold', 'italic', 'strike', 'underline'],
        ['token', 'hr' ],


         ['quote', 'unordered', 'ordered' ],
        ['undo', 'redo','fullscreen'],
             ]"

              ref="htmlEditor" v-model="text"/>

    <q-btn @click="send" :loading="loading" label="Отправить">
      <loader-sent slot="loading"/>
    </q-btn>
    <q-btn @click="$router.$goBack()" label="Отмена"/>
  </div>
</template>

<script>
  import Page from "Page";
  import LoaderSent from "LoaderSent";

  export default {
    name: "WritePrivateMessage",
    components: {LoaderSent},
    mixins: [Page],
    props: {
      userId: {
        type: String,
        required: true
      },
      userName: {
        type: String,
        required: true
      }
    },
    data: function () {
      return {
        text: "",
        loading: false
      }
    },
    methods: {
      async send() {
        await this.$store.dispatch("request",
          {
            url: "/Profile/SendPrivateMessage",
            data: {
              userId: this.userId,
              text: this.text
            }
          })
          .then(
            response => {
              this.$q.notify({
                message: `Сообщение успешно отправлено на почтовый ящик ${this.userName}.`,
                timeout: 5000,
                type: 'positive',
                position: 'top'
              });
              this.loading = false;
              this.$router.$goBack();
            }
          ).catch(x => {
            debugger;
            if (x.response.data.errorName == "SpamProtection") {
              this.$q.notify({
                message: 'Нельзя так часто отправлять личные сообщения. Необходимо подождать.',
                timeout: 5000,
                type: 'warning',
                position: 'top'
              });
            } else {
              console.log("error", x);
              this.loading = false;
              this.$q.notify({
                message: 'Сообщение не отправлено. Ошибка на сервере.',
                timeout: 5000,
                type: 'negative',
                position: 'top'
              });
              this.loading = false;
            }
          });

      }
    },
    created() {
      this.setTitle("Написать личное сообщение");
    }

  }
</script>

<style scoped>

</style>
