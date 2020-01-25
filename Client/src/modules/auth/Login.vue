<template>
	<SunPage class="login flex flex-center page-padding">
		<div class="center-form">
			<q-form @submit="login">
				<q-input
					ref="nameOrEmail"
					v-model="nameOrEmail"
					:label="$tl('nameOrEmail')"
					:rules="[value => !!value || $tl('validation.nameOrEmail.required')]"
				>
					<template v-slot:prepend>
						<q-icon :name="$iconsSet.Login.user" />
					</template>
				</q-input>

				<q-input
					ref="password"
					v-model="password"
					:type="showPassword ? 'text' : 'password'"
					@keyup.enter="login"
					:label="$tl('password')"
					:rules="[value => !!value || $tl('validation.password.required')]"
				>
					<template v-slot:prepend>
						<q-icon :name="$iconsSet.Login.key" />
					</template>
					<template v-slot:append>
						<q-icon
							:name="showPassword ? $iconsSet.Login.eye : $iconsSet.Login.eyeSlash"
							class="cursor-pointer"
							@click="showPassword = !showPassword"
						/>
					</template>
				</q-input>

				<q-btn
					type="submit"
					class="send-btn full-width"
					:label="$tl('enterBtn')"
					:loading="submitting"
				>
					<LoaderSent slot="loading">
						{{ $tl("entering") }}
					</LoaderSent>
				</q-btn>
				<div class="text-center q-mt-lg">
					<router-link :to="{ name: 'ResetPassword' }">
						<q-icon class="q-mr-sm" :name="$iconsSet.Login.question" />
						<span>{{ $tl("forgotPassword") }}</span>
					</router-link>
				</div>
			</q-form>
		</div>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "Login",
	mixins: [Page],
	props: {
		ret: {
			type: String,
			required: false
		}
	},
	data() {
		return {
			nameOrEmail: null,
			password: null,
			submitting: false,
			showPassword: false
		};
	},
	methods: {
		login() {
			this.$refs.nameOrEmail.validate();
			this.$refs.password.validate();

			if (this.$refs.nameOrEmail.hasError || this.$refs.password.hasError) return;

			this.submitting = true;

			this.$store
				.dispatch("login", {
					nameOrEmail: this.nameOrEmail,
					password: this.password
				})
				.catch(error => {
					this.submitting = false;
					this.$errorNotify(error);
				});
		}
	},
	created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss"></style>
