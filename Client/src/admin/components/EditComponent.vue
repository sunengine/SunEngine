<template>
	<SunPage class="edit-component page-padding">
		<PageHeader :title="title" />

		<ComponentForm
			:editMode="true"
			v-if="component"
			ref="form"
			class="q-mb-xl"
			:component="component"
		/>

		<LoaderWait v-else />

		<div class="q-gutter-md">
			<q-btn
				:icon="$iconsSet.EditComponent.save"
				class="send-btn"
				no-caps
				:loading="loading"
				:label="$tl('saveBtn')"
				@click="save"
				color="send"
			>
				<LoaderSent slot="loading" />
			</q-btn>
			<q-btn
				no-caps
				:icon="$iconsSet.EditComponent.cancel"
				class="cancel-btn q-ml-sm"
				@click="$router.back()"
				:label="$tl('cancelBtn')"
				color="warning"
			/>
			<q-btn
				no-caps
				:icon="$iconsSet.EditComponent.delete"
				class="delete-btn q-ml-sm float-right"
				@click="removeComponent()"
				:label="$tl('deleteBtn')"
			/>
		</div>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "EditComponent",
	mixins: [Page],
	props: {
		name: {
			type: String,
			required: true
		}
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("ComponentsAdmin");
		}
	},
	data() {
		return {
			component: null,
			loading: false
		};
	},
	methods: {
		save() {
			const form = this.$refs.form;
			form.validate();
			if (form.hasError) return;

			this.loading = true;

			this.$request(
				this.$AdminApi.ComponentsAdmin.UpdateComponent,
				this.component,
				true
			)
				.then(async () => {
					this.$successNotify();
					await this.$store.dispatch("loadAllComponents");
					await this.$store.dispatch("setAllRoutes");
					this.$router.push({ name: "ComponentsAdmin" });
				})
				.catch(error => {
					this.$errorNotify(error);
					this.loading = false;
				});
		},
		removeComponent() {
			const deleteMsg = this.$tl("deleteMsg");
			const btnDeleteOk = this.$tl("btnDeleteOk");
			const btnDeleteCancel = this.$tl("btnDeleteCancel");

			this.$q
				.dialog({
					message: deleteMsg,
					ok: btnDeleteOk,
					cancel: btnDeleteCancel
				})
				.onOk(() =>
					this.$request(this.$AdminApi.ComponentsAdmin.DeleteComponent, {
						componentId: this.component.id
					})
						.then(() => {
							this.$successNotify(null, "warning");
							this.$router.push({ name: "ComponentsAdmin" });
						})
						.catch(error => {
							this.$errorNotify(error);
							this.loading = false;
						})
				);
		},
		loadData() {
			this.$request(this.$AdminApi.ComponentsAdmin.GetComponent, {
				name: this.name
			}).then(response => {
				this.component = response.data;
			});
		}
	},
	beforeCreate() {
		this.$options.components.LoaderSent = require("sun").LoaderSent;
		this.$options.components.LoaderWait = require("sun").LoaderWait;
		this.$options.components.ComponentForm = require("sun").ComponentForm;
	},
	created() {
		this.title = this.$tl("title");
		this.loadData();
	}
};
</script>

<style scoped></style>
