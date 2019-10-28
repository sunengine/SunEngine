<template>
  <q-page class="create-component page-padding">
    <h2 class="page-title">
      {{title}}
    </h2>

    <ComponentForm ref="form" :component="component"/>

    <div class="create-component__btn-block q-mt-lg q-gutter-md">
      <q-btn icon="fas fa-plus" class="send-btn" no-caps :loading="loading" :label="$tl('createBtn')" @click="save"
             color="send">
        <LoaderSent slot="loading"/>
      </q-btn>
      <q-btn no-caps icon="fas fa-times" class="cancel-btn" @click="$router.back()" :label="$tl('cancelBtn')"
             color="warning"/>
    </div>

  </q-page>
</template>

<script>
    import {Page} from 'mixins'


    export default {
        name: 'CreateComponent',
        mixins: [Page],
        data() {
            return {
                component: {
                    name: '',
                    type: '',
                    roles: 'Unregistered,Registered',
                    isCacheData: false,
                    clientSettingsJson: '{}',
                    serverSettingsJson: '{}'
                },
                loading: false
            }
        },
        methods: {
            save() {
                const form = this.$refs.form;
                form.validate();
                if (form.hasError)
                    return;

                this.loading = true;

                this.$request(
                    this.$AdminApi.ComponentsAdmin.AddComponent,
                    this.component,
                    true
                ).then(async () => {
                    this.$successNotify();
                    await this.$store.dispatch('loadAllComponents');
                    await this.$store.dispatch('setAllRoutes');
                    this.$router.push({name: 'ComponentsAdmin'});
                }).catch(error => {
                    this.$errorNotify(error);
                    this.loading = false;
                });
            }
        },
        beforeCreate() {
            this.$options.components.LoaderSent = require('sun').LoaderSent;
            this.$options.components.ComponentForm = require('sun').ComponentForm;
        },
        created() {
            this.title = this.$tl('title');
        }
    }

</script>

<style scoped>

</style>
