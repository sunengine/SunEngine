<template>
	<SunPage class="load-photo flex column middle page-padding">
		<PageHeader :title="title" />

		<img
			class="load-photo__photo q-mb-xs"
			v-if="photo"
			width="300"
			:src="photo"
		/>
		<q-banner dense rounded class="bg-grey-1 text-grey-9">
			{{ $tl("maxPhotoSize") }} {{ photoSize }} {{ $t("Global.units.megabytes") }}
		</q-banner>
		<br />
		<input
			ref="file"
			type="file"
			accept="image/*"
			class="hidden"
			@change="handleFile"
		/>
		<q-btn
			no-caps
			class="load-photo__send-btn send-btn q-mb-xl"
			:loading="loading"
			:icon="$iconsSet.LoadPhoto.upload"
			:label="$tl('uploadNewPhotoBtn')"
			@click="upload"
		/>
		<q-btn
			no-caps
			v-if="!isDefault && !loading"
			class="load-photo__delete-btn delete-btn"
			:icon="$iconsSet.LoadPhoto.delete"
			:label="$tl('resetBtn')"
			@click="resetAvatar"
		/>
	</SunPage>
</template>

<script>
import { Page } from "mixins";
import { validateFileSize } from "sun";

const defaultAvatar = config.Misc.DefaultAvatar;

export default {
	name: "LoadPhoto",
	mixins: [Page],
	data() {
		return {
			loading: false
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Personal");
		},
		photoSize() {
			return Math.floor(config.Images.ImageRequestSizeLimitBytes / 1048576, 2);
		},
		photo() {
			if (
				this.$store &&
				this.$store.state &&
				this.$store.state.auth &&
				this.$store.state.auth.user.photo
			)
				return this.$store.state.auth.user.photo;
			return null;
		},
		isDefault() {
			if (
				this.$store &&
				this.$store.state &&
				this.$store.state.auth &&
				this.$store.state.auth.user.photo
			)
				return this.$store.state.auth.user.photo.endsWith(defaultAvatar);
		}
	},
	methods: {
		handleFile() {
			if (!this.$refs.file.files.length) return;

			const file = this.$refs.file.files[0];

			if (!validateFileSize(file)) {
				this.$refs.file.value = "";
				return;
			}

			const formData = new FormData();
			formData.append("file", file);

			this.loading = true;

			this.$request(this.$Api.UploadImages.UploadUserPhoto, formData)
				.then(async () => {
					await this.$store.dispatch("loadMyUserInfo");
					this.loading = false;
					this.$successNotify(this.$tl("avatarChangedSuccessNotify"));
				})
				.catch(error => {
					this.loading = false;
				});
		},
		upload() {
			this.$refs.file.click();
		},
		resetAvatar() {
			this.$request(this.$Api.Personal.RemoveMyAvatar)
				.then(async () => {
					await this.$store.dispatch("loadMyUserInfo");
					this.loading = false;
					this.$successNotify(this.$tl("avatarDeletedSuccessNotify"));
				})
				.catch(x => {
					this.loading = false;
					const msg = this.$t("Global.errorNotify");
					this.$q.notify({
						message: msg,
						timeout: 2000,
						color: "negative",
						position: "top"
					});
				});
		}
	},
	created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss"></style>
