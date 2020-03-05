<template>
	<SunPage class="posts-and-activities-page">
		<PageHeader
			v-if="pageTitle"
			class="page-padding"
			:title="pageTitle"
			:subTitle="pageSubTitle"
		/>

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
				<PostsMultiCat ref="postsList" :section-name="postsSectionName" />
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
				<activities-list
					ref="activitiesList"
					:sectionName="activitiesSectionName"
				/>
			</div>
		</div>
		<LoaderWait v-if="!loaded" />
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "PostsAndActivitiesPage",
	mixins: [Page],
	props: {
		pageTitle: {
			type: String,
			required: false
		},
		pageSubTitle: {
			type: String,
			required: false
		},
		postsSectionName: {
			type: String,
			required: false,
			default: "Posts"
		},
		activitiesSectionName: {
			type: String,
			required: false,
			default: "Activities"
		}
	},
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
		this.$options.components.ActivitiesList = require("sun").ActivitiesList;;
		this.$options.components.PostsList = require("sun").PostsList;;
		this.$options.components.PostsMultiCat = require("sun").PostsMultiCat;;
	},
	created() {
		this.title = this.pageTitle;
	}
};
</script>

<style lang="scss">
.posts-and-activities-page {
	.hr-minus {
		.posts-list {
			.hr-sep {
				margin-right: 9px !important;
			}
		}
	}
}
</style>
