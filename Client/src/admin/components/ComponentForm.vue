<template>
  <div class="component-form q-gutter-sm">
    <q-input ref="name" v-model="component.name" :label="$tl('name')" :rules="rules.name">
      <template v-slot:prepend>
        <q-icon name="fas fa-signature" class="q-mr-xs"/>
      </template>
    </q-input>

    <q-select ref="type" :disable="editMode" class="q-mb-lg" emit-value map-options
              :label="$tl('type')" :rules="rules.type" v-model="component.type"
              :options="componentTypes" option-value="name" option-label="title">
      <q-icon slot="prepend" name="fas fa-cube"/>
    </q-select>

    <q-input ref="serverSettingsJson" type="textarea" v-model="component.serverSettingsJson" autogrow
             :label="$tl('serverSettingsJson')"
             :rules="rules.serverSettingsJson"/>

    <q-input ref="clientSettingsJson" type="textarea" v-model="component.clientSettingsJson" autogrow
             :label="$tl('clientSettingsJson')"
             :rules="rules.clientSettingsJson"/>

    <q-select v-if="allRoles" class="q-mb-md" v-model="roles" :options="allRoles" multiple use-chips stack-label
              option-value="name" option-label="title" :label="$tl('roles')"/>
    <LoaderWait v-else/>

    <q-checkbox ref="isCacheData" v-model="component.isCacheData" :label="$tl('isCacheData')"/>
  </div>
</template>

<script>
    import {isJson} from 'sun';


    function createRules() {
        return {
            name: [
                value => !!value || this.$tl('validation.name.required'),
                value => (!value || value.length >= 3) || this.$tl('validation.name.minLength'),
                value => (!value || value.length <= config.DbColumnSizes.MenuItems_Name) || this.$tl('validation.name.maxLength'),
                value => /^[a-zA-Z0-9_-]*$/.test(value) || this.$tl('validation.name.allowedChars'),
            ],
            type: [
                value => !!value || this.$tl('validation.type.required'),
            ],
            serverSettingsJson: [
                value => (!value || isJson(value)) || this.$tl('validation.jsonFormatError')
            ],
            clientSettingsJson: [
                value => (!value || isJson(value)) || this.$tl('validation.jsonFormatError')
            ]
        }
    }

    export default {
        name: "ComponentForm",
        props: {
            component: {
                type: Object,
                required: true
            },
            editMode: {
                type: Boolean,
                required: false,
                default: false
            }
        },
        data() {
            return {
                allRoles: null,
                roles: null
            }
        },
        watch: {
            'roles': 'rolesUpdated',
            'component.type': 'typeChanges'
        },
        computed: {
            hasError() {
                return this.$refs.name.hasError || this.$refs.type.hasError || this.$refs.serverSettingsJson.hasError || this.$refs.clientSettingsJson.hasError;
            },
            componentTypes() {
                return Object.values(this.$store.state.components.componentsTypes);
            }
        },
        methods: {
            typeChanges() {
                const type = this.$store.getters.getComponentType(this.component.type);
                this.component.serverSettingsJson = JSON.stringify(type.getServerTemplate(), null, 2);
                this.component.clientSettingsJson = JSON.stringify(type.getClientTemplate(), null, 2);
            },
            rolesUpdated() {
                this.component.roles = this.roles.map(x => x.name).join(',');
            },
            validate() {
                this.$refs.name.validate();
                this.$refs.type.validate();
                this.$refs.serverSettingsJson.validate();
                this.$refs.clientSettingsJson.validate();
            },
            loadRoles() {
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
                            const componentRoles = this.component.roles.split(',');
                            this.roles = this.allRoles.filter(x => componentRoles.some(y => y === x.name));
                        }
                    );
            }
        },
        beforeCreate() {
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        },
        created() {
            this.rules = createRules.call(this);
            this.loadRoles();
        }
    }
</script>

<style lang="stylus">

  .component-form {

  }

</style>
