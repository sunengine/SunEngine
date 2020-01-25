<template>
	<SunPage class="images-cleaner page-padding">
		<PageHeader :title="title" />

		<q-banner rounded class="bg-amber-2 q-mb-md">
			{{ $tl("info") }}
		</q-banner>

		<div class="images-cleaner__btn-block q-gutter-md flex q-mb-xl">
			<q-btn
				v-if="images"
				:icon="$iconsSet.ImagesCleaner.clean"
				class="send-btn"
				:loading="loading"
				@click="clear()"
				no-caps
				:label="$tl('clearBtn')"
			>
				<LoaderSent slot="loading">
					{{ $tl("working") }}
				</LoaderSent>
			</q-btn>
			<q-space />
			<q-btn
				no-caps
				class="refresh-btn q-ml"
				color="info"
				:icon="$iconsSet.ImagesCleaner.refresh"
				@click="reloadImages()"
				:label="$tl('refreshBtn')"
			/>
		</div>

		<div
			v-if="images"
			class="images-cleaner__img-block img flex row q-col-gutter-sm"
		>
			<img
				v-for="image in images"
				:src="$imagePath(image)"
				height="80"
				width="80"
				class="images-cleaner__clean-img"
			/>
		</div>
		<q-banner rounded class="images-cleaner__empty-result bg-grey-3" v-else>
			{{ $tl("emptyResult") }}
		</q-banner>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "ImagesCleaner",
	mixins: [Page],
	data() {
		return {
			imagesDeleted: null,
			images: null,
			loading: false
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Admin");
		}
	},
	methods: {
		clear() {
			this.loading = true;
			this.$request(this.$AdminApi.ImagesCleaner.DeleteImages)
				.then(response => {
					this.imagesDeleted = response.data.imagesDeleted;
					this.$successNotify(this.$tl("clearCount") + this.imagesDeleted);
					this.loadImages();
				})
				.catch(error => {
					this.$errorNotify(error);
				})
				.finally(_ => {
					this.loading = false;
				});
		},

		loadImages() {
			this.$request(this.$AdminApi.ImagesCleaner.GetAllImages)
				.then(response => {
					if (response.data.length !== 0) this.images = response.data;
					else this.images = null;
				})
				.catch(error => {
					this.$errorNotify(error);
				});
		},
		async reloadImages() {
			await this.loadImages();
			this.images
				? this.$successNotify()
				: this.$successNotify(this.$tl("emptyResult"));
		}
	},
	beforeCreate() {
		this.$options.components.LoaderSent = require("sun").LoaderSent;
	},
	created() {
		this.title = this.$tl("title");
		this.loadImages();
	}
};
</script>
<style lang="scss">
.images-cleaner__clean-img {
	object-fit: cover;
	width: 100px;
	height: 110px;
}

/*.images-cleaner__send-btn {
      background-color: $info;
      color: white;
    }*/
</style>
