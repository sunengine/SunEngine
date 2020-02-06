<template>
	<SunPage class="index-page">
		<MaterialInline name="index-page" />
	</SunPage>
</template>

<script>
import { Page } from "mixins";
import { MaterialInline } from "sun";

export default {
	name: "IndexPage",
	mixins: [Page],
	components: { MaterialInline }
};
</script>

<style lang="scss">

</style>
