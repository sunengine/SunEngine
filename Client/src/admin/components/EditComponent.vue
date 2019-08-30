<template>
  <q-page class="page-padding">
    <h2 class="q-title">
      {{title}}
    </h2>
    <ComponentForm v-if="component" ref="form" class="q-mb-xl" :component="component"/>
    <LoaderWait v-else/>
    <q-btn icon="far fa-save" class="send-btn" no-caps :loading="loading" :label="$tl('saveBtn')" @click="save"
           color="send">
      <LoaderSent slot="loading"/>
    </q-btn>
    <q-btn no-caps icon="fas fa-times" class="cancel-btn q-ml-sm" @click="$router.back()" :label="$tl('cancelBtn')"
           color="warning"/>
    <q-btn no-caps icon="far fa-times-circle" class="delete-btn q-ml-sm float-right" @click="removeComponent()"
           :label="$tl('deleteBtn')"/>
  </q-page>
</template>

<script>
    import {Page} from 'sun'


    export default {
        name: "EditComponent",
        mixins: [Page],
        props: {
            name: {
                type: String,
                required: true
            }
        },
        data() {
            return {
                component: null,
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

                this.$store.dispatch('request',
                    {
                        url: '/Admin/ComponentsAdmin/UpdateComponent',
                        data: this.component,
                        sendAsJson: true
                    })
                    .then(async () => {
                        this.$successNotify();
                        this.$router.push({name: 'ComponentsAdmin'});
                    }).catch(error => {
                    this.$errorNotify(error);
                    this.loading = false;
                });
            },
            loadData() {
                this.$store.dispatch('request',
                    {
                        url: '/Admin/ComponentsAdmin/GetComponent',
                        data: {
                            name: this.name
                        }
                    })
                    .then(response => {
                        this.component = response.data;
                    });
            }
        },
        beforeCreate() {
            this.$options.components.LoaderSent = require('sun').LoaderSent;
            this.$options.components.LoaderWait = require('sun').LoaderWait;
            this.$options.components.ComponentForm = require('sun').ComponentForm;
        },
        created() {
            this.title = this.$tl('title');
            this.loadData();
        }
    }

</script>

<style scoped>

</style>
