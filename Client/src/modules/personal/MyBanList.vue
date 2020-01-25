<template>
	<SunPage class="my-ban-list page-padding">
		<PageHeader :title="$tl('title')" :subTitle="$tl('subTitle')" />
		<div v-if="users">
			<template v-if="users.length > 0">
				<router-link
					:key="user.id"
					class="my-ban-list__user-link link block q-mb-xs"
					:to="{ name: 'User', params: { link: user.link } }"
					v-for="user in users"
					><q-icon name="far fa-user" /> {{ user.name }}
				</router-link>
			</template>
			<q-banner v-else rounded class="bg-grey-2 text-grey-8">
				{{ $tl("voidResult") }}
			</q-banner>
		</div>
		<LoaderWait v-else />
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "MyBanList",
	mixins: [Page],
	data() {
		return {
			users: null
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Personal");
		}
	},
	methods: {
		loadData() {
			this.$request(this.$Api.Personal.GetMyBanList).then(response => {
				this.users = response.data;
			});
		}
	},
	created() {
		this.title = this.$tl("title");
		this.loadData();
	}
};
</script>

<style lang="scss">
.my-ban-list__user-link {
	font-weight: 600;
}
</style>
