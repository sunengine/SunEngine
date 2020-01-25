<template>
	<SunPage class="thread">
		<PageHeader class="page-padding" :title="showTitle" :category="category">
			<q-btn
				no-caps
				class="thread__post-btn post-btn"
				@click="
					$router.push({
						name: 'CreateMaterial',
						params: {
							categoriesNames: category.sectionRoot.name,
							initialCategoryName: category.name
						}
					})
				"
				:label="$tl('newTopicBtn')"
				v-if="canAddTopic"
				:icon="$iconsSet.Thread.add"
			/>
		</PageHeader>

		<div class="q-mt-sm" v-if="topics.items">
			<div class="thread__table-header margin-back bg-grey-2 gt-xs text-grey-6 ">
				<hr class="thread__sep" />

				<div class="row">
					<div class="col-xs-12 col-sm-8" style="padding: 2px 0px 2px 76px; ">
						{{ $tl("topic") }}
					</div>
					<div class="col-xs-12 col-sm-2" style="padding: 2px 0px 2px 60px;">
						{{ $tl("last") }}
					</div>
				</div>
			</div>

			<q-list class="thread__list" no-border>
				<hr class="thread__sep margin-back" />

				<div class="margin-back" v-for="topic in topics.items" :key="topic.id">
					<Topic :topic="topic" />
					<hr class="thread__sep" />
				</div>
			</q-list>

			<q-pagination
				v-if="topics.totalPages > 1"
				v-model="topics.pageIndex"
				color="pagination"
				:max-pages="12"
				:max="topics.totalPages"
				ellipses
				direction-links
				@input="pageChanges"
			/>
		</div>
		<LoaderWait ref="loader" v-else />
	</SunPage>
</template>

<script>
import { Page } from "mixins";
import { Pagination } from "mixins";

export default {
	name: "Thread",
	mixins: [Page, Pagination],
	props: {
		categoryName: {
			type: String,
			required: true
		},
		loadTopics: {
			type: Function,
			required: true
		},
		pageTitle: {
			type: String,
			required: false
		}
	},
	data() {
		return {
			topics: {}
		};
	},
	watch: {
		$route: "loadData"
	},
	computed: {
		showTitle() {
			return this.pageTitle ?? this.category.title;
		},
		canAddTopic() {
			return this.category?.categoryPersonalAccess?.MaterialWrite; // || this.category?.categoryPersonalAccess?.MaterialWriteWithModeration;
		},
		category() {
			return (this.$store.state.currentCategory = this.$store.getters.getCategory(
				this.categoryName
			));
		}
	},
	beforeCreate() {
		this.$options.centered = true;
		this.$options.components.Topic = require("sun").Topic;
	},
	methods: {
		loadData() {
			this.loadTopics.call(this);
			this.title = this.showTitle;
		}
	},
	created() {
		this.loadData();
	}
};
</script>

<style lang="scss">
.thread__sep {
	border: 0;
	height: 1px;
	margin-top: 0;
	margin-bottom: 0;
	//margin-left: -50px;
	//margin-right: -150px;
	background-color: #d3eecc;
	//border: solid #d3eecc 1px;
	//border-left: none;
	//max-width: 200% !important;
	//position: relative;
	//left: -50px;
	//right: -150px;
}

.thread__list {
	padding: 0;
	margin-bottom: 12px;
}
</style>
