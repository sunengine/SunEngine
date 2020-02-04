<template>
	<q-breadcrumbs class="breadcrumbs text-grey" active-color="purple">
		<q-breadcrumbs-el
			:exact="true"
			key="mr"
			:label="$tl('home')"
			:to="{ name: 'Home' }"
		/>
		<template v-if="category">
			<q-breadcrumbs-el
				:key="cat.id"
				:exact="true"
				:class="{ 'q-breadcrumbs--grey': !cat.route }"
				v-for="cat of breadCrumbsCategories"
				:to="cat.route"
				:label="cat.title"
			/>
		</template>
		<q-breadcrumbs-el
			:exact="true"
			v-if="pageTitle && pageTitle !== ' '"
			key="ml"
			:label="pageTitle"
		/>
	</q-breadcrumbs>
</template>

<script>
export default {
	name: "Breadcrumbs",
	props: {
		category: {
			type: Object,
			required: false
		},
		pageTitle: {
			type: String,
			required: false
		}
	},
	computed: {
		breadCrumbsCategories() {
			if (!this.category) return null;

			let rez = [];
			let current = this.category;
			while (current && current.name !== "Root") {
				if (current.showInBreadcrumbs) rez.push(current);
				current = current.parent;
			}

			rez = rez.reverse();

			if (rez.length >= 1 && rez[rez.length - 1].title === this.pageTitle)
				rez.splice(rez.length - 1, 1);

			return rez;
		}
	}
};
</script>

<style lang="scss">
.q-breadcrumbs__el {
	color: #00acc1;
}

.q-breadcrumbs {
	.q-breadcrumbs__el.q-breadcrumbs--grey,
	.q-breadcrumbs--last {
		color: $grey-6;
	}
}

.breadcrumbs {
	div {
		display: inline;
	}
}
</style>
