<template>
	<SunPage class="news-2-col-page">
		<PageHeader class="page-padding" :title="title" :subTitle="subTitle" />

		<div :class="['row', { hidden: !loaded }]">
			<div
				:class="[
					'col-xs-12',
					'col-md-6',
					'col1',
					'pull-left',
					$q.screen.gt.sm ? 'hr-minus' : 'pull-right'
				]"
			>
				<PostsMultiCat ref="postsList" component-name="Posts" />
			</div>
			<div
				:class="[
					'col-xs-12',
					'col-md-6',
					'col2',
					'pull-right',
					{ 'pull-left': !$q.screen.gt.sm }
				]"
			>
				<activities-list ref="activitiesList" componentName="Activities" />
			</div>
		</div>
		<LoaderWait v-if="!loaded" />
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "News2ColPage",
	mixins: [Page],
	data: function() {
		return {
			mounted: false
		};
	},
	watch: {
		"$route.query.page": "loadData"
	},
	computed: {
		subTitle() {
			return this.$tle("subTitle") ? this.$tl("subTitle") : null;
		},
		loaded() {
			if (!this.mounted) return;
			return (
				this.$refs?.postsList?.posts && this.$refs?.activitiesList?.activities
			);
		}
	},
	mounted() {
		this.mounted = true;
	},
	beforeCreate() {
		this.$options.components.ActivitiesList = require("sun").ActivitiesList;
		this.$options.components.PostsList = require("sun").PostsList;
		this.$options.components.LoaderWait = require("sun").LoaderWait;
		this.$options.components.PostsMultiCat = require("sun").PostsMultiCat;
	},
	created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss">
.news-2-col-page {
	.hr-minus {
		.posts-list {
			.hr-sep {
				margin-right: 9px !important;
			}
		}
	}
}
</style>
