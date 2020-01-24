<template>
	<div class="role-users">
		<div class="role-users__header">
			<q-icon  :name="$iconsSet.RoleUsers.user" class="q-mr-sm" />
			{{ $tl("users") }}
		</div>

		<q-input
			class="role-users__filter q-my-md"
			outlined
			dense
			v-model="filter"
			:label="$tl('filter')"
			clearable
		>
			<template v-slot:prepend>
				<q-icon  :name="$iconsSet.RoleUsers.search" size="0.75em" />
			</template>
		</q-input>

		<div v-if="users" class="role-users__list">
			<div class="role-users__user" :key="user.id" v-for="user in users">
				<router-link
					class="role-users__user-link link"
					:to="`/user/${user.link}`"
					>{{ user.name }}</router-link
				>
			</div>
			<div v-if="users.length === 0" class="text-grey">{{ $tl("noResults") }}</div>
			<div v-if="users.length === maxUsersTake" class="text-grey">
				{{ $tl("filterLimitReached", maxUsersTake) }}
			</div>
		</div>

		<div v-else>
			<LoaderWait />
		</div>
	</div>
</template>

<script>
export default {
	name: "RoleUsers",
	props: {
		roleName: {
			type: String,
			required: true
		}
	},
	data() {
		return {
			users: null,
			filter: null
		};
	},
	maxUsersTake: null,
	methods: {
		loadRoleUsers() {
			this.users = null;

			this.$request(this.$AdminApi.UserRolesAdmin.GetRoleUsers, {
				roleName: this.roleName,
				userNamePart: this.filter
			}).then(response => {
				this.users = response.data;
			});
		}
	},
	beforeCreate() {
		this.maxUsersTake = config.Misc.AdminRoleUsersMaxUsersTake;
		this.$options.components.LoaderWait = require("sun").LoaderWait;
	},
	created() {
		this.loadRoleUsers();
		this.$watch("roleName", this.loadRoleUsers);
		this.loadRoleUsers = this.$throttle(this.loadRoleUsers);
		this.$watch("filter", this.loadRoleUsers);
	}
};
</script>

<style lang="scss">
.role-users__header {
	background-color: $grey-4;
	padding: 10px;
}

.role-users__list {
	padding: 0 10px;
}

.role-users_user-div {
	margin: 3px 0;
}

.role-users__user {
	margin: 0 0 3px 0;
}
</style>
