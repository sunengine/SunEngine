<template>
	<SunPage class="index-page middle flex flex-center">
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
.index-page {
	display: flex;
	flex-direction: column;
	align-items: center;
	padding-bottom: 0 !important;
}

.index-page__demo-header {
	margin: 0 0 40px;
	font-size: 100px;
	font-weight: bold;
	letter-spacing: 0.08em;
	text-align: center;
	color: #00acef;
	text-shadow: 1px 1px 5px #b1b1b1;
}

.index-page__info-block {
	font-weight: 500;
	font-size: 1.2em;
	max-width: 600px;

	p {
		text-align: center;
		color: #616161;
	}
}

.index-page__info-block-accent {
	font-weight: 600;
	color: #a38167;
}

.index-page__version {
	.num {
		font-weight: 600;
		color: #330f53;
	}
}
</style>
