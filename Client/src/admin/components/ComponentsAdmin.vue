<template>
  <q-page class="components-admin page-padding">
    <div class="header-with-button">
      <h2 class="q-title">
        {{$tl("title")}}
      </h2>
      <q-btn icon="far fa-plus-square" class="post-btn q-mr-lg" type="a" :to="{name: 'CreateComponent'}" no-caps
             :label="$tl('addComponentBtn')"/>
      <div class="clear"></div>
    </div>

    <div v-if="components">
      <div v-for="component in component">
        <router-link :to="{name: 'EditComponent', props: {name: component.name}}">{{component.title}}
          [{{component.name}}]
        </router-link>
      </div>
      <!-- <ComponentItem :component="component" v-for="component in components" />-->
    </div>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>

    export default {
        name: "ComponentsAdmin",
        components: {ComponentItem, LoaderWait},
        data() {
            return {
                components: null
            }
        },
        methods: {
            loadData() {
                this.$store.dispatch('request', {url: '/Admin/MenuAdmin/GetMenuItems',})
                    .then(response => {
                        this.components = response.data;
                    })
            }
        },
        beforeCreate() {
            this.$options.components.LoaderWait = require('sun').LoaderWait;
            this.$options.components.ComponentItem = require('sun').ComponentItem;
        },
        created() {
            this.title = this.$tl('title');
            this.loadData();
        }
    }

</script>

<style lang="stylus">

  .components-admin {

  }

</style>
