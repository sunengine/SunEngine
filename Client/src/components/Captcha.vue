<template>
	<div class="captcha">
		<div class="captcha__img-block">
			<span class="captcha__wait-msg" v-if="waitToken">{{
				$t("Captcha.waitMessage")
			}}</span>
			<img
				class="block"
				v-else-if="token"
				:src="$apiPath($Api.Captcha.CaptchaImage'?token=' + token)"
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
			v-model="captchaText"
			:label="$tl('enterToken')"
			:rules="[value => !!value || this.$tl('required')]"
		>
			<template v-slot:prepend>
				<q-icon :name="$iconsSet.Register.hand" />
			</template>
		</q-input>
	</div>
</template>

<script>
export default {
	name: "Captcha",
	 data() {
	    return {
           captchaText: "",
           token: null,
           waitToken: false,
       }
	 },
	 methods: {
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
