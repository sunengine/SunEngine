<template>
	<SunPage class="configuration-admin page-padding">
		<PageHeader :title="title">
			<q-input
				dense
				v-model="filter"
				@input="doFilter"
				:placeholder="$tl('filter')"
				clearable
			>
				<template v-slot:before>
					<q-icon :name="$iconsSet.ConfigurationAdmin.search" />
				</template>
			</q-input>
		</PageHeader>

		<div v-if="configurationGroups">
			<q-markup-table wrap-cells>
				<tbody v-if="configurationGroupsFiltered">
					<template v-for="group of configurationGroupsFiltered">
						<tr class="configuration-admin__group-header-tr">
							<td
								colspan="2"
								class="configuration-admin__group-header-td q-gutter-y-sm"
							>
								<div class="configuration-admin__group-title">
									{{ group.localTitle }}
								</div>
								<div
									class="configuration-admin__group-sub-title"
									v-html="group.localSubTitle"
								></div>
							</td>
						</tr>
						<tr v-for="item of group.items">
							<td class="configuration-admin__name-column">
								<div class="flex no-wrap align-center">
									<div>{{ item.localTitle }}</div>
									<q-space />
									<div v-if="item.localSubTitle">
										<q-icon
											:name="$iconsSet.ConfigurationAdmin.question"
											class="text-blue"
											size="xs"
											right
										>
											<q-tooltip
												anchor="bottom middle"
												self="top middle"
												max-width="200px"
											>
												{{ item.localSubTitle }}
											</q-tooltip>
										</q-icon>
									</div>
								</div>
							</td>
							<td class="configuration-admin__value-column">
								<q-checkbox dense v-if="item.type === 'Boolean'" v-model="item.value" />
								<q-select
									dense
									v-else-if="item.type === 'Enum'"
									:options="enums[item.enumName]"
									v-model="item.value"
								/>
								<q-input
									dense
									v-else
									:type="getTypeType(item.type)"
									:rules="item.type === 'JsonString' ? rules : null"
									v-model="item.value"
								/>
							</td>
						</tr>
					</template>
				</tbody>
				<tbody v-else>
					<tr>
						<td>{{ $tl("noResults") }}</td>
					</tr>
				</tbody>
			</q-markup-table>

			<q-page-sticky position="bottom-left" :offset="[34, 12]">
				<q-btn
					class="send-btn"
					@click="uploadConfiguration"
					no-caps
					:icon="$iconsSet.ConfigurationAdmin.save"
					:loading="loading"
					:label="$tl('saveBtn')"
				>
					<template v-slot:loading>
						<LoaderSent />
					</template>
				</q-btn>
			</q-page-sticky>
			<q-page-sticky position="bottom-right" :offset="[34, 12]">
				<q-btn
					class="reset-btn q-mr-md"
					@click="resetConfiguration"
					no-caps
					:icon="$iconsSet.ConfigurationAdmin.reset"
					:label="$tl('resetBtn')"
				>
					<q-tooltip :delay="800">
						{{ $tl("resetBtnTooltip") }}
					</q-tooltip>
				</q-btn>
				<q-btn
					class="cancel-btn"
					@click="$router.back()"
					no-caps
					:icon="$iconsSet.ConfigurationAdmin.cancel"
					:label="$tl('cancelBtn')"
				/>
			</q-page-sticky>

			<br />
		</div>

		<LoaderWait v-else />
	</SunPage>
</template>

<script>
import { Page } from "mixins";
import { isJson } from "sun";

function createRules() {
	return [
		value => isJson(value) || this.$t("Global.validation.jsonFormatError")
	];
}

export default {
	name: "ConfigurationAdmin",
	mixins: [Page],
	data() {
		return {
			filter: "",
			configurationGroups: null,
			configurationGroupsFiltered: null,
			tokens: null,
			enums: null,
			loading: false
		};
	},
	watch: {
		filter: "buildTable"
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Admin");
		},
		filterLowerCase() {
			return this.filter.toLowerCase();
		}
	},
	methods: {
		filterItems() {
			if (!this.filter)
				this.configurationGroupsFiltered = this.configurationGroups;

			const rez = [];
			for (const group of this.configurationGroups) {
				if (group.filterStr.includes(this.filterLowerCase)) {
					rez.push(group);
					continue;
				}

				const { items, ...grp } = group;
				grp.items = [];

				for (const item of group.items)
					if (item.filterStr.includes(this.filterLowerCase)) grp.items.push(item);

				if (grp.items.length > 0) rez.push(grp);
			}

			this.configurationGroupsFiltered = rez.length > 0 ? rez : null;
		},
		getItemTitle(item) {
			const key = this.$options.name + ".items." + item.name;
			if (this.$te(key) && this.$t(key)) return this.$t(key);
			else return item.name;
		},
		getItemSubTitle(item) {
			const key = this.$options.name + ".tooltips." + item.name;
			if (this.$te(key) && this.$t(key)) return this.$t(key);
			else return null;
		},
		getGroupTitle(group) {
			const key = this.$options.name + ".groupTitles." + group.name;
			if (this.$te(key) && this.$t(key)) return this.$t(key);
			else return group.name;
		},
		getGroupSubTitle(group) {
			const key = this.$options.name + ".groupSubTitles." + group.name;
			if (this.$te(key) && this.$t(key))
				return this.$t(key).replace(
					/((http:\/\/|https:\/\/)[^\s]+?)(\s|$)/gi,
					'<a href="$1" target="_blank">$1</a>'
				);
			else return null;
		},
		getTypeType(type) {
			switch (type) {
				case "String":
					return "text";
				case "LongString":
					return "textarea";
				case "JsonString":
					return "textarea";
				case "Number":
					return "number";
			}
		},
		getEnum(name) {
			return this.enums[name];
		},
		getEnums() {
			return this.$request(this.$AdminApi.ConfigurationAdmin.GetEnums).then(
				response => {
					this.enums = response.data;
				}
			);
		},
		resetConfiguration() {
			return this.loadConfiguration().then(_ => {
				this.$successNotify(this.$tl("resetSuccessNotify"), "info");
			});
		},
		buildTable(items) {
			function GetTokens(name) {
				let arr = name.split(":");
				return [arr[0], arr.splice(1).join(":")];
			}

			const MakeGroupTitles = group => {
				group.localTitle = this.getGroupTitle(group);
				group.localSubTitle = this.getGroupSubTitle(group);
				group.filterStr = (
					group.name +
					" ! " +
					group.localTitle +
					" ! " +
					group.localSubTitle
				).toLocaleLowerCase();
			};

			const MakeItemTitles = item => {
				item.localTitle = this.getItemTitle(item);
				item.localSubTitle = this.getItemSubTitle(item);
				item.filterStr = (
					item.name +
					" ! " +
					item.localTitle +
					" ! " +
					item.localSubTitle
				).toLocaleLowerCase();
			};

			const toks0 = GetTokens(items[0].name);

			const newItem = {
				shortName: toks0[1],
				...items[0]
			};
			MakeItemTitles(newItem);

			const groups = [
				{
					name: toks0[0],
					items: [newItem]
				}
			];
			MakeGroupTitles(groups[0]);

			newItem.group = groups[0];

			items.reduce((previousValue, currentValue, index, array) => {
				const toks1 = GetTokens(previousValue.name);
				const toks2 = GetTokens(currentValue.name);

				const newItem = {
					shortName: toks2[1],
					...currentValue
				};
				MakeItemTitles(newItem);

				if (toks1[0] === toks2[0]) {
					groups[groups.length - 1].items.push(newItem);
				} else {
					const newGroup = {
						name: toks2[0],
						fullName: currentValue.name,
						items: [newItem]
					};
					MakeGroupTitles(newGroup);

					groups.push(newGroup);
				}

				return currentValue;
			});

			this.configurationGroups = groups;
		},
		doFilter() {
			this.filterItems();
		},
		loadConfiguration() {
			return this.$request(
				this.$AdminApi.ConfigurationAdmin.LoadConfiguration
			).then(response => {
				this.buildTable(response.data);
			});
		},
		uploadConfiguration() {
			const data = new FormData();

			for (const group of this.configurationGroups)
				for (const ci of group.items) data.append(ci.name, ci.value);

			this.loading = true;

			return this.$request(
				this.$AdminApi.ConfigurationAdmin.UploadConfiguration,
				data
			).then(_ => {
				this.$successNotify();
				this.loading = false;
				this.loadConfiguration();
			});
		}
	},
	beforeCreate() {
		this.$options.components.LoaderWait = require("sun").LoaderWait;
		this.$options.components.LoaderSent = require("sun").LoaderSent;
	},
	async created() {
		this.filterItems = this.$throttle(this.filterItems, 1000);
		this.title = this.$tl("title");
		this.rules = createRules.call(this);
		await this.getEnums();
		await this.loadConfiguration();
		this.doFilter();
	}
};
</script>

<style lang="scss">
.configuration-admin__table {
	width: 100%;
}

.configuration-admin__group-header-tr {
}

.configuration-admin__group-header-td {
	padding: 7px !important;
	text-align: center;
	background-color: $grey-3 !important;
}

.configuration-admin__group-title {
	font-size: 1.15em;
}

.configuration-admin__group-sub-title {
	color: $grey-7;
	font-size: 1em;
}

.configuration-admin__name-column {
	width: 150px !important;
}

.configuration-admin__value-column {
	textarea {
		height: 70px;
	}
}
</style>
