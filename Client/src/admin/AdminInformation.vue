<template>
  <q-page class="admin-page page-padding">
    <h2 class="page-title ">
      {{title}}
    </h2>
    <q-markup-table >
      <tbody>
      <tr v-if="serverInfo && serverInfo.Name">
        <td>{{$tl("serverName")}}</td>
        <td>{{serverInfo.Name}}</td>
      </tr>
      <tr v-if="serverInfo && serverInfo.ServerVersion">
        <td>{{$tl("serverVersion")}}</td>
        <td>{{serverInfo.ServerVersion}}</td>
      </tr>
      <tr v-if="serverInfo && serverInfo.ServerRepository">
        <td>{{$tl("serverRepository")}}</td>
        <td><a :href="serverInfo.ServerRepository" target="_blank">{{serverInfo.ServerRepository}}</a></td>
      </tr>
      <tr v-if="sunEngineVersion">
        <td>{{$tl("sunEngineVersion")}}</td>
        <td>{{sunEngineVersion}}</td>
      </tr>
      <tr v-if="dotNetVersion">
        <td>{{$tl("dotNetVersion")}}</td>
        <td>{{dotNetVersion}}</td>
      </tr>
      <tr v-if="serverInfo && serverInfo.Maintainer">
        <td>{{$tl("maintainer")}}</td>
        <td>{{serverInfo.Maintainer}}</td>
      </tr>
      <tr v-if="serverInfo && serverInfo.MaintainerContacts && serverInfo.MaintainerContacts.length > 0 ">
        <td>{{$tl("maintainerContacts")}}</td>
        <td class="q-gutter-y-xs">
          <div v-for="contact of serverInfo.MaintainerContacts">
            <a :href="contact" target="_blank" v-if="contact.startsWith('http')">{{contact}}</a>
            <span v-else>{{contact}}</span>
          </div>
        </td>
      </tr>
      <tr v-if="serverInfo && serverInfo.Description">
        <td>{{$tl("description")}}</td>
        <td>{{serverInfo.Description}}</td>
      </tr>
      <template v-if="additionalData">
        <tr v-for="(value, name) in additionalData">
          <td>{{name}}</td>
          <td>{{value}}</td>
        </tr>
      </template>
      <tr>
        <td>{{$tl("sunEngineRepository")}}</td>
        <td><a href="https://github.com/sunengine/SunEngine" target="_blank">https://github.com/sunengine/SunEngine</a>
        </td>
      </tr>
      <tr>
        <td>{{$tl("sunEngineSkinsRepository")}}</td>
        <td><a href="https://github.com/sunengine/SunEngine.Skins" target="_blank">https://github.com/sunengine/SunEngine.Skins</a>
        </td>
      </tr>
      </tbody>
    </q-markup-table>

  </q-page>
</template>

<script>
    export default {
        name: "AdminInformation",
        data() {
            return {
                serverInfo: null,
                sunEngineVersion: null,
                dotNetVersion: null
            }
        },
        computed: {
            additionalData() {
                const {Name, ServerVersion, Maintainer, MaintainerContacts, Description, ServerRepository, ...rez} = {...this.serverInfo};
                return rez;
            }
            /* clentVersion() {
                 return process.env.PACKAGE_JSON.version;
             },
             clentName() {
                 return process.env.PACKAGE_JSON.version;
             }*/
        },
        methods: {
            getServerInfo() {
                return this.$request(
                    this.$AdminApi.ServerInfoAdmin.GetServerInfo
                ).then(response => {
                    this.serverInfo = response.data.ServerInfo;
                });
            },
            getDotNetVersion() {
                return this.$request(
                    this.$AdminApi.ServerInfoAdmin.DotnetVersion
                ).then(response => {
                    this.dotNetVersion = response.data;
                });
            },
            getSunEngineVersion() {
                return this.$request(
                    this.$AdminApi.ServerInfoAdmin.Version
                ).then(response => {
                    this.sunEngineVersion = response.data.version;
                });
            }
        },
        async created() {
            this.title = this.$tl("title");
            await this.getServerInfo();
            await this.getSunEngineVersion();
            await this.getDotNetVersion();
        }
    }
</script>

<style scoped>

</style>
