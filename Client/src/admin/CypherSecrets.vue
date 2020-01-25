<template>
	<SunPage class="cypher-secrets page-padding">
		<PageHeader :title="title" />

		<div class="cypher-secrets__btn-block q-gutter-md q-mt-sm">
			<q-btn
				class="cypher-secrets__short-jwt"
				no-caps
				:icon="$iconsSet.CypherSecrets.key"
				@click="shortJwt"
				color="warning"
				label="ShortJwt"
			/>
			<q-btn
				class="cypher-secrets__short-long-2jwt"
				no-caps
				:icon="$iconsSet.CypherSecrets.key"
				@click="long2Jwt"
				color="warning"
				label="Long2Jwt"
			/>
			<q-btn
				class="cypher-secrets__short-email-change"
				no-caps
				:icon="$iconsSet.CypherSecrets.key"
				@click="emailChange"
				color="warning"
				label="EmailChange"
			/>
		</div>
	</SunPage>
</template>

<script>
export default {
	name: "CypherSecrets",
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Admin");
		}
	},
	methods: {
		shortJwt() {
			this.$request(this.$AdminApi.CypherSecretsAdmin.ResetCypher, {
				name: "ShortJwt"
			}).then(_ => this.$successNotify());
		},
		long2Jwt() {
			this.$request(this.$AdminApi.CypherSecretsAdmin.ResetCypher, {
				name: "Long2Jwt"
			}).then(_ => this.$successNotify());
		},
		emailChange() {
			this.$request(this.$AdminApi.CypherSecretsAdmin.ResetCypher, {
				name: "EmailChange"
			}).then(_ => this.$successNotify());
		}
	},
	created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss"></style>
