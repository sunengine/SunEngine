<template>
	<SunPage class="change-email-result flex middle page-padding">
		<q-banner
			v-if="success"
			class="change-email-result__success-banner bg-positive text-white"
		>
			<template v-slot:avatar>
				<q-icon :name="$iconsSet.ChangeEmailResult.success" size="2em" />
			</template>
			{{ $tl("success") }}
		</q-banner>
		<q-banner
			v-else
			class="change-email-result__error-banner bg-negative text-white"
		>
			<template v-slot:avatar>
				<q-icon :name="$iconsSet.ChangeEmailResult.failed" size="2em" />
			</template>
			{{ $tl("error") }}
		</q-banner>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "ChangeEmailResult",
	mixins: [Page],
	computed: {
		success() {
			return this.$route.query["result"] === "ok";
		}
	},
	created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss"></style>
