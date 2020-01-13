<template>
	<q-page class="error404 flex column flex-center">
		<p>
			<img src="/statics/sad.svg" style="width:30vw;max-width:150px;" />
		</p>
		<p class="text-faded">
			{{ $tl("info") }}
			<strong>(404)</strong>
		</p>
		<q-btn color="secondary" style="width:200px;" @click="$router.back()">
			{{ $tl("goBackBtn") }}
		</q-btn>
	</q-page>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "Error404",
	mixins: [Page],
	created() {
		this.title = this.$tl("title");
	}
};
</script>
