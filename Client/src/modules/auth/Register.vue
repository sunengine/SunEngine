<template>
	<SunPage class="register flex flex-center page-padding">
		<div v-if="!done" class="center-form">
			<q-input
				ref="userName"
				v-model="userName"
				:label="$tl('userName')"
				@keyup="checkUserNameInDb"
				:rules="rules.userName"
			>
				<template v-slot:prepend>
					<q-icon :name="$iconsSet.Register.user" />
				</template>
			</q-input>

			<q-input
				ref="email"
				v-model="email"
				type="email"
				:label="$tl('email')"
				:rules="rules.email"
			>
				<template v-slot:prepend>
					<q-icon :name="$iconsSet.Register.envelope" />
				</template>
			</q-input>

			<q-input
				ref="password"
				v-model="password"
				:type="showPassword ? 'text' : 'password'"
				:label="$tl('password')"
				:rules="rules.password"
			>
				<template v-slot:prepend>
					<q-icon :name="$iconsSet.Register.key" />
				</template>
				<template v-slot:append>
					<q-icon
						:name="
							showPassword ? $iconsSet.Register.eye : $iconsSet.Register.eyeSlash
						"
						class="cursor-pointer"
						@click="showPassword = !showPassword"
					/>
				</template>
			</q-input>

			<q-input
				ref="password2"
				v-model="password2"
				:type="showPassword2 ? 'text' : 'password'"
				:label="$tl('password2')"
				:rules="rules.password2"
			>
				<template v-slot:prepend>
					<q-icon :name="$iconsSet.Register.key" />
				</template>
				<template v-slot:append>
					<q-icon
						:name="
							showPassword2 ? $iconsSet.Register.eye : $iconsSet.Register.eyeSlash
						"
						class="cursor-pointer"
						@click="showPassword2 = !showPassword2"
					/>
				</template>
			</q-input>

			<Captcha ref="captcha" v-model="captchaText" />

			<q-btn
				style="width:100%;"
				class="send-btn"
				:label="$tl('registerBtn')"
				@click="register"
				:loading="submitting"
			>
				<LoaderSent slot="loading">
					{{ $tl("registering") }}
				</LoaderSent>
			</q-btn>
		</div>
		<q-banner v-else class="bg-positive">
			<template v-slot:avatar>
				<q-icon :name="$iconsSet.Register.envelope" size="2em" />
			</template>
			{{ $tl("emailSent") }}
		</q-banner>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

function createRules() {
	const password = [
		value => !!value || this.$tl("validation.password.required"),
		value =>
			value.length >= config.PasswordValidation.MinLength ||
			this.$tl("validation.password.minLength"),
		value =>
			[...new Set(value.split(""))].length >=
				config.PasswordValidation.MinDifferentChars ||
			this.$tl("validation.password.minDifferentChars")
	];

	return {
		userName: [
			value => !!value || this.$tl("validation.userName.required"),
			value => value.length >= 3 || this.$tl("validation.userName.minLength"),
			value =>
				value.length <= config.DbColumnSizes.Users_UserName ||
				this.$tl("validation.userName.maxLength"),
			value => !this.userNameInDb || this.$tl("validation.userName.nameInDb") // link in db
		],
		email: [
			value => !!value || this.$tl("validation.email.required"),
			value => /.+@.+/.test(value) || this.$tl("validation.email.emailSig"),
			value =>
				value.length <= config.DbColumnSizes.Users_Email ||
				this.$tl("validation.email.maxLength")
		],
		password: password,
		password2: [
			...password,
			value =>
				this.password === this.password2 || this.$tl("validation.password2.equals")
		]
	};
}

export default {
	name: "Register",
	mixins: [Page],
	data() {
		return {
			userName: "",
			email: "",
			password: "",
			password2: "",
			submitting: false,
			done: false,
			showPassword: false,
			showPassword2: false,
			userNameInDb: false,
			captchaText: ""
		};
	},
	methods: {
		checkUserNameInDb() {
			clearTimeout(this.timeout);
			this.timeout = setTimeout(this.checkUserNameInDbDo, 500);
		},
		checkUserNameInDbDo() {
			this.$request(this.$Api.Auth.CheckUserNameInDb, {
				userName: this.userName
			}).then(response => {
				this.userNameInDb = response.data.yes;
				this.$refs.userName.validate();
			});
		},
		register() {
			this.$refs.userName.validate();
			this.$refs.email.validate();
			this.$refs.password.validate();
			this.$refs.password2.validate();
			this.$refs.captcha.validate();
			 
			if (
				this.$refs.userName.hasError ||
				this.$refs.email.hasError ||
				this.$refs.password.hasError ||
				this.$refs.password2.hasError ||
				this.$refs.captcha.hasError
			)
				return;

			this.submitting = true;

			this.$request(this.$Api.Auth.Register, {
				UserName: this.userName,
				Email: this.email,
				Password: this.password,
				CaptchaToken: this.token,
				CaptchaText: this.captchaText
			})
				.then(() => {
					this.done = true;
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
		this.$options.components.Captcha = require("sun").Captcha;
	},
	created() {
		this.title = this.$tl("title");

		this.rules = createRules.call(this);
	}
};
</script>

<style lang="scss"></style>
