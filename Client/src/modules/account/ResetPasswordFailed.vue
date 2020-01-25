<template>
	<SunPage class="reset-password-failed flex middle page-padding">
		<q-banner rounded class="bg-negative text-white">
			<template v-slot:avatar>
				<q-icon :name="$iconsSet.ResetPasswordFailed.failed" size="2em" />
			</template>
			{{ $tl("message") }}
		</q-banner>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "ResetPasswordFailed",
	mixins: [Page],
	created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss"></style>
