<template>
	<SunPage class="edit-section page-padding">
		<PageHeader :title="title" />

		<SectionForm
			:editMode="true"
			v-if="section"
			ref="form"
			class="q-mb-xl"
			:section="section"
		/>

		<LoaderWait v-else />

		<div class="q-gutter-md">
			<q-btn
				:icon="$iconsSet.EditSection.save"
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
				:icon="$iconsSet.EditSection.cancel"
				class="cancel-btn q-ml-sm"
				@click="$router.back()"
				:label="$tl('cancelBtn')"
				color="warning"
			/>
			<q-btn
				no-caps
				:icon="$iconsSet.EditSection.delete"
				class="delete-btn q-ml-sm float-right"
				@click="removeSection()"
				:label="$tl('deleteBtn')"
			/>
		</div>
	</SunPage>
</template>

<script>
import { Page } from "mixins";
import { extend } from "quasar";

export default {
	name: "EditSection",
	mixins: [Page],
	props: {
		name: {
			type: String,
			required: true
		}
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("SectionsAdmin");
		}
	},
	data() {
		return {
			section: null,
			loading: false
		};
	},
	methods: {
		save() {
			const form = this.$refs.form;
			form.validate();
			if (form.hasError) return;

			this.loading = true;

			const data = JSON.parse(JSON.stringify(this.section));
			delete data.enums;
			data.options = {};
			for (const option of this.section.options)
				data.options[option.name] = option.value;

			data.options = JSON.stringify(data.options);

			console.log(data);

			this.$request(this.$AdminApi.SectionsAdmin.UpdateSection, data, true)
				.then(async () => {
					this.$successNotify();
					await this.$store.dispatch("loadAllSections");
					await this.$store.dispatch("setAllRoutes");
					this.$router.push({ name: "SectionsAdmin" });
				})
				.catch(error => {
					this.$errorNotify(error);
					this.loading = false;
				});
		},
		removeSection() {
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
					this.$request(this.$AdminApi.SectionsAdmin.DeleteSection, {
						sectionId: this.section.id
					})
						.then(() => {
							this.$successNotify(null, "warning");
							this.$router.push({ name: "SectionsAdmin" });
						})
						.catch(error => {
							this.$errorNotify(error);
							this.loading = false;
						})
				);
		},
		loadData() {
			this.$request(this.$AdminApi.SectionsAdmin.GetSection, {
				name: this.name
			}).then(response => {
				this.section = response.data;
			});
		}
	},
	beforeCreate() {
		this.$options.components.SectionForm = require("sun").SectionForm;
	},
	created() {
		this.title = this.$tl("title");
		this.loadData();
	}
};
</script>

<style scoped></style>
