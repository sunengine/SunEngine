<template>
	<div class="activities-list">
		<template v-if="activities">
			<Activity
				:key="activity.materialId + '-' + activity.commentId"
				:activity="activity"
				v-for="activity in activities"
			/>
		</template>
		<LoaderWait ref="loader" v-else />
	</div>
</template>

<script>
export default {
	name: "ActivitiesList",
	props: {
		componentName: {
			type: String,
			required: true
		}
	},
	data() {
		return {
			activities: null
		};
	},
	computed: {
		component() {
			return this.$store.getters.getComponent(this.componentName);
		}
	},
	methods: {
		loadData() {
			this.$request(this.$Api.Activities.GetActivities, {
				componentName: this.componentName
			})
				.then(response => {
					this.activities = response.data;
				})
				.catch(x => {
					this.$refs.loader.fail();
				});
		}
	},
	beforeCreate() {
		this.$options.components.Activity = require("sun").Activity;
	},
	async created() {
		await this.loadData();
	}
};
</script>

<style lang="scss"></style>
