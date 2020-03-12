<template>
	<SunPage class="index-page">
		<MaterialInline name="index-page" />
	</SunPage>
</template>

<script>
export default {
	name: "IndexPage",
	components: {
		MaterialInline: sunImport("material","MaterialInline"),
	},
	mixins: [Page]
};
</script>

<style lang="scss"></style>
