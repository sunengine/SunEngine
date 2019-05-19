<template>
  <div>
    <q-input ref="name" v-model="menuItem.name" :label="$tl('name')" :rules="rules.name"/>
    <q-input ref="title" v-model="menuItem.title" :label="$tl('title')" :rules="rules.title"/>
    <q-input ref="subTitle" v-model="menuItem.subTitle" :label="$tl('subTitle')" :rules="rules.subTitle"/>
    <q-input ref="url" @input="urlUpdated" v-model="url" :label="$tl('url')" :rules="rules.url"/>
    <q-checkbox ref="exact" v-model="menuItem.exact" :label="$tl('exact')"/>
    <q-input ref="cssClass" v-model="menuItem.cssClass" :label="$tl('cssClass')" :rules="rules.cssClass"/>
    <q-input ref="icon" v-model="menuItem.icon" :label="$tl('icon')" :rules="rules.icon"/>
    <q-input ref="customIcon" v-model="menuItem.customIcon" :label="$tl('customIcon')" :rules="rules.customIcon"/>
    <q-input ref="settingsJson" type="textarea" v-model="menuItem.settingsJson" :label="$tl('settingsJson')"
             :rules="rules.settingsJson"/>
    <q-checkbox ref="isHidden" v-model="menuItem.isHidden" :label="$tl('isHidden')"/>
  </div>
</template>

<script>


  export default {
    name: "MenuItemForm",
    props: {
      menuItem: {
        type: Object,
        required: false,
        default: {
          id: 0,
          name: '',
          title: '',
          subTitle: '',
          exact: false,
          routeName: '',
          routeParamsJson: '',
          cssClass: '',
          externalUrl: '',
          icon: '',
          customIcon: '',
          settingsJson: '',
          isHidden: false
        }
      }
    },
    data() {
      return {
        url: '',
        urlError: false
      }
    },
    watch: {
      'url': 'urlUpdated'
    },
    methods: {
      urlUpdated() {
        if (this.url.startsWith(config.SiteUrl)) {
          const resolved = this.$router.resolve(this.url);
          if (resolved && resolved.route) {
            this.menuItem.routeName = resolved.route.name;
            this.menuItem.routeParamsJson = resolved.route.params;
            this.menuItem.externalUrl = '';
            this.urlError = false;
            return;
          }
        } else if (this.url.startsWith('http://') || this.url.startsWith('https://')) {
          this.menuItem.routeName = '';
          this.menuItem.routeParamsJson = '';
          this.menuItem.externalUrl = this.url;
          this.urlError = false;
          return;
        }

        this.urlError = true;
      }
    }
  }

</script>

<style scoped>

</style>
