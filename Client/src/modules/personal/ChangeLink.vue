<template>
	<SunPage class="change-link flex column page-padding">
		<div class="flex flex-center">
			<PageHeader :title="title" />
		</div>

		<div class="flex flex-center grow">
			<div class="center-form">
				<div class="text-grey-7 q-mb-lg text-justify">
					{{ $tl("linkValidationInfo") }}
				</div>

				<q-input
					clearable
					@clear="clear"
					ref="link"
					v-model="link"
					:label="$tl('link')"
					:rules="rules"
				>
					<template v-slot:prepend>
						<q-icon :name="$iconsSet.ChangeLink.link" />
					</template>
					<template v-slot:hint>
						{{ $tl("hintLink") }} &nbsp; {{ userLink }}
					</template>
				</q-input>

				<q-btn
					no-caps
					class="q-mt-lg send-btn block full-width"
					:icon="$iconsSet.ChangeLink.save"
					:label="$tl('saveBtn')"
					@click="save"
					:loading="submitting"
				>
					<LoaderSent slot="loading" />
				</q-btn>
			</div>
		</div>
	</SunPage>
</template>

<script>
import { Page } from "mixins";
import { store } from "sun";

function allowMyIdOrEmpty(id) {
	return !id || store.state.auth.user.id == id;
}

function createRules() {
	return [
		value =>
			value.length >= 3 ||
			allowMyIdOrEmpty.call(this, value) ||
			this.$tl("validation.minLength"), // minLength or myId
		value => /^[a-z0-9-]*$/.test(value) || this.$tl("validation.allowedChars"), // allowed chars
		value =>
			/[a-zA-Z]/.test(value) ||
			allowMyIdOrEmpty.call(this, value) ||
			this.$tl("validation.numberNotAllow"), // need char or myId
		value => !this.linkInDb || this.$tl("validation.linkInDb") // link in db
	];
}

export default {
	name: "ChangeLink",
	mixins: [Page],
	data() {
		return {
			link: this.$store.state.auth.user.link,
			linkInDb: false,
			submitting: false
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Personal");
		},
		userLink() {
			const route = this.$router.resolve({
				name: "User",
				params: { link: this.link }
			});
			return config.Global.SiteUrl + route?.resolved?.fullPath;
		}
	},
	methods: {
		clear() {
			this.link = this.$store.state.auth.user.id;
		},
		checkLinkInDb() {
			this.$request(this.$Api.Personal.CheckLinkInDb, {
				link: this.link
			}).then(response => {
				this.linkInDb = response.data.yes;
				this.$refs.link.validate();
			});
		},
		save() {
			this.$refs.link.validate();

			if (this.$refs.link.hasError) return;

			this.submitting = true;

			this.$request(this.$Api.Personal.SetMyLink, {
				link: this.link
			})
				.then(response => {
					this.$store.commit("setUserInfo", response.data);
					this.$successNotify();
					this.$router.push({ name: "Personal" });
				})
				.catch(error => {
					this.$errorNotify(error);
					this.submitting = false;
				});
		}
	},
	beforeDestroy() {
		clearTimeout(this.timeout);
	},
	created() {
		this.title = this.$tl("title");

		this.rules = createRules.call(this);

		this.checkLinkInDb = this.$throttle(this.checkLinkInDb);

		this.$watch("link", this.checkLinkInDb);
	}
};
</script>

<style lang="scss"></style>
