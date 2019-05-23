<template>
  <div>
    <q-input ref="name" v-model="menuItem.name" :label="$tl('name')" :rules="rules.name"/>
    <q-input ref="title" v-model="menuItem.title" :label="$tl('title')" :rules="rules.title"/>
    <q-input ref="subTitle" v-model="menuItem.subTitle" :label="$tl('subTitle')" :rules="rules.subTitle"/>
    <q-input ref="url" @input="urlUpdated" v-model="url" :label="$tl('url')" :rules="rules.url"/>
    <q-checkbox ref="exact" v-model="menuItem.exact" :label="$tl('exact')"/>


    <q-field v-if="parentOptions" :label="$tl('parent')" stack-label>
      <template v-slot:control>
        <div tabindex="0" class="no-outline full-width">
           <q-icon v-if="parentIcon" :name="parentIcon" class="q-mr-xs"/> {{parentTitle}}
        </div>
      </template>
      <template v-slot:append>
        <q-icon name="expand_more"></q-icon>
      </template>
      <q-menu fit auto-close>
        <q-tree
          :nodes="parentOptions"
          default-expand-all
          :selected.sync="menuItem.parentId"
          node-key="id"
          label-key="title"
        ></q-tree>
      </q-menu>
    </q-field>
    <LoaderWait v-else/>

    <q-input ref="cssClass" v-model="menuItem.cssClass" :label="$tl('cssClass')" :rules="rules.cssClass"/>
    <q-input ref="icon" v-model="menuItem.icon" :label="$tl('icon')" :rules="rules.icon"/>
    <q-input ref="settingsJson" type="textarea" v-model="menuItem.settingsJson" :label="$tl('settingsJson')"
             :rules="rules.settingsJson"/>
    <q-checkbox ref="isHidden" v-model="menuItem.isHidden" :label="$tl('isHidden')"/>
  </div>
</template>

<script>
  function createRules() {
    return {
      name: [
        value => (!value || value.length >= 3) || this.$tl("validation.name.minLength"),
        value => value.length <= config.DbColumnSizes.MenuItems_Name || this.$tl("validation.name.maxLength"),
        value => /^[a-zA-Z0-9_-]*$/.test(value) || this.$tl("validation.name.allowedChars"),
      ],
      title: [
        value => !!value || this.$tl("validation.title.required"),
        value => value.length >= 3 || this.$tl("validation.title.minLength"),
        value => value.length <= config.DbColumnSizes.MenuItems_Title || this.$tl("validation.title.maxLength"),
      ],
      subTitle: [
        value => (!value || value.length >= 3) || this.$tl("validation.subTitle.minLength"),
        value => value.length <= config.DbColumnSizes.MenuItems_SubTitle || this.$tl("validation.subTitle.maxLength"),
      ],
      url: [],
      cssClass: [
        value => (!value || value.length >= 3) || this.$tl("validation.cssClass.minLength"),
        value => value.length <= config.DbColumnSizes.MenuItems_SubTitle || this.$tl("validation.cssClass.maxLength"),
      ],
      icon: [
        value => (!value || value.length >= 3) || this.$tl("validation.icon.minLength"),
        value => value.length <= config.DbColumnSizes.MenuItems_SubTitle || this.$tl("validation.icon.maxLength"),
      ],
      settingsJson: [
        value => (!value || isJson(value)) || this.$tl("validation.settingsJson.jsonFormatError")
      ]
    }
  }

  function isJson(str) {
    try {
      JSON.parse(str);
    } catch {
      return false;
    }
    return true;
  }

  export default {
    name: "MenuItemForm",
    props: {
      menuItem: {
        type: Object,
        required: true
      }
    },
    data() {
      return {
        url: '',
        urlError: false,
        parentOptions: null,
        menuItemsById: null,
        selectedParentMenuItem: null
      }
    },
    rules: null,
    watch: {
      'url': 'urlUpdated'
    },
    computed: {
      parentTitle() {
        const key = this.menuItem.parentId;
        if(!this.menuItemsById[key])
          return "";
        else
          return this.menuItemsById[key].title;
      },
      parentIcon() {
        const key = this.menuItem.parentId;
        if(!this.menuItemsById[key])
          return null;
        else
          return this.menuItemsById[key].icon;
      },
      hasError() {
        return this.$refs.name.hasError ||
          this.$refs.title.hasError ||
          this.$refs.subTitle.hasError ||
          this.$refs.cssClass.hasError ||
          this.$refs.icon.hasError ||
          this.$refs.settingsJson.hasError;
      }
    },
    methods: {
      validate() {
        this.$refs.name.validate();
        this.$refs.title.validate();
        this.$refs.subTitle.validate();
        this.$refs.cssClass.validate();
        this.$refs.icon.validate();
        this.$refs.settingsJson.validate();
      },
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
      },
      prepareMenuItems(allMenuItems) {

        this.menuItemsById = {};

        for (const menuItem of allMenuItems) {
          this.menuItemsById[menuItem.id.toString()] = menuItem;
        }

        let root;

        for (let menuItem of allMenuItems) {
          if (menuItem.parentId) {
            const parent = this.menuItemsById[menuItem.parentId.toString()];
            if (!parent)
              continue;

            if (!parent.children)
              parent.children = [];

            parent.children.push(menuItem);
            menuItem.parent = parent;
          } else {
            root = menuItem;
          }
        }

        this.parentOptions = [root];
      },
      async loadData() {
        await this.$store.dispatch("request",
          {
            url: "/Admin/MenuAdmin/GetMenuItems",
          })
          .then(
            response => {
              this.prepareMenuItems(response.data);
            }
          ).catch(error => {
            this.$errorNotify(error);
          });
      }
    },
    beforeCreate() {
      this.$options.components.LoaderWait = require('sun').LoaderWait;
    },
    async created() {
      this.rules = createRules.call(this);
      await this.loadData();
    }
  }

</script>

<style scoped>

</style>
