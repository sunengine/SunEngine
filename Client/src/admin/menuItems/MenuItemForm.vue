<template>
  <div class="menu-item-form">
    <q-input ref="name" v-model="menuItem.name" :label="$tl('name')" :rules="rules.name">
      <template v-slot:prepend>
        <q-icon name="fas fa-signature" class="q-mr-xs"/>
      </template>
    </q-input>
    <q-input ref="title" v-model="menuItem.title" :label="$tl('title')" :rules="rules.title">
      <template v-slot:prepend>
        <q-icon name="fas fa-heading" class="q-mr-xs"/>
      </template>
    </q-input>
    <q-input ref="subTitle" v-model="menuItem.subTitle" :label="$tl('subTitle')" :rules="rules.subTitle">
      <template v-slot:prepend>
        <q-icon name="fas fa-info" class="q-mr-xs"/>
      </template>
    </q-input>
    <q-input ref="url" class="q-mb-md" @input="urlUpdated" v-model="url" :label="$tl('url')" :rules="rules.url">
      <template v-slot:prepend>
        <q-icon name="fas fa-link" class="q-mr-xs"/>
      </template>
      <div slot="hint">
        <div v-if="menuItem.routeName" class="text-positive">
          [{{$tl("local")}}: RouteName={{menuItem.routeName}}
          <template v-if="menuItem.routeParamsJson">
            Params={{menuItem.routeParamsJson}}
          </template>
          ]
        </div>
        <div v-else-if="menuItem.externalUrl" class="text-positive">
          [{{$tl("external")}}]
        </div>
        <div v-else-if="urlError" class="text-red-8">
          [{{$tl("urlError")}}]
        </div>
      </div>
    </q-input>

    <q-field class="cursor-pointer q-mb-md" v-if="parentOptions" :label="$tl('parent')" stack-label>
      <template v-slot:control>
        <div tabindex="0" class="no-outline full-width">
          <q-icon :name="parentIcon" class="q-mr-xs" color="grey-7"/>
          {{parentTitle}}
        </div>
      </template>
      <template v-slot:append>
        <q-icon name="fas fa-caret-down"></q-icon>
      </template>
      <q-menu fit auto-close>
        <q-tree
          :nodes="parentOptions"
          default-expand-all
          :selected.sync="menuItem.parentId"
          node-key="id"
          label-key="title"
        >
          <template v-slot:default-header="prop">
            <div style="margin:0; padding: 0;">
              <q-icon :name="prop.node.icon" class="q-mx-sm" color="grey-7" size="16px"/>
              <span>{{prop.node.title}}</span>
            </div>
          </template>
        </q-tree>
      </q-menu>
    </q-field>

    <LoaderWait v-else/>

    <q-select v-if="allRoles" class="q-mb-md" v-model="roles" :options="allRoles" multiple use-chips stack-label
              option-value="name" option-label="title" :label="$tl('roles')"/>
    <LoaderWait v-else/>

    <q-input ref="cssClass" v-model="menuItem.cssClass" :label="$tl('cssClass')" :rules="rules.cssClass">
      <template v-slot:prepend>
        <q-icon name="fab fa-css3-alt" class="q-mr-xs"/>
      </template>
    </q-input>
    <q-input ref="icon" v-model="menuItem.icon" :label="$tl('icon')" :rules="rules.icon">
      <div slot="prepend" v-if="menuItem.icon">
        <q-icon :name="menuItem.icon"/>
      </div>
    </q-input>
    <q-input ref="settingsJson" type="textarea" v-model="menuItem.settingsJson" autogrow :label="$tl('settingsJson')"
             :rules="rules.settingsJson"/>
    <q-checkbox ref="exact" v-model="menuItem.exact" :label="$tl('exact')"/>
    <br/>
    <q-checkbox ref="isHidden" v-model="menuItem.isHidden" :label="$tl('isHidden')"/>
  </div>
</template>

<script>
  import {isJson} from 'sun';


  function createRules() {
    return {
      name: [
        value => (!value || value.length >= 3) || this.$tl('validation.name.minLength'),
        value => (!value || value.length <= config.DbColumnSizes.MenuItems_Name) || this.$tl('validation.name.maxLength'),
        value => /^[a-zA-Z0-9_-]*$/.test(value) || this.$tl('validation.name.allowedChars'),
      ],
      title: [
        value => !!value || this.$tl('validation.title.required'),
        value => (!value || value.length >= 3) || this.$tl('validation.title.minLength'),
        value => value.length <= config.DbColumnSizes.MenuItems_Title || this.$tl('validation.title.maxLength'),
      ],
      subTitle: [
        value => (!value || value.length >= 3) || this.$tl('validation.subTitle.minLength'),
        value => (!value || value.length) <= config.DbColumnSizes.MenuItems_SubTitle || this.$tl('validation.subTitle.maxLength'),
      ],
      url: [],
      cssClass: [
        value => (!value || value.length >= 3) || this.$tl('validation.cssClass.minLength'),
        value => (!value || value.length) <= config.DbColumnSizes.MenuItems_SubTitle || this.$tl('validation.cssClass.maxLength'),
      ],
      icon: [
        value => (!value || value.length >= 3) || this.$tl('validation.icon.minLength'),
        value => (!value || value.length) <= config.DbColumnSizes.MenuItems_SubTitle || this.$tl('validation.icon.maxLength'),
      ],
      settingsJson: [
        value => (!value || isJson(value)) || this.$tl('validation.settingsJson.jsonFormatError')
      ]
    }
  }



  export default {
    name: 'MenuItemForm',
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
        selectedParentMenuItem: null,
        roles: null,
        allRoles: null
      }
    },
    watch: {
      'url': 'urlUpdated',
      'roles': 'rolesUpdated'
    },
    computed: {
      parentTitle() {
        const key = this.menuItem.parentId;
        if (!this.menuItemsById[key])
          return '';
        else
          return this.menuItemsById[key].title;
      },
      parentIcon() {
        const key = this.menuItem.parentId;
        if (!this.menuItemsById[key])
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
      rolesUpdated() {
        this.menuItem.roles = this.roles.map(x => x.name).join(',');
      },
      getUrl() {
        if (!this.menuItem)
          return;

        if (this.menuItem.routeName) {
          const resolved = this.$router.resolve(
            {
              name: this.menuItem.routeName,
              params: this.menuItem.routeParamsJson ? JSON.parse(this.menuItem.routeParamsJson) : undefined
            });
          if (resolved && resolved.href) {
            this.url = resolved.href;
          }
        } else if (this.menuItem.externalUrl) {
          this.url = this.menuItem.externalUrl;
        }
      },
      /*pull() {  TODO Needed for future release to push and pull information from route url, parse it and set menuItem.title, subTitle and icon from route page
        if (this.menuItem?.routeParamsJson) {
          const routeParams =  JSON.parse(this.menuItem.routeParamsJson);
          if(routeParams.idOrName)
            alert('mat ' + routeParams.idOrName);
        }
        if(/articles|forum|blog/.test(this.menuItem.routeName)) {
          const routeParams =  JSON.parse(this.menuItem.routeParamsJson);
          if(routeParams && routeParams.categoryName) {
            alert('cat ' + routeParams.categoryName);
          }
          else {
            alert('cat ' + this.menuItem.routeName);
          }
        }
      },*/
      validate() {
        this.$refs.name.validate();
        this.$refs.title.validate();
        this.$refs.subTitle.validate();
        this.$refs.cssClass.validate();
        this.$refs.icon.validate();
        this.$refs.settingsJson.validate();
      },
      urlUpdated() {

        const resolve = function resolve(url) {
          if (!url) {
            this.menuItem.routeName = '';
            this.menuItem.routeParamsJson = '';
            this.menuItem.externalUrl = '';
            this.urlError = false;
            return;
          }

          const resolved = this.$router.resolve(url);

          if (resolved && resolved.route && resolved.route.name) {
            this.menuItem.routeName = resolved.route.name;
            this.menuItem.routeParamsJson = JSON.stringify(resolved.route.params);
            if (this.menuItem.routeParamsJson === '{}') {
              this.menuItem.routeParamsJson = null;
            }
            this.menuItem.externalUrl = '';
            this.urlError = false;
          } else {
            this.menuItem.routeName = '';
            this.menuItem.routeParamsJson = '';
            this.menuItem.externalUrl = '';
            this.urlError = true;
          }
        }.bind(this);

        if (this.url.startsWith(config.SiteUrl)) {
          const lastPart = this.url.substring(config.SiteUrl.length);
          resolve(lastPart);
        } else if (this.url.startsWith('http://') || this.url.startsWith('https://')) {
          this.menuItem.routeName = '';
          this.menuItem.routeParamsJson = '';
          this.menuItem.externalUrl = this.url;
          this.urlError = false;
        } else {
          resolve(this.url);
        }
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

            if (!menuItem.icon)
              menuItem.icon = 'far fa-file';
          } else {
            root = menuItem;
            root.icon = 'fas fa-dot-circle';
          }
        }

        this.parentOptions = [root];
      },
      loadData() {
        this.$store.dispatch('request',
          {
            url: '/Admin/MenuAdmin/GetMenuItems',
          })
          .then(
            response => {
              this.prepareMenuItems(response.data);
            }
          ).catch(error => {
          this.$errorNotify(error);
        });
        this.$store.dispatch('request',
          {
            url: '/Admin/UserRolesAdmin/GetAllRoles'
          })
          .then(response => {
              this.allRoles = response.data;
              this.allRoles.push({
                name: 'Unregistered',
                title: 'Гость'
              });
              const menuItemRoles = this.menuItem.roles.split(',');
              this.roles = this.allRoles.filter(x => menuItemRoles.some(y => y === x.name));
            }
          );
      }
    },
    beforeCreate() {
      this.$options.components.LoaderWait = require('sun').LoaderWait;
    },
    async created() {
      this.rules = createRules.call(this);
      await this.loadData();
      this.getUrl();
    }
  }

</script>

<style lang="stylus">

  .menu-item-form {
    .q-tree__node-header {
      padding: 2px 4px !important;
      margin: 0;
    }
  }

</style>
