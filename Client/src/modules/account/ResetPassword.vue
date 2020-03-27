<template>
	<SunPage class="reset-password flex middle page-padding">
		<div class="center-form q-gutter-y-sm" v-if="!done">
			<q-input
				class="reset-password__email"
				ref="email"
				v-model="email"
				type="email"
				:label="$tl('email')"
				:rules="rules"
			>
				<template v-slot:prepend>
					<q-icon :name="$iconsSet.ResetPassword.envelope" />
				</template>
			</q-input>

			<Captcha ref="captcha" v-model="captchaText" />

			<q-btn
				class="send-btn full-width"
				:label="$tl('resetPasswordBtn')"
				@click="send"
				:loading="submitting"
			>
				<LoaderSent slot="loading" />
			</q-btn>
		</div>

		<q-banner
			v-else
			class="reset-password__banner-success bg-positive text-white"
		>
			<template v-slot:avatar>
				<q-icon :name="$iconsSet.ResetPassword.envelope" size="2em" />
			</template>
			{{ $tl("success") }}
		</q-banner>
	</SunPage>
</template>

<script>
import { emailRules } from "utils";

export default {
	name: "ResetPassword",
	mixins: [Page],
	data() {
		return {
			email: "",
			submitting: false,
			start: true,
			done: false,
			captchaText: ""
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Personal");
		},
		rules() {
			return emailRules.call(this);
		}
	},
	methods: {
		send() {
			this.$refs.email.validate();
			this.$refs.captcha.validate();

			if (this.$refs.email.hasError || this.$refs.captcha.hasError) return;

			this.submitting = true;
			this.$request(this.$Api.Account.ResetPasswordSendEmail, {
				Email: this.email,
				CaptchaToken: this.token,
				CaptchaText: this.captchaText
			})
				.then(() => {
					this.done = true;
					this.submitting = false;
				})
				.catch(error => {
					this.$errorNotify(error);
					this.submitting = false;

					if (error?.response?.data?.code === "CaptchaValidationError") {
						this.$refs.captcha.GetToken();
					}
				});
		}
	},
	beforeCreate() {
		this.$config.components.Captcha = require("comp").Captcha;
	},
	created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss"></style>
