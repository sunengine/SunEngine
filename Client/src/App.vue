<template>
	<div id="q-app" class="app">
		<Layout :key="rerenderKey" v-if="isInitialized" />

		<div v-else-if="!initializeError" class="app__loader">
			<div>
				<q-spinner-gears size="40px" class="q-mr-sm" />
				{{ $tl("loading") }}
			</div>
		</div>

		<div v-else-if="initializeError" class="app__api-error">
			<p>
				<img class="app__img-sad" src="/statics/sad.svg" />
			</p>
			<p class="app__error-info">
				{{ $tl("canNotConnectApi") }}
			</p>
			<a class="app__refresh-btn" href="#" @click="refresh">
				<q-icon name="fa fa-sync-alt" class="q-mr-xs" />
				{{ $t("Global.refresh") }}</a
			>
		</div>
	</div>
</template>

<script>
import { mapGetters } from "vuex";
import Vue from "vue";
import { Layout } from "sun";

var app;

export default {
	name: "App",
	components: { Layout },
	data() {
		return {
			rerenderKey: 1
		};
	},
	computed: {
		...mapGetters(["isInitialized", "initializeError"])
	},
	watch: {
		isInitialized() {
			if (this.isInitialized && config.Dev.VueAppInWindow) {
				window.app = this;
				window.pulseException = () => this.$request(this.$Api.Pulse.PulseException);
			}
		}
	},
	methods: {
		rerender() {
			this.rerenderKey += 1;
		},
		refresh() {
			this.$store.state.initializedPromise = this.$store.dispatch("initStore");
			this.$store.state.initializedPromise.then(_ =>
				this.$router.push(this.$router.currentRoute)
			);
		}
	},
	beforeCreate() {
		app = this;
	}
};

export { app };
</script>

<style lang="scss">
.app__api-error {
	display: flex;
	height: 100vh;
	align-items: center;
	align-content: center;
	justify-content: center;
	flex-direction: column;
	font-size: 1.2rem;
	color: grey;
	font-weight: 400;
}

.app__loader {
	display: flex;
	height: 100vh;
	align-items: center;
	align-content: center;
	justify-content: center;
	font-size: 1.4em;
	color: #005d00;
}

.app__img-sad {
	width: 30vw;
	max-width: 150px;
}

.app__refresh-btn {
	font-size: 1.1rem;
	color: grey;
	padding: 10px 25px;
	vertical-align: middle;
	border-radius: 4px;

	&:hover {
		background-color: $grey-4;
		transition: 0.2s;
	}
}
</style>
