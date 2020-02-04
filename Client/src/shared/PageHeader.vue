<template>
	<div class="page-header">
		<header>
			<div class="page-title-block">
				<h1 class="page-title grow">
					<slot name="title">
						<q-icon v-if="icon" :name="icon" />
						{{ showTitle }}
					</slot>
				</h1>
				<slot> </slot>
			</div>
			<div v-if="showSubTitle" class="page-sub-title">
				{{ showSubTitle }}
			</div>
			<div
				v-if="showHeader"
				v-html="showHeader"
				class="page-category-header"
			></div>
		</header>
	</div>
</template>

<script>
export default {
	name: "PageHeader",
	props: {
		category: {
			type: Object,
			required: false,
			default: () => {}
		},
		title: {
			type: String,
			required: false
		},
		subTitle: {
			type: String,
			required: false
		},
		header: {
			type: String,
			required: false
		},
		icon: {
			type: String,
			required: false
		}
	},
	computed: {
		showTitle() {
			return this.title ?? this.category?.title;
		},
		showSubTitle() {
			return this.subTitle ?? this.category?.subTitle;
		},
		showHeader() {
			return this.header ?? this.category?.header;
		}
	}
};
</script>

<style lang="scss">
.page-header {
	margin: 14px 0 34px 0;
}

.page-title {
	padding: 0;
	font-size: 36px;
	line-height: 40px;
	font-weight: 300;
	margin: 12px 0 0 0;
}

.page-title-block {
	display: flex;
	flex-wrap: wrap;
	align-items: center;
	padding-top: 0;
	padding-bottom: 0;

	.page-title {
		flex-grow: 1;
		padding: 0 !important;
		margin: 3px 0;
	}

	.q-btn {
		flex-grow: 0;
		margin: 3px 0;
	}
}

.page-sub-title {
	margin-top: 12px;
	margin-bottom: 22px;
	color: $grey-7;
	font-style: italic;
	border-left: 4px solid silver;
	padding: 4px 0 4px 20px;
}

.category-header {
	margin-top: -14px;
}
</style>
