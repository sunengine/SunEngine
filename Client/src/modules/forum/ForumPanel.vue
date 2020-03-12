<template>
	<nav>
		<PanelWrapper class="forum-panel" :titleProp="$tl('sections')">
			<q-item class="q-my-xs" exact dense :to="newTopicsRoute">
				<q-item-section>
					<q-item-label>
						{{ $tl("newTopics") }}
					</q-item-label>
				</q-item-section>
			</q-item>
			<component :is="categories" :categoryName="categoryName" />
		</PanelWrapper>
	</nav>
</template>

<script>
export default {
	name: "ForumPanel",
	components: {
		PanelWrapper: sunImport("components","PanelWrapper"),
	},
	props: {
		categories: {
			type: [Object, Function],
			required: true
		},
		categoryName: {
			type: String,
			required: true
		}
	},
	computed: {
		category() {
			return this.$store.getters.getCategory(this.categoryName);
		},
		newTopicsRoute() {
			return this.category?.getRoute();
		}
	}
};
</script>

<style lang="scss"></style>
