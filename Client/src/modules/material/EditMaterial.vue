<template>
	<SunPage class="edit-material q-pa-md">
		<div v-if="material.deletedDate" class="edit-material__delete-date text-red">
			<q-chip
				icon="fas fa-trash"
				color="red"
				text-color="white"
				:label="$tl('deleted')"
			/>
		</div>

		<MaterialForm
			class="edit-material__material-form"
			ref="form"
			:material="material"
			:categories-nodes="categoryNodes"
		/>

		<div class="edit-material__btn-block flex q-gutter-md q-mt-lg">
			<q-btn
				:icon="$iconsSet.EditMaterial.save"
				class="send-btn"
				no-caps
				:loading="loading"
				:label="$tl('saveBtn')"
				@click="save"
			>
				<LoaderSent slot="loading" />
			</q-btn>

			<q-btn
				no-caps
				:icon="$iconsSet.EditMaterial.cancel"
				class="cancel-btn"
				@click="$router.back()"
				:label="$tl('cancelBtn')"
			/>

			<q-space />

			<q-btn
				v-if="!material.deletedDate && canDelete"
				no-caps
				:icon="$iconsSet.EditMaterial.delete"
				class="delete-btn"
				@click="deleteMaterial"
				:label="$tl('deleteBtn')"
			/>

			<q-btn
				v-if="material.deletedDate && canRestore"
				class="restore-btn"
				no-caps
				icon="fas fa-trash-restore"
				@click="restoreMaterial"
				:label="$tl('restoreBtn')"
			/>
		</div>
	</SunPage>
</template>

<script>
import { getWhereToMove } from "sun";
import { deleteMaterial } from "sun";
import { restoreMaterial } from "sun";
import { canDeleteMaterial } from "sun";
import { canRestoreMaterial } from "sun";
import { Page } from "mixins";

export default {
	name: "EditMaterial",
	mixins: [Page],
	props: {
		id: {
			type: Number,
			required: true
		}
	},
	data() {
		return {
			material: {
				name: null,
				title: "",
				text: "",
				subTitle: null,
				settingsJson: null,
				tags: [],
				isHidden: false,
				isBlockComments: false
			},
			comments: null,
			loading: false
		};
	},
	computed: {
		categoryNodes() {
			return getWhereToMove(this.$store);
		},
		category() {
			return this.$store.getters.getCategory(this.material.categoryName);
		},
		canDelete() {
			return canDeleteMaterial.call(this);
		},
		canRestore() {
			return canRestoreMaterial.call(this);
		}
	},
	methods: {
		deleteMaterial() {
			deleteMaterial.call(this);
		},
		restoreMaterial() {
			restoreMaterial.call(this);
		},
		async save() {
			this.$refs.form.start = false;
			this.$refs.form.validate();
			if (this.$refs.form.hasError) return;
			this.loading = true;

			const data = {
				id: this.id,
				categoryName: this.material.categoryName,
				title: this.material.title,
				text: this.material.text,
				tags: this.material.tags.join(","),
				isHidden: this.material.isHidden,
				isCommentsBlocked: this.material.isCommentsBlocked
			};

			if (this.material.name) data.name = this.material.name;
			if (this.material.subTitle) data.subTitle = this.material.subTitle;
			if (this.material.settingsJson)
				data.settingsJson = this.material.settingsJson;

			await this.$request(this.$Api.Materials.Update, data)
				.then(() => {
					this.$successNotify();
					this.$router.push(this.category.getRoute());
				})
				.catch(error => {
					this.$errorNotify(error);
					this.loading = false;
				});
		},
		async loadData() {
			await this.$request(this.$Api.Materials.Get, {
				idOrName: this.id
			}).then(response => {
				this.material = response.data;
			});
			await this.$request(this.$Api.Comments.GetMaterialComments, {
				materialId: this.material.id
			}).then(response => {
				this.comments = response.data;
			});
		}
	},
	beforeCreate() {
		this.$options.components.MaterialForm = require("sun").MaterialForm;
	},
	created() {
		this.title = this.$tl("title");
		this.loadData();
	}
};
</script>

<style lang="scss"></style>
