<template>
	<SunPage class="send-private-message page-padding">
		<PageHeader>
			<template slot="title">
				{{ $tl("titleStart") }}
				<router-link
					:to="{ name: 'User', params: { link: userLink } }"
					class="link q-ml-sm"
				>
					<!--	<q-icon name="far fa-user" />-->
					{{ userName }}
				</router-link>
			</template>
		</PageHeader>

		<q-editor
			class="send-private-message__editor q-mb-md"
			:toolbar="sendPrivateMessageToolbar"
			ref="htmlEditor"
			v-model="text"
		/>

		<q-btn
			class="send-btn q-mr-sm"
			no-caps
			:icon="$iconsSet.SendPrivateMessage.send"
			@click="send"
			:loading="loading"
			:label="$tl('sendBtn')"
		>
			<LoaderSent slot="loading" />
		</q-btn>
		<q-btn
			class="cancel-btn"
			no-caps
			:icon="$iconsSet.SendPrivateMessage.cancel"
			@click="$router.back()"
			:label="$t('Global.btn.cancel')"
		/>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "SendPrivateMessage",
	mixins: [Page],
	props: {
		userId: {
			type: Number,
			required: true
		},
		userName: {
			type: String,
			required: true
		},
		userLink: {
			type: String,
			required: true
		}
	},
	data() {
		return {
			text: "",
			loading: false
		};
	},
	methods: {
		send() {
			this.$request(this.$Api.Profile.SendPrivateMessage, {
				userId: this.userId,
				text: this.text
			})
				.then(_ => {
					this.$successNotify();
					this.loading = false;
					this.$router.back();
				})
				.catch(error => {
					this.$errorNotify(error);
					this.loading = false;
				});
		}
	},
	created() {
		this.title = this.$tl("title");
		this.sendPrivateMessageToolbar = JSON.parse(
			config.Editor.SendPrivateMessageToolbar
		);
	}
};
</script>

<style lang="scss"></style>
