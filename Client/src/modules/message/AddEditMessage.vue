<template>
  <div>
    <MyEditor
      :toolbar="[
          ['bold', 'italic', 'strike', 'underline'],
          ['token', 'hr', 'link', 'addImages'],
          [
            {
              label: $q.i18n.editor.formatting,
              icon: $q.icon.editor.formatting,
              list: 'no-icons',
              options: ['p', 'h2', 'h3', 'h4', 'h5', 'h6', 'code']
            },
            {
              label: $q.i18n.editor.fontSize,
              icon: $q.icon.editor.fontSize,
              fixedLabel: true,
              fixedIcon: true,
              list: 'no-icons',
              options: ['size-1', 'size-2', 'size-3', 'size-4', 'size-5', 'size-6', 'size-7']
            },
            'removeFormat'
        ],
        ['quote', 'unordered', 'ordered'],
        ['undo', 'redo','fullscreen'],
                    ]"

      class="editor" ref="htmlEditor" v-model="message.text"/>
    <div>
      <q-btn icon-right="fas fa-arrow-circle-right" no-caps @click="send" :loading="loading"
             :label="isNew ? 'Отправить' : 'Сохранить'" color="send">
        <LoaderSent slot="loading"/>
      </q-btn>
      <span :class="['error', {'invis' : !($v.message.text.$invalid && !start) } ]">
        {{!$v.message.text.required ? 'Введите текст' : 'Минимальная длинна текста - 5'}}
      </span>
    </div>
  </div>
</template>

<script>
  import LoaderSent from "LoaderSent";
  import {required} from 'vuelidate/lib/validators'
  import htmlTextSizeOrHasImage from "HtmlTextSizeOrHasImage.js";
  import MyEditor from "../../components/MyEditor";

  export default {
    name: "AddEditMessage",
    components: {MyEditor, LoaderSent},
    data: function () {
      return {
        message: {
          materialId: this.materialId,
          text: ""
        },
        loading: false,
        start: true
      }
    },
    props: {
      messageId: {
        type: Number,
        required: false
      },
      materialId: {
        type: Number,
        required: false
      },
      done: Function
    },
    validations: {
      message: {
        text: {
          required,
          htmlTextSizeOrHasImage() {
            return htmlTextSizeOrHasImage(this.$refs?.htmlEditor?.$refs?.content, 5)
          }
        }
      }
    },

    computed: {
      isNew: function () {
        return this.messageId == null;
      }
    },

    methods: {
      async addMessage() {
        this.loading = true;
        await this.$store.dispatch("request",
          {
            url: "/Messages/Add",
            data: {
              materialId: this.materialId,
              text: this.message.text
            }
          }).then(response => {
          this.message.text = "";
          this.$emit('done');
          this.loading = false;
        }).catch(error => {
          if (error.response.data.errorName == "SpamProtection") {
            this.$q.notify({
              message: 'Нельзя так часто отправлять сообщения. Подождите немного.',
              timeout: 5000,
              type: 'warning',
              position: 'top'
            });
          }
          else {
            this.$q.notify({
              message: error.response.data.errorText,
              timeout: 2000,
              type: 'negative',
              position: 'top'
            });
          }
          this.loading = false;
        });
      },
      async editMessage() {
        this.loading = true;
        await this.$store.dispatch("request",
          {
            url: "/Messages/Edit",
            data: {
              Id: this.messageId,
              MaterialId: this.materialId,
              Text: this.message.text
            }
          }).then(response => {
            this.$emit('done');
            this.loading = false;
          }
        ).catch(error => {

          this.loading = false;
        });
      },
      async send() {
        this.start = false;
        this.$v.$touch();

        if (this.$v.$invalid) {
          return;
        }

        if (this.isNew) {
          await this.addMessage();
        } else {
          await this.editMessage();
        }

        this.start = true;
        this.$v.$reset();
      }
    },

    async created() {
      if (!this.isNew) {
        await this.$store.dispatch("request",
          {
            url: "/Messages/Get",
            data: {
              id: this.messageId
            }
          }).then(response => {
            this.message = response.data;
          }
        )
      }
    },
  }
</script>

<style lang="stylus" scoped>
  @import '~variables';

  .editor {
    margin-bottom: 7px;
  }

  .error {
    font-size: 0.9em;
    color: $red-5;
    margin-left: 10px;
  }

  .invis {
    visibility: hidden;
  }
</style>
