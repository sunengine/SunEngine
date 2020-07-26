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
    import Vue from "vue";
    import { mapGetters } from "vuex";

var app;

export default {
	name: "App",
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

			// Will run on boot
            // this.startUpCustomJsScripts();
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
		},
        startUpCustomJsScripts() {
                if (!config.Admin.StartUpScripts || !config.Admin.AllowCustomJavaScript)
                    return;

                const scriptsToStart = config.Admin.StartUpScripts.split(",");

                for (const script of scriptsToStart) {
                    try {
                        window[script](this, Vue);
                    } catch (e) {
                        console.error(e);
                    }
                }
        }
	},
	beforeCreate() {
		app = this;
		this.$options.components.Layout = sunImport("layout","Layout");
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
