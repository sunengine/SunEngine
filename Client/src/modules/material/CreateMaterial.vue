<template>
	<SunPage class="create-material q-pa-md">
		<MaterialForm
			ref="form"
			class="create-material__material-form"
			:material="material"
			:categoriesNames="categoriesNames"
		/>

		<div class="create-material__btn-block q-mt-md">
			<q-btn
				:icon="$iconsSet.CreateMaterial.save"
				class="send-btn"
				no-caps
				:loading="loading"
				:label="$tl('sendBtn')"
				@click="send"
				color="send"
			>
				<LoaderSent slot="loading" />
			</q-btn>
			<q-btn
				no-caps
				:icon="$iconsSet.CreateMaterial.cancel"
				class="cancel-btn q-ml-sm"
				@click="$router.back()"
				:label="$t('Global.btn.cancel')"
				color="warning"
			/>
		</div>
	</SunPage>
</template>

<script>

export default {
	name: "CreateMaterial",
	components: {
		MaterialForm: sunImport("material", "MaterialForm")
	},
	mixins: [Page],
	props: {
		categoriesNames: {
			type: String,
			required: true
		},
		categoriesNamesExclude: {
			type: String,
			required: false
		},
		initialCategoryName: {
			type: String,
			required: false,
			default: ""
		}
	},
	data() {
		const getInitialCategoryName = () => {
			if (this.initialCategoryName) {
				const category = this.$store.getters.getCategory(this.initialCategoryName);
				if (
					category &&
					category.isMaterialsContainer &&
					category.categoryPersonalAccess?.MaterialWrite
				)
					return this.initialCategoryName;
			}
			return null;
		};

		return {
			material: {
				name: null,
				title: "",
				text: "",
				subTitle: null,
				settingsJson: null,
				tags: [],
				categoryName: getInitialCategoryName(),
				isCommentsBlocked: false,
				isHidden: false
			},
			loading: false
		};
	},
	methods: {
		send() {
			this.$refs.form.start = false;
			this.$refs.form.validate();
			if (this.$refs.form.hasError) return;

			this.loading = true;

			const data = {
				categoryName: this.material.categoryName,
				title: this.material.title,
				text: this.material.text,
				tags: this.material.tags.join(",")
			};

			if (this.material.name) data.name = this.material.name;
			if (this.material.subTitle) data.subTitle = this.material.subTitle;
			if (this.material.settingsJson)
				data.settingsJson = this.material.settingsJson;

			this.$request(this.$Api.Materials.Create, data)
				.then(() => {
					this.$successNotify();
					this.$router.push(this.$refs.form.category.getRoute());
				})
				.catch(error => {
					this.$errorNotify(error);
					this.loading = false;
				});
		}
	},
	created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss"></style>
