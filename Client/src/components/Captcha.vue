<template>
	<div class="captcha">
		<div class="captcha__img-block">
			<span class="captcha__wait-msg" v-if="waitToken">{{
				$t("Captcha.waitMessage")
			}}</span>
			<img
				class="block"
				v-else-if="token"
				:src="$apiPath($Api.Captcha.CaptchaImage + '?token=' + token)"
			/>

			<q-btn
				class="captcha__refresh-btn shadow-1 q-mt-sm block"
				color="lime-6"
				@click="GetToken"
				size="sm"
				no-caps
				:icon="$iconsSet.Captcha.refresh"
				:label="$t('Captcha.newMessageBtn')"
			/>
		</div>
		<q-input
			ref="captcha"
			v-model="text"
			@input="handleInput"
			:label="$tl('enterToken')"
			:rules="[value => !!value || this.$tl('required')]"
		>
			<template v-slot:prepend>
				<q-icon :name="$iconsSet.Captcha.hand" />
			</template>
		</q-input>
	</div>
</template>

<script>
export default {
	name: "Captcha",
	props: {
		value: {
			type: String,
			required: false
		}
	},
	data() {
		return {
			text: this.value,
			token: null,
			waitToken: false
		};
	},
	computed: {
		hasError() {
			return this.$refs.captcha.hasError;
		}
	},
	methods: {
		handleInput() {
			this.$emit("input", this.text);
		},
		validate() {
			this.$refs.captcha.validate();
		},
		GetToken() {
			return this.$request(this.$Api.Captcha.GetCaptchaKey)
				.then(response => {
					this.token = response.data;
					this.waitToken = false;
				})
				.catch(x => {
					if (x.response.data.code === "SpamProtection") this.waitToken = true;
				});
		}
	},
	created() {
		this.GetToken();
	}
};
</script>

<style lang="scss">
.captcha__img-block {
	padding: 10px 10px 10px 44px;
	border-radius: 5px;
	background-color: #f0f4c3;
}
</style>
