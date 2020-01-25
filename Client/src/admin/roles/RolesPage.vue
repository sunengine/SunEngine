<template>
	<SunPage class="roles-page page-padding">
		<PageHeader :title="title" />

		<div class="row">
			<div class="col-xs-12 col-sm-4">
				<template v-if="roles">
					<div class="roles-page__header q-mb-sm">
						<q-icon :name="$iconsSet.RolesPage.users" class="q-mr-sm" />
						{{ $tl("roles") }}
					</div>

					<div class="roles-page__list ">
						<div class="roles-page__role" :key="role.id" v-for="role in roles">
							<router-link
								class="roles-page__role-link link"
								:to="{ name: 'RoleUsers', params: { roleName: role.name } }"
							>
								{{ role.title }}
							</router-link>
						</div>
					</div>
				</template>
				<div v-else>
					<LoaderWait />
				</div>
			</div>

			<router-view
				class="roles-page__router-view col-xs-12 col-sm-8"
			></router-view>
		</div>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "RolesPage",
	mixins: [Page],
	data() {
		return {
			roles: null,
			currentRole: null
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Admin");
		}
	},

	methods: {
		loadAllRoles() {
			this.$request(this.$AdminApi.UserRolesAdmin.GetAllRoles).then(response => {
				this.roles = response.data;
			});
		}
	},
	beforeCreate() {
		this.$options.components.LoaderWait = require("sun").LoaderWait;
	},
	created() {
		this.title = this.$tl("title");
		this.loadAllRoles();
	}
};
</script>

<style lang="scss">
.roles-page__header {
	background-color: $grey-4;
	padding: 10px;
}

.roles-page__list {
	padding-right: 16px;

	.router-link-exact-active {
		background: #e1e1e1;
	}
}

.roles-page__role {
	margin: 1px 0;
}

.roles-page__role-link {
	display: block;
	padding: 3px 10px;
}

.roles-page__router-view {
}
</style>
