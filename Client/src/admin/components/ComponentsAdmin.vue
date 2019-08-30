<template>
  <q-page class="components-admin page-padding">
    <div class="header-with-button">
      <h2 class="q-title">
        {{$tl("title")}}
      </h2>
      <q-btn icon="fas fa-plus" class="post-btn q-mr-lg" type="a" :to="{name: 'CreateComponent'}" no-caps
             :label="$tl('addComponentBtn')"/>
      <div class="clear"></div>
    </div>

    <div class="components" v-if="components">
      <div v-for="component in components">
        <router-link :to="{name: 'EditComponent', params: {name: component.name}}">
          <q-icon name="fas fa-cube" class="q-mr-xs"/>
          {{component.name}}
          <span class="text-grey-7 q-ml-sm">
            [{{component.type}}]
          </span>
        </router-link>
      </div>
    </div>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
    import {Page} from 'sun';


    export default {
        name: "ComponentsAdmin",
        mixins: [Page],
        data() {
            return {
                components: null
            }
        },
        methods: {
            loadData() {
                this.$store.dispatch('request', {url: '/Admin/ComponentsAdmin/GetAllComponents',})
                    .then(response => {
                        this.components = response.data;
                    })
            }
        },
        beforeCreate() {
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        },
        created() {
            this.title = this.$tl('title');
            this.loadData();
        }
    }

</script>

<style lang="stylus">

  .components-admin {
    .components {
      font-size: 1.15em;
    }
  }

</style>
