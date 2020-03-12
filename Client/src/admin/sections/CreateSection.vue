<template>
	<SunPage class="create-section page-padding">
		<PageHeader :title="title" />

		<SectionForm
			:editMode="false"
			v-if="section"
			ref="form"
			class="q-mb-xl"
			:section="section"
		/>

		<div class="create-section__btn-block q-mt-lg q-gutter-md">
			<q-btn
				:icon="$iconsSet.CreateSection.add"
				class="send-btn"
				no-caps
				:loading="loading"
				:label="$tl('createBtn')"
				@click="save"
				color="send"
			>
				<LoaderSent slot="loading" />
			</q-btn>
			<q-btn
				no-caps
				:icon="$iconsSet.CreateSection.cancel"
				class="cancel-btn"
				@click="$router.back()"
				:label="$tl('cancelBtn')"
				color="warning"
			/>
		</div>
	</SunPage>
</template>

<script>
export default {
	name: "CreateSection",
	components: {
		SectionForm: sunImport("sections","SectionForm"),
	},
	mixins: [Page],
	props: {
		templateName: {
			type: String,
			required: true
		}
	},
	data() {
		return {
			section: null,
			loading: false
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("SectionsAdmin");
		}
	},
	methods: {
		getSectionTemplate() {
			this.$request(this.$AdminApi.SectionsAdmin.GetSectionTemplate, {
				templateName: this.templateName
			}).then(response => {
				this.section = response.data;
			});
		},
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

			this.$request(this.$AdminApi.SectionsAdmin.AddSection, data, true)
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
		}
	},
	created() {
		this.title = this.$tl("title");
		this.getSectionTemplate();
	}
};
</script>

<style scoped></style>
