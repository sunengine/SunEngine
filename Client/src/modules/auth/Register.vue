<template>
	<SunPage class="register flex flex-center page-padding">
		<div v-if="!done" class="center-form q-gutter-y-xs">
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

			<div v-if="registerConfirmText" class="q-mb-lg q-mt-md flex align-center">
				<q-toggle v-model="acceptConfirm"> </q-toggle>
				<div id="register__confirm-text" v-html="registerConfirmText"></div>
			</div>

			<Captcha ref="captcha" v-model="captchaText" />

			<q-btn
				:disable="registerConfirmText && !acceptConfirm"
				style="width:100%;"
				:class="{
					'send-btn': true,
					'bg-grey': registerConfirmText && !acceptConfirm
				}"
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
import { passwordRules } from "utils";
import { userNameRules } from "utils";
import { emailRules } from "utils";

function createRules() {
	return {
		userName: userNameRules,
		email: emailRules,
		password: passwordRules,
		password2: [
			...passwordRules,
			value =>
				this.password === this.password2 ||
				this.$t("Global.validation.password.passwordsNotEquals")
		]
	};
}

export default {
	name: "Register",
	components: {
		Captcha: sunImport("components","Captcha"),
	},
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
			captchaText: "",
			acceptConfirm: false
		};
	},
	computed: {
		registerConfirmText() {
			return config.Register.ConfirmText;
		}
	},
	methods: {
		addTargetBlankOnLinks() {
			if (!this.registerConfirmText) return;

			const el = document.getElementById("register__confirm-text");
			const links = el.getElementsByTagName("a");

			for (const link of links) {
				link.classList.add("link");
				link.setAttribute("target", "_blank");
				/*	link.addEventListener("click", e => {
					e.preventDefault();
					e.stopPropagation();
					window.open(link.href);
					return true;
				});*/
			}
		},
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
	mounted() {
		this.addTargetBlankOnLinks();
	},
	created() {
		this.title = this.$tl("title");

		this.rules = createRules.call(this);
	}
};
</script>

<style lang="scss"></style>
