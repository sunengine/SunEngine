﻿<template>
	<div class="main-skins-admin">
		<div class="flex flex-center">
			<q-btn
				no-caps
				:icon="$iconsSet.MainSkinsAdmin.upload"
				@click="showUploadDialog"
				class="skins-admin__post-btn post-btn q-mb-lg"
				:loading="loading"
				:label="$tl('upload')"
			>
				<LoaderSent slot="loading" />
			</q-btn>
		</div>

		<input
			type="file"
			@change="uploadSkin"
			class="hidden"
			accept=".zip"
			ref="file"
		/>

		<div v-if="skins" class="row q-gutter-lg">
			<q-card :key="skin.name" class="skins-admin__card" v-for="skin in skins">
				<div class="skins-admin__card-image-section">
					<q-img
						:class="{ hidden: skin.showInfo }"
						:src="skinImgUrl(skin.name)"
						class="skins-admin__skin-img"
					>
						<div class="skins-admin__preview-btn absolute-bottom-right">
							<q-icon :name="$iconsSet.MainSkinsAdmin.preview" size="20px"> </q-icon>
							<q-tooltip anchor="center middle" self="center middle">
								<img :src="skinImgUrl(skin.name)" width="800" />
							</q-tooltip>
						</div>
					</q-img>
					<q-card-section :class="{ hidden: !skin.showInfo }">
						<div>
							{{ $tl("author") }}
							<a
								class="link"
								v-if="skin.contacts"
								href="#"
								@click.prevent.stop="skin.showContacts = !skin.showContacts"
							>
								{{ skin.author }}</a
							>
							<span v-else> {{ skin.author }} </span>
						</div>
						<q-slide-transition>
							<div v-if="skin.contacts && skin.showContacts">
								{{ $tl("contacts") }}
								<span :key="index" v-for="(contact, index) of skin.contacts">
									<a v-if="contact.startsWith('http')" :href="contact" class="link">{{
										contact
									}}</a>
									<template v-else> {{ contact }} </template>
									<template v-if="index != skin.contacts.length - 1">, </template>
								</span>
							</div>
						</q-slide-transition>
						<div>{{ $tl("version") }} {{ skin.version }}</div>
						<div v-if="skin.sourceUrl">
							<a class="link" target="_blank" :href="skin.sourceUrl">{{
								$tl("link")
							}}</a>
						</div>
						<div v-if="skin.description">
							{{ $tl("description") }} {{ skin.description }}
						</div>
					</q-card-section>
				</div>
				<q-card-section class="skins-admin__skin-name h6">
					{{ skin.name }}
				</q-card-section>

				<q-card-actions align="around">
					<q-btn
						v-if="skin.current"
						class="skins-admin__current-btn"
						flat
						no-caps
						:label="$tl('current')"
						:icon="$iconsSet.MainSkinsAdmin.current"
					/>

					<q-btn
						flat
						v-if="!skin.current"
						:loading="skin.loading"
						no-caps
						@click="changeSkin(skin.name)"
						:icon="$iconsSet.MainSkinsAdmin.set"
						class="skins-admin__send-btn"
						:label="$tl('set')"
					>
						<LoaderSent slot="loading" />
					</q-btn>

					<q-btn
						class="skins-admin__delete-btn"
						no-caps
						@click="showSkinInfo(skin)"
						flat
						:icon="$iconsSet.MainSkinsAdmin.info"
					/>
					<q-btn
						v-if="!skin.current"
						class="skins-admin__info-btn"
						no-caps
						@click="deleteSkin(skin.name)"
						flat
						:icon="$iconsSet.MainSkinsAdmin.delete"
					/>
				</q-card-actions>
			</q-card>
		</div>
		<LoaderWait v-else />

		<q-banner rounded class="skins-admin__info shadow-1 q-mt-xl">
			{{ $tl("info") }}
			<a
				class="skins-admin__info-link link"
				href="https://github.com/sunengine/SunEngine.Skins"
				target="_blank"
			>
				https://github.com/sunengine/SunEngine.Skins
			</a>
		</q-banner>
	</div>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "MainSkinsAdmin",
	mixins: [Page],
	data() {
		return {
			skins: null,
			loading: false
		};
	},
	methods: {
		skinImgUrl(skinName) {
			return this.$buildPath(config.Global.SkinsUrl, skinName, "preview.png");
		},
		showUploadDialog() {
			this.$refs.file.click();
		},
		uploadSkin() {
			this.loading = true;
			const file = this.$refs.file.files[0];
			const formData = new FormData();
			formData.append("file", file);
			this.$request(this.$AdminApi.SkinsAdmin.UploadSkin, formData)
				.then(response => {
					this.$successNotify(this.$tl("uploadSuccessNotify"));
					this.$refs.file.value = "";
					this.getAllSkins();
				})
				.finally(_ => {
					this.loading = false;
				});
		},
		deleteSkin(name) {
			const deleteMsg = this.$tl("deleteMsg");
			const btnDeleteOk = this.$tl("btnDeleteOk");
			const btnDeleteCancel = this.$tl("btnDeleteCancel");

			this.$q
				.dialog({
					message: deleteMsg,
					ok: btnDeleteOk,
					cancel: btnDeleteCancel
				})
				.onOk(() => {
					this.$request(this.$AdminApi.SkinsAdmin.DeleteSkin, {
						name: name
					}).then(_ => {
						this.$successNotify(this.$tl("deleteSuccessNotify"));
						this.getAllSkins();
					});
				});
		},
		changeSkin(name) {
			this.skins.find(x => x.name === name).loading = true;

			this.$request(this.$AdminApi.SkinsAdmin.ChangeSkin, {
				name: name
			})
				.then(_ => {
					this.$successNotify();
					this.getAllSkins();
				})
				.finally(_ => {
					this.skins.find(x => x.name === name).loading = false;
				});
		},
		getAllSkins() {
			this.$request(this.$AdminApi.SkinsAdmin.GetAllSkins).then(response => {
				for (const skin of response.data) {
					skin.showInfo = false;
					skin.showContacts = false;
					skin.loading = false;
				}
				this.skins = response.data;
			});
		},
		showSkinInfo(skin) {
			skin.showInfo = !!!skin.showInfo;
		}
	},
	beforeCreate() {
		this.$options.components.LoaderWait = require("sun").LoaderWait;
		this.$options.components.LoaderSent = require("sun").LoaderSent;
	},
	created() {
		this.title = this.$tl("title");
		this.getAllSkins();
	}
};
</script>

<style lang="scss">
.skins-admin__skin-name {
	//font-family: "Open Sans", sans-serif;
	text-align: center;
	font-weight: 500;
	font-size: 1.3em;
	letter-spacing: 0.1px;
	padding: 8px !important;
	background-color: rgba(0, 0, 0, 0.15);
	color: $grey-9;
}

.skins-admin__card-image-section {
	height: 172px;
	overflow-y: auto;
}

.skins-admin__card {
	width: 330px;
}

.skins-admin__delete-btn,
.skins-admin__preview-btn {
	.q-icon {
		color: $grey-8;
	}
}

.skins-admin__current-btn {
	.q-icon {
		color: $green-5;
	}
}

.skins-admin__send-btn {
	background-color: $green-1;
	// color: white;
	.q-icon {
		color: $green-5;
	}
}

.skins-admin__preview-btn {
	background-color: rgba(0, 0, 0, 0.22) !important;
	border-radius: 6px 0px 0px 0px;

	padding: 10px !important;

	.q-icon {
		color: white;
	}
}

.skins-admin__info {
	margin-top: 100px;
	background-color: $grey-2;
	border: 1px solid #e6e6e6;
}
</style>
