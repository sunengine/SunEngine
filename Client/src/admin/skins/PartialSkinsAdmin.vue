﻿<template>
	<div class="partial-skins-admin">
		<div class="flex flex-center">
			<q-btn
				no-caps
				:icon="$iconsSet.PartialSkinsAdmin.upload"
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
			@change="uploadPartialSkin"
			class="hidden"
			accept=".zip"
			ref="file"
		/>

		<q-markup-table v-if="pSkins" wrap-cells>
			<tbody>
				<q-tr
					:key="pSkin.name"
					v-for="pSkin of pSkins"
					:class="
						pSkin.current ? 'partial-skins-admin__on' : 'partial-skins-admin__off'
					"
				>
					<q-td>{{ pSkin.name }}</q-td>
					<q-td>
					 <q-toggle v-model="pSkin.current" @input="(value) => enablePartialSkin(value,pSkin)" />
					</q-td>
					<q-td style="">{{ pSkin.description }}</q-td>
					<q-td>
						<q-btn
							:disable="pSkin.current"
							flat
							dense
							@click="deletePartialSkin(pSkin.name)"
							:icon="$iconsSet.PartialSkinsAdmin.delete"
						/>
					</q-td>
				</q-tr>
			</tbody>
		</q-markup-table>

		<LoaderWait v-else />
	</div>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "PartialSkinsAdmin",
	mixins: [Page],
	data() {
		return {
			pSkins: null,
			loading: false
		};
	},
	computed: {
		skinsDir() {
			return config.SkinsDir;
		}
	},
	methods: {
		showUploadDialog() {
			this.$refs.file.click();
		},
		uploadPartialSkin() {
			this.loading = true;
			const file = this.$refs.file.files[0];
			const formData = new FormData();
			formData.append("file", file);
			this.$request(this.$AdminApi.SkinsAdmin.UploadPartialSkin, formData)
				.then(response => {
					this.$successNotify(this.$tl("uploadSuccessNotify"));
					this.$refs.file.value = "";
					this.getAllPartialSkins();
				})
				.finally(_ => {
					this.loading = false;
				});
		},
		deletePartialSkin(name) {
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
					this.$request(this.$AdminApi.SkinsAdmin.DeletePartialSkin, {
						name: name
					}).then(_ => {
						this.$successNotify(this.$tl("deleteSuccessNotify"));
						this.getAllPartialSkins();
					});
				});
		},
		enablePartialSkin(value,skin) {
			this.$request(this.$AdminApi.SkinsAdmin.EnablePartialSkin, {
				name: skin.name,
				enable: value
			}).then(_ => {
				//this.$successNotify();
				this.getAllPartialSkins();
			});
		},
		getAllPartialSkins() {
			this.$request(this.$AdminApi.SkinsAdmin.GetAllPartialSkins).then(
				response => {
					for (const skin of response.data) {
						skin.showInfo = false;
						skin.showContacts = false;
						skin.loading = false;
					}
					this.pSkins = response.data;
				}
			);
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
		this.getAllPartialSkins();
	}
};
</script>

<style lang="scss">
.partial-skins-admin__on {
	background-color: $green-1;
}

.partial-skins-admin__off {
	.partial-skins-admin__enable-btn {
		background-color: $green-1;
	}
}
</style>
