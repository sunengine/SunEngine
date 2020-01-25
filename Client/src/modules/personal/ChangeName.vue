<template>
	<SunPage class="change-name flex column page-padding">
		<h1 class="page-title text-center">
			{{ title }}
		</h1>

		<div class="flex flex-center grow">
			<div class="center-form">
				<div class="change-name__info text-grey-7 q-mb-lg">
					{{ $tl("nameValidationInfo") }}
				</div>

				<q-input
					class="change-name__password"
					ref="password"
					v-model="password"
					:type="showPassword ? 'text' : 'password'"
					:label="$tl('password')"
					:rules="rules.passwordRules"
				>
					<template v-slot:prepend>
						<q-icon :name="$iconsSet.ChangeName.key" />
					</template>
					<template v-slot:append>
						<q-icon
							:name="
								showPassword ? $iconsSet.ChangeName.eye : $iconsSet.ChangeName.eyeSlash
							"
							class="cursor-pointer"
							@click="showPassword = !showPassword"
						/>
					</template>
				</q-input>

				<q-input
					class="change-name__name"
					ref="name"
					color="positive"
					v-model="name"
					:label="$tl('name')"
					:rules="rules.nameRules"
				>
					<template v-slot:prepend>
						<q-icon :name="$iconsSet.ChangeName.user" />
					</template>
				</q-input>

				<q-btn
					no-caps
					class="send-btn q-mt-lg full-width"
					:icon="$iconsSet.ChangeName.save"
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

function createRules() {
	return {
		passwordRules: [value => !!value || this.$tl("validation.password.required")],
		nameRules: [
			value => !!value || this.$tl("validation.name.required"),
			value => value.length >= 3 || this.$tl("validation.name.minLength"),
			value =>
				/^[ a-zA-Zа-яА-ЯёЁ0-9-]*$/.test(value) ||
				this.$tl("validation.name.allowedChars"),
			value => !this.nameInDb || this.$tl("validation.name.nameInDb")
		]
	};
}

export default {
	name: "ChangeName",
	mixins: [Page],
	data() {
		return {
			name: this.$store.state.auth.user.name,
			password: null,
			showPassword: false,
			nameInDb: false,
			submitting: false
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Personal");
		}
	},
	methods: {
		checkNameInDb() {
			if (
				this.name.toLowerCase() === this.$store.state.auth.user.name.toLowerCase()
			)
				return;
			this.$request(this.$Api.Personal.CheckNameInDb, {
				name: this.name
			}).then(response => {
				this.nameInDb = response.data.yes;
				this.$refs.name.validate();
			});
		},
		save() {
			this.$refs.name.validate();
			this.$refs.password.validate();

			if (this.$refs.name.hasError || this.$refs.password.hasError) return;

			this.submitting = true;

			this.$request(this.$Api.Personal.SetMyName, {
				password: this.password,
				name: this.name
			})
				.then(async response => {
					await this.$store.dispatch("loadMyUserInfo");
					this.$successNotify();
					this.$router.push({ name: "Personal" });
				})
				.catch(error => {
					this.$errorNotify(error);
					this.submitting = false;
				});
		}
	},
	created() {
		this.title = this.$tl("title");
		this.rules = createRules.call(this);
		this.checkNameInDb = this.$throttle(this.checkNameInDb);
		this.$watch("name", this.checkNameInDb);
	}
};
</script>

<style lang="scss"></style>
