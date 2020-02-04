<template>
	<SunPage class="error404 flex column flex-center">
		<p>
			<img class="error404__sad-img" src="/statics/sad.svg" />
		</p>
		<p class="error404__info text-faded">
			{{ $tl("info") }}
			<strong>(404)</strong>
		</p>
		<q-btn color="secondary" class="error404__back-btn" @click="$router.back()">
			{{ $tl("goBackBtn") }}
		</q-btn>
	</SunPage>
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

<style lang="scss">
.error404__back-btn {
	width: 200px;
}
.error404__sad-img {
	width: 30vw;
	max-width: 150px;
}
</style>
