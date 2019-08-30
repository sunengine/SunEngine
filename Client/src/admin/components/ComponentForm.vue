<template>
  <div class="component-form q-gutter-sm">
    <q-input ref="name" v-model="component.name" :label="$tl('name')" :rules="rules.name">
      <template v-slot:prepend>
        <q-icon name="fas fa-signature" class="q-mr-xs"/>
      </template>
    </q-input>

    <q-select class="q-mb-lg" emit-value map-options :label="$tl('type')" v-model="component.type"
              :options="['Posts','Activities']">
      <q-icon slot="prepend" name="fas fa-boxes"/>
    </q-select>

    <q-input ref="serverSettingsJson" type="textarea" v-model="component.serverSettingsJson" autogrow :label="$tl('serverSettingsJson')"
             :rules="rules.serverSettingsJson"/>

    <q-input ref="clientSettingsJson" type="textarea" v-model="component.clientSettingsJson" autogrow :label="$tl('clientSettingsJson')"
             :rules="rules.clientSettingsJson"/>

    <q-checkbox ref="isCacheData" v-model="component.isCacheData" :label="$tl('isCacheData')"/>
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
            }
        },
        created() {
            this.rules = createRules.call(this);
        }
    }
</script>

<style lang="stylus">

  .component-form {

  }

</style>
