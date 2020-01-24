<template>
	<div class="component-form q-gutter-xs">
		<q-input
			class="component-form__name"
			ref="name"
			v-model="component.name"
			:label="$tl('name')"
			:rules="rules.name"
		/>

		<q-select
			class="component-form__type"
			ref="type"
			:disable="editMode"
			emit-value
			map-options
			:label="$tl('type')"
			:rules="rules.type"
			v-model="component.type"
			:options="componentTypes"
			option-value="name"
			option-label="title"
		>
			<q-icon slot="prepend" :name="$iconsSet.ComponentForm.component" />
		</q-select>

		<q-input
			class="component-form__server-settings-json"
			ref="serverSettingsJson"
			type="textarea"
			v-model="component.serverSettingsJson"
			autogrow
			:label="$tl('serverSettingsJson')"
			:rules="rules.serverSettingsJson"
		/>

		<q-input
			class="component-form__client-settings-json"
			ref="clientSettingsJson"
			type="textarea"
			v-model="component.clientSettingsJson"
			autogrow
			:label="$tl('clientSettingsJson')"
			:rules="rules.clientSettingsJson"
		/>

		<q-select
			bottom-slots
			v-if="allRoles"
			class="component-form__title"
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
			class="component-form__is-cache-data"
			ref="isCacheData"
			v-model="component.isCacheData"
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
		};
	},
	watch: {
		roles: "rolesUpdated",
		"component.type": "typeChanges"
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
		componentTypes() {
			return Object.values(this.$store.state.components.componentsTypes);
		}
	},
	methods: {
		typeChanges() {
			const type = this.$store.getters.getComponentType(this.component.type);
			this.component.serverSettingsJson = JSON.stringify(
				type.getServerTemplate(),
				null,
				2
			);
			this.component.clientSettingsJson = JSON.stringify(
				type.getClientTemplate(),
				null,
				2
			);
		},
		rolesUpdated() {
			this.component.roles = this.roles.map(x => x.name).join(",");
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
				const componentRoles = this.component.roles.split(",");
				this.roles = this.allRoles.filter(x =>
					componentRoles.some(y => y === x.name)
				);
			});
		}
	},
	beforeCreate() {
		this.$options.components.LoaderWait = require("sun").LoaderWait;
	},
	created() {
		this.rules = createRules.call(this);
		this.loadRoles();
	}
};
</script>

<style lang="scss">
.component-form {
}
</style>
