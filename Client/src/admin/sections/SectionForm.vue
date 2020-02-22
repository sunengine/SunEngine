<template>
	<div class="section-form q-gutter-sm">
		<q-markup-table
			flat
			bordered
			wrap-cells
			v-if="section.options && section.options.length > 0"
		>
			<tr>
				<td class="config-item__name-column">{{ $tl("name") }}</td>
				<td>
					<q-input
						class="section-form__name"
						ref="name"
						v-model="section.name"
						:rules="rules.name"
					/>
				</td>
			</tr>
			<tr>
				<td class="config-item__name-column">{{ $tl("type") }}</td>
				<td>
					<q-select
						class="section-form__type"
						ref="type"
						:disable="editMode"
						emit-value
						map-options
						:rules="rules.type"
						v-model="section.type"
						:options="sectionTypes"
						option-value="name"
						option-label="title"
					>
						<q-icon slot="prepend" :name="$iconsSet.SectionForm.section" />
					</q-select>
				</td>
			</tr>
			<tr>
				<td class="config-item__name-column">{{ $tl("roles") }}</td>
				<td>
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
					/>
					<LoaderWait v-else />
				</td>
			</tr>
			<tr>
				<td class="config-item__name-column">{{ $tl("isCacheData") }}</td>
				<td>
					<q-checkbox
						class="section-form__is-cache-data"
						ref="isCacheData"
						v-model="section.isCacheData"
					/>
				</td>
			</tr>
			<tr :key="configItem.name" v-for="configItem of section.options">
				<td>{{ configItem.name }}</td>
				<td>
					<ConfigItem ref="configItem" :item="configItem" :enums="section.enums" />
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
				this.$refs.name.hasError ||
				this.$refs.type.hasError ||
				this.$refs["configItem"].some(x => x.hasError)
			);
		},
		sectionTypes() {
			return Object.values(this.$store.state.sections.sectionsTypes);
		}
	},
	methods: {
		typeChanges() {
			const type = this.$store.getters.getSectionType(this.section.type);
		},
		rolesUpdated() {
			this.section.roles = this.roles.map(x => x.name).join(",");
		},
		validate() {
			this.$refs.name.validate();
			this.$refs.type.validate();
			this.$refs["configItem"].forEach(x => x.validate());
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
