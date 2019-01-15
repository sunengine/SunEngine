<template>
  <div>
    <img class="avatar msg-avatar" :src="$imagePath(message.authorAvatar)"/>

    <div class="q-ma-md">
      <div class="q-mb-xs text-grey-6" style="display: flex;">
        <span style="flex-grow:1">
           <router-link :to="'/user/'+message.authorLink">
             {{message.authorName}}
           </router-link>
        </span> &nbsp;
        <span v-if="canEdit" class=" q-px-sm">
                    <a href="#" @click.prevent="$emit('goEdit')">редактировать</a>
        </span>
        <span v-if="canMoveToTrash" class=" q-px-sm">
                    <a href="#" @click.prevent="moveToTrash"><q-icon name="fas fa-trash"/></a>
        </span>
        <span>
                    <q-icon name="far fa-clock"/> {{ $formatDate(message.publishDate) }}
        </span>
      </div>
      <div v-html="message.text">

      </div>
      <div class="q-py-xs text-grey-8">


      </div>
    </div>

  </div>
</template>

<script>
  export default {
    name: "ReadMessage",
    props: {
      message: Object,
      canEdit: {
        type: Boolean,
        default: false
      },
      canMoveToTrash: {
        type: Boolean,
        default: false
      },
      goEdit: Function
    },
    methods: {
      async moveToTrash() {
        this.$q.dialog({
          title: 'Удалить сообщение?',
          //message: '',
          ok: 'Да',
          cancel: 'Отмeна'
        }).then(async () => {
          await this.$store.dispatch("request",
            {
              url: "/Messages/MoveToTrash",
              data:
                {
                  id: this.message.id
                }
            }).then(
            () => {
              this.message.isDeleted = true;
            }).catch((x) => {
            console.log("error", x)
          });
        }).catch(() => {
        });
      },
    }
  }
</script>

<style lang="stylus" scoped>
  .msg-avatar {
    float: left;
    margin: 2px 12px 12px 17px
  }

</style>
