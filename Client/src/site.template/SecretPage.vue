<template>
	<SunPage class="secret-page middle flex">
		<h1 class="secret-page__header">
			<q-icon name="fas fa-user-secret" left />
			{{ title }}
		</h1>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "SecretPage",
	mixins: [Page],
	created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss">
.secret-page__header {
	border-radius: 12px;
	border: 4px dashed $amber-3;
	padding: 14px 34px;
	color: $light-blue-6;
	font-size: 60px;
	margin: 20px;
	text-align: center;
}
</style>
