<template>
	<SunPage class="reset-password-set-new flex middle page-padding">
		<div class="center-form" v-if="!done">
			<q-input
				class="reset-password-set-new__password"
				ref="password"
				v-model="password"
				:type="showPassword ? 'text' : 'password'"
				:label="$tl('password')"
				:rules="rules.password"
			>
				<template v-slot:prepend>
					<q-icon :name="$iconsSet.ResetPasswordSetNew.key" />
				</template>
				<template v-slot:append>
					<q-icon
						:name="
							showPassword
								? $iconsSet.ResetPasswordSetNew.eye
								: $iconsSet.ResetPasswordSetNew.eyeSlash
						"
						class="cursor-pointer"
						@click="showPassword = !showPassword"
					/>
				</template>
			</q-input>

			<q-input
				class="reset-password-set-new__password2"
				ref="password2"
				v-model="password2"
				:type="showPassword2 ? 'text' : 'password'"
				:label="$tl('password2')"
				:rules="rules.password2"
			>
				<template v-slot:prepend>
					<q-icon :name="$iconsSet.ResetPasswordSetNew.key" />
				</template>
				<template v-slot:append>
					<q-icon
						:name="
							showPassword2
								? $iconsSet.ResetPasswordSetNew.eye
								: $iconsSet.ResetPasswordSetNew.eyeSlash
						"
						class="cursor-pointer"
						@click="showPassword2 = !showPassword2"
					/>
				</template>
			</q-input>

			<q-btn
				class="q-mt-md send-btn full-width"
				:label="$tl('saveBtn')"
				@click="changePassword"
				:loading="submitting"
			>
				<LoaderSent slot="loading" />
			</q-btn>
		</div>

		<q-banner
			v-else
			class="reset-password-set-new__banner-success bg-positive text-white"
		>
			<template v-slot:avatar>
				<q-icon :name="$iconsSet.ResetPasswordSetNew.key" size="2em" />
			</template>
			{{ $tl("successMessage") }}
			<router-link :to="{ name: 'Login' }">{{ $tl("enter") }}</router-link>
			.
		</q-banner>
	</SunPage>
</template>

<script>
import { Page } from "mixins";
import { passwordRules } from "utils";

export default {
	name: "ResetPasswordSetNew",
	mixins: [Page],
	data() {
		return {
			password: "",
			password2: "",
			submitting: false,
			done: false,
			showPassword: false,
			showPassword2: false
		};
	},
	computed: {
		rules() {
			const passwordRulesInst = passwordRules.call(this);
			return {
				password: passwordRulesInst,
				password2: [
					...passwordRulesInst,
					value =>
						this.password === this.password2 ||
						this.$t("Global.validation.password.passwordsNotEquals")
				]
			};
		}
	},
	methods: {
		changePassword() {
			this.$refs.password.validate();
			this.$refs.password2.validate();

			if (this.$refs.password.hasError || this.$refs.password2.hasError) return;

			this.submitting = true;

			this.$request(this.$Api.Account.ResetPasswordSetNew, {
				uid: this.$route.query.uid,
				token: this.$route.query.token,
				newPassword: this.password
			})
				.then(() => {
					this.done = true;
				})
				.catch(error => {
					this.$errorNotify(error.response.data);
					this.submitting = false;
				});
		}
	},
	created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss"></style>
