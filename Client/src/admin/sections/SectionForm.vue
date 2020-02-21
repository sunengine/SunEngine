<template>
	<div class="section-form q-gutter-sm">
		<q-input
			class="section-form__name"
			ref="name"
			v-model="section.name"
			:label="$tl('name')"
			:rules="rules.name"
		/>

		<q-select
			class="section-form__type"
			ref="type"
			:disable="editMode"
			emit-value
			map-options
			:label="$tl('type')"
			:rules="rules.type"
			v-model="section.type"
			:options="sectionTypes"
			option-value="name"
			option-label="title"
		>
			<q-icon slot="prepend" :name="$iconsSet.SectionForm.section" />
		</q-select>

		<q-markup-table
			wrap-cells
			v-if="section.configItems && section.configItems.length > 0"
		>
			<tr :key="configItem.name" v-for="configItem of section.configItems">
				<td>{{ configItem.name }}</td>
				<td><ConfigItem :item="configItem" :enums="section.enums" /></td>
			</tr>
		</q-markup-table>

		<q-select
			bottom-slots
			v-if="allRoles"
			class="section-form__title"
			v-model="roles"
			:options="allRoles"
			multiple
			use-chips
			stack-label
			option-value="name"
			option-label="title"
			:label="$tl('roles')"
		/>

		<LoaderWait v-else />

		<q-checkbox
			class="section-form__is-cache-data"
			ref="isCacheData"
			v-model="section.isCacheData"
			:label="$tl('isCacheData')"
		/>
	</div>
</template>

<script>
import { isJson } from "sun";

function createRules() {
	return {
		name: [
			value => !!value || this.$tl("validation.name.required"),
			value =>
				!value || value.length >= 3 || this.$tl("validation.name.minLength"),
			value =>
				!value ||
				value.length <= config.DbColumnSizes.MenuItems_Name ||
				this.$tl("validation.name.maxLength"),
			value =>
				/^[a-zA-Z0-9_-]*$/.test(value) || this.$tl("validation.name.allowedChars")
		],
		type: [value => !!value || this.$tl("validation.type.required")],
		serverSettingsJson: [
			value => !value || isJson(value) || this.$tl("validation.jsonFormatError")
		],
		clientSettingsJson: [
			value => !value || isJson(value) || this.$tl("validation.jsonFormatError")
		]
	};
}

export default {
	name: "SectionForm",
	props: {
		section: {
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
		};
	},
	watch: {
		roles: "rolesUpdated",
		"section.type": "typeChanges"
	},
	computed: {
		hasError() {
			return (
				this.$refs.name.hasError ||
				this.$refs.type.hasError ||
				this.$refs.serverSettingsJson.hasError ||
				this.$refs.clientSettingsJson.hasError
			);
		},
		sectionTypes() {
			return Object.values(this.$store.state.sections.sectionsTypes);
		}
	},
	methods: {
		typeChanges() {
			const type = this.$store.getters.getSectionType(this.section.type);
			this.section.serverSettingsJson = JSON.stringify(
				type.getServerTemplate(),
				null,
				2
			);
			this.section.clientSettingsJson = JSON.stringify(
				type.getClientTemplate(),
				null,
				2
			);
		},
		rolesUpdated() {
			this.section.roles = this.roles.map(x => x.name).join(",");
		},
		validate() {
			this.$refs.name.validate();
			this.$refs.type.validate();
			this.$refs.serverSettingsJson.validate();
			this.$refs.clientSettingsJson.validate();
		},
		loadRoles() {
			this.$request(this.$AdminApi.UserRolesAdmin.GetAllRoles).then(response => {
				this.allRoles = response.data;
				this.allRoles.push({
					name: "Unregistered",
					title: "Гость"
				});
				const sectionRoles = this.section.roles.split(",");
				this.roles = this.allRoles.filter(x =>
					sectionRoles.some(y => y === x.name)
				);
			});
		}
	},
	beforeCreate() {
		this.$options.components.ConfigItem = require("sun").ConfigItem;
	},
	created() {
		this.rules = createRules.call(this);
		this.loadRoles();
	}
};
</script>

<style lang="scss">
.section-form {
}
</style>
