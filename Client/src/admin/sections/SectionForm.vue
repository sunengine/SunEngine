<template>
	<div class="section-form q-gutter-sm">
		<q-markup-table
			flat
			bordered
			wrap-cells
			v-if="section.options && section.options.length > 0 && allRoles"
		>
			<tr>
				<td class="config-item__name-column">{{ $tl("type") }}</td>
				<td>
					{{ $t(`SectionsEditor.${section.type}.name`) }}
				</td>
			</tr>
			<tr>
				<td class="config-item__name-column">{{ $tl("name") }}</td>
				<td>
					<q-input
						dense
						hide-bottom-space
						hide-hint
						class="section-form__name"
						ref="name"
						v-model="section.name"
						:rules="rules.name"
					/>
				</td>
			</tr>
			<tr>
				<td class="config-item__name-column">{{ $tl("token") }}</td>
				<td>
					<q-input
						dense
						hide-bottom-space
						hide-hint
						class="section-form__name"
						ref="token"
						v-model="section.token"
						:rules="rules.token"
					/>
				</td>
			</tr>
			<tr>
				<td class="config-item__name-column">{{ $tl("roles") }}</td>
				<td>
					<q-select
						dense
						v-if="allRoles"
						class="section-form__title"
						v-model="roles"
						:options="allRoles"
						multiple
						use-chips
						stack-label
						option-value="name"
						option-label="title"
					/>
					<LoaderWait v-else />
				</td>
			</tr>
			<tr>
				<td class="config-item__name-column">{{ $tl("isCacheData") }}</td>
				<td>
					<q-checkbox
						dense
						class="section-form__is-cache-data"
						ref="isCacheData"
						v-model="section.isCacheData"
					/>
				</td>
			</tr>
			<tr :key="configItem.name" v-for="configItem of section.options">
				<td>{{ $t(`SectionsEditor.${section.type}.${configItem.name}`) }}</td>
				<td>
					<ConfigItem ref="configItem" :allRoles="allRoles" :item="configItem" :enums="section.enums" />
				</td>
			</tr>
		</q-markup-table>
	</div>
</template>

<script>
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
		token: [
			value =>
				!value ||
				value.length <= config.DbColumnSizes.Sections_Token ||
				this.$tl("validation.token.maxLength"),
			value =>
				/^[a-zA-Z0-9-]*$/.test(value) || this.$tl("validation.token.allowedChars")
		],
		type: [value => !!value || this.$tl("validation.type.required")]
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
				this.$refs.name.hasError || this.$refs["configItem"].some(x => x.hasError)
			);
		}
	},
	methods: {
		rolesUpdated() {
			this.section.roles = this.roles.map(x => x.name).join(",");
		},
		validate() {
			this.$refs.name.validate();
			this.$refs["configItem"].forEach(x => x.validate());
		},
		loadRoles() {
			this.$request(this.$AdminApi.UserRolesAdmin.GetAllRoles).then(response => {
				this.allRoles = response.data;
				this.allRoles.push({
					name: "Unregistered",
					title: "Unregistered"
				});
				const sectionRoles = this.section.roles.split(",");
				this.roles = this.allRoles.filter(x =>
					sectionRoles.some(y => y === x.name)
				);
			});
		}
	},
	beforeCreate() {
		this.$options.components.ConfigItem = sunRequire("ConfigItem","admin");
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
