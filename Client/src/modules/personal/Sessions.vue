<template>
  <q-page class="sessions">
    <q-table v-if="sessions" :rows-per-page-options="[0]" @selection="updateSelected"
             :title="title"
             :data="sessions"
             :columns="columns"
             row-key="id"
             selection="multiple" hide-bottom
             :selected.sync="selected"
    >

      <template v-slot:header="props">
        <q-tr :props="props">
          <q-th auto-width class="text-center">

          </q-th>
          <q-td auto-width>
            <q-icon name="fas fa-desktop" class="q-mr-md"/>
            {{$tl('deviceInfo')}}
          </q-td>
          <q-td auto-width>
            <q-icon name="far fa-clock" class="q-mr-md"/>
            {{$tl('updateDate')}}
          </q-td>
        </q-tr>
      </template>

      <template v-slot:body="props">
        <q-tr :props="props" :class="{sessions__current: props.row.isCurrent}">
          <q-td auto-width class="text-center">
            <q-checkbox v-if="!props.row.isCurrent" v-model="props.selected"/>
          </q-td>
          <q-td auto-width>
            {{props.row.deviceInfo}}
            <q-badge class="q-ml-sm" v-if="props.row.isCurrent">{{$tl('current')}}</q-badge>
          </q-td>
          <q-td auto-width>
            {{$formatDate(props.row.updateDate)}}
          </q-td>
        </q-tr>
      </template>
    </q-table>
    <LoaderWait v-else/>

    <div class="text-center">
      <q-btn class="q-mt-md delete-btn" icon="fas fa-sign-out-alt" :label="$tl('logout')" @click="deleteSessions"
             v-if="selected.length" no-caps/>
    </div>
  </q-page>
</template>

<script>
    import {Page} from 'mixins';

    export default {
        name: "Sessions",
        mixins: [Page],
        data() {
            return {
                sessions: null,
                selected: []
            }
        },
        computed: {
            columns() {
                return [
                    //{name: 'Id',  align: 'left', label: 'Id', field: 'id', sortable: false},
                    {
                        name: 'deviceInfo',
                        align: 'left',
                        label: this.$tl('deviceInfo'),
                        field: 'deviceInfo',
                        sortable: false
                    }
                ]
            }
        },
        methods: {
            deleteSessions() {
                let sessions = this.selected.filter(x => !x.isCurrent);
                if (sessions.length === 0)
                    return;

                this.$request(
                    this.$Api.Personal.RemoveMySessions,
                    {
                        sessions: sessions.map(x => x.id).join(",")
                    }
                ).then((response) => {
                    this.$successNotify(null, 'warning');
                    this.selected = [];
                    this.loadData();
                });
            },
            loadData() {
                this.$request(
                    this.$Api.Personal.GetMySessions
                ).then(response => {
                    this.sessions = response.data;
                });
            }
        },
        beforeCreate() {
            this.$options.components.LoaderWait = require('sun').LoaderWait
        },
        created() {
            this.title = this.$tl('title');
            this.loadData();
        }
    }
</script>

<style lang="scss">

  .sessions__current {
    background-color: #efffec;
  }

</style>
