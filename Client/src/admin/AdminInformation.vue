<template>
	<SunPage class="admin-page page-padding">
		<PageHeader :title="title" />

		<q-expansion-item
			class="q-mb-md shadow-1 overflow-hidden"
			style="border-radius: 24px"
			expand-icon-class="text-white"
			header-class="bg-pink-11 text-white"
			:label="$tl('support')"
			:icon="$iconsSet.AdminInformation.support"
		>
			<div class="q-pa-lg">
				{{ $tl("supportInfo") }}
				<ul>
					<li>
						<a
							href="https://my.qiwi.com/Dmytryi-P9qqHzXldd?noCache=true"
							target="_blank"
							class="link"
						>
							QIWI
						</a>
					</li>
					<li>
						<a href="https://yasobe.ru/na/SunEngine" target="_blank" class="link"
							>Yandex.Money</a
						>
					</li>
					<li>
						<a href="https://paypal.me/dmitrijpolyanin" target="_blank" class="link">
							PayPal
						</a>
					</li>
				</ul>
			</div>
		</q-expansion-item>

		<q-markup-table>
			<tbody>
				<tr v-if="siteName">
					<td>{{ $tl("siteName") }}</td>
					<td>{{ siteName }}</td>
				</tr>
				<tr v-if="serverInfo && serverInfo.Name">
					<td>{{ $tl("serverName") }}</td>
					<td>{{ serverInfo.Name }}</td>
				</tr>
				<tr v-if="serverInfo && serverInfo.ServerVersion">
					<td>{{ $tl("serverVersion") }}</td>
					<td>{{ serverInfo.ServerVersion }}</td>
				</tr>
				<tr v-if="serverInfo && serverInfo.ServerRepository">
					<td>{{ $tl("serverRepository") }}</td>
					<td>
						<a :href="serverInfo.ServerRepository" target="_blank">{{
							serverInfo.ServerRepository
						}}</a>
					</td>
				</tr>
				<tr v-if="sunEngineVersion">
					<td>{{ $tl("sunEngineVersion") }}</td>
					<td>{{ sunEngineVersion }}</td>
				</tr>
				<tr v-if="showClientName && clientName">
					<td>{{ $tl("clientName") }}</td>
					<td>{{ clientName }}</td>
				</tr>
				<tr v-if="clientVersion !== sunEngineVersion && clientVersion">
					<td>{{ $tl("clientVersion") }}</td>
					<td>{{ clientVersion }}</td>
				</tr>
				<tr v-if="dotNetVersion">
					<td>{{ $tl("dotNetVersion") }}</td>
					<td>{{ dotNetVersion }}</td>
				</tr>
				<tr v-if="quasarVersion">
					<td>{{ $tl("quasarVersion") }}</td>
					<td>{{ quasarVersion }}</td>
				</tr>
				<tr v-if="vueJsVersion">
					<td>{{ $tl("vueJsVersion") }}</td>
					<td>{{ vueJsVersion }}</td>
				</tr>
				<tr v-if="serverInfo && serverInfo.Maintainer">
					<td>{{ $tl("maintainer") }}</td>
					<td>{{ serverInfo.Maintainer }}</td>
				</tr>
				<tr
					v-if="
						serverInfo &&
							serverInfo.MaintainerContacts &&
							serverInfo.MaintainerContacts.length > 0
					"
				>
					<td>{{ $tl("maintainerContacts") }}</td>
					<td class="q-gutter-y-xs">
						<div v-for="contact of serverInfo.MaintainerContacts">
							<a
								class="link"
								:href="contact"
								target="_blank"
								v-if="contact.startsWith('http')"
								>{{ contact }}</a
							>
							<span v-else>{{ contact }}</span>
						</div>
					</td>
				</tr>
				<tr v-if="serverInfo && serverInfo.Description">
					<td>{{ $tl("description") }}</td>
					<td>{{ serverInfo.Description }}</td>
				</tr>
				<template v-if="additionalData">
					<tr v-for="(value, name) in additionalData">
						<td>{{ name }}</td>
						<td>{{ value }}</td>
					</tr>
				</template>
				<tr>
					<td>{{ $tl("sunEngineRepository") }}</td>
					<td>
						<a
							class="link"
							href="https://github.com/sunengine/SunEngine"
							target="_blank"
							>https://github.com/sunengine/SunEngine</a
						>
					</td>
				</tr>
				<tr>
					<td>{{ $tl("sunEngineSkinsRepository") }}</td>
					<td>
						<a
							class="link"
							href="https://github.com/sunengine/SunEngine.Skins"
							target="_blank"
							>https://github.com/sunengine/SunEngine.Skins</a
						>
					</td>
				</tr>
			</tbody>
		</q-markup-table>
	</SunPage>
</template>

<script>
import { Page } from "mixins";
import Vue from "vue";

export default {
	name: "AdminInformation",
	mixins: [Page],
	data() {
		return {
			serverInfo: null,
			sunEngineVersion: null,
			dotNetVersion: null
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Admin");
		},
		siteName() {
			return config.Global.SiteName;
		},
		additionalData() {
			const {
				Name,
				ServerVersion,
				Maintainer,
				MaintainerContacts,
				Description,
				ServerRepository,
				...rez
			} = { ...this.serverInfo };
			return rez;
		},
		clientVersion() {
			return process.env.PACKAGE_JSON.version;
		},
		clientName() {
			return process.env.PACKAGE_JSON.name;
		},
		showClientName() {
			return this.clientName !== "sunengine-client";
		},
		quasarVersion() {
			return this.$q.version;
		},
		vueJsVersion() {
			return Vue.version;
		}
	},
	methods: {
		getServerInfo() {
			return this.$request(this.$AdminApi.ServerInfoAdmin.GetServerInfo).then(
				response => {
					this.serverInfo = response.data.ServerInfo;
				}
			);
		},
		getDotNetVersion() {
			return this.$request(this.$AdminApi.ServerInfoAdmin.DotnetVersion).then(
				response => {
					this.dotNetVersion = response.data;
				}
			);
		},
		getSunEngineVersion() {
			return this.$request(this.$AdminApi.ServerInfoAdmin.Version).then(
				response => {
					this.sunEngineVersion = response.data.version;
				}
			);
		}
	},
	async created() {
		this.title = this.$tl("title");
		await this.getServerInfo();
		await this.getSunEngineVersion();
		await this.getDotNetVersion();
	}
};
</script>

<style scoped></style>
