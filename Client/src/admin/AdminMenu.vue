<template>
	<nav>
		<q-list class="admin-menu sun-second-menu" no-border>
			<q-item exact :to="{ name: 'AdminInformation' }">
				<q-item-section avatar>
					<q-icon :name="$iconsSet.AdminMenu.AdminInformation" />
				</q-item-section>
				<q-item-section>
					<q-item-label>{{ $tl("adminInformation") }}</q-item-label>
					<q-item-label v-if="adminInformationCaption" caption>{{
						adminInformationCaption
					}}</q-item-label>
				</q-item-section>
			</q-item>

			<q-item :to="{ name: 'ConfigurationAdmin' }">
				<q-item-section avatar>
					<q-icon :name="$iconsSet.AdminMenu.ConfigurationAdmin" />
				</q-item-section>
				<q-item-section>
					<q-item-label>{{ $tl("configuration") }}</q-item-label>
					<q-item-label v-if="configurationCaption" caption>{{
						configurationCaption
					}}</q-item-label>
				</q-item-section>
			</q-item>

			<q-item :to="{ name: 'MenuItemsAdmin' }">
				<q-item-section avatar>
					<q-icon :name="$iconsSet.AdminMenu.MenuItemsAdmin" />
				</q-item-section>
				<q-item-section>
					<q-item-label>{{ $tl("menuItems") }}</q-item-label>
					<q-item-label v-if="menuItemsCaption" caption>{{
						menuItemsCaption
					}}</q-item-label>
				</q-item-section>
			</q-item>

			<q-item :to="{ name: 'CategoriesAdmin' }">
				<q-item-section avatar>
					<q-icon :name="$iconsSet.AdminMenu.CategoriesAdmin" />
				</q-item-section>
				<q-item-section>
					<q-item-label>{{ $tl("categories") }}</q-item-label>
					<q-item-label v-if="categoriesCaption" caption>{{
						categoriesCaption
					}}</q-item-label>
				</q-item-section>
			</q-item>

			<q-item :to="{ name: 'ComponentsAdmin' }">
				<q-item-section avatar>
					<q-icon :name="$iconsSet.AdminMenu.ComponentsAdmin" />
				</q-item-section>
				<q-item-section>
					<q-item-label>{{ $tl("components") }}</q-item-label>
					<q-item-label v-if="componentsCaption" caption>{{
						componentsCaption
					}}</q-item-label>
				</q-item-section>
			</q-item>

			<q-item :to="{ name: 'SkinsAdmin' }">
				<q-item-section avatar>
					<q-icon :name="$iconsSet.AdminMenu.SkinsAdmin" />
				</q-item-section>
				<q-item-section>
					<q-item-label>{{ $tl("skins") }}</q-item-label>
					<q-item-label v-if="skinsCaption" caption>{{ skinsCaption }}</q-item-label>
				</q-item-section>
			</q-item>

			<q-item :to="{ name: 'RolesPage' }">
				<q-item-section avatar>
					<q-icon :name="$iconsSet.AdminMenu.RolesPage" />
				</q-item-section>
				<q-item-section>
					<q-item-label>{{ $tl("rolesUsers") }}</q-item-label>
					<q-item-label v-if="rolesUsersCaption" caption>{{
						rolesUsersCaption
					}}</q-item-label>
				</q-item-section>
			</q-item>

			<q-item :to="{ name: 'RolesPermissions' }">
				<q-item-section avatar>
					<q-icon :name="$iconsSet.AdminMenu.RolesPermissions" />
				</q-item-section>
				<q-item-section>
					<q-item-label>{{ $tl("rolesPermissions") }}</q-item-label>
					<q-item-label v-if="rolesPermissionsCaption" caption>{{
						rolesPermissionsCaption
					}}</q-item-label>
				</q-item-section>
			</q-item>

			<q-item :to="{ name: 'DeletedElements' }">
				<q-item-section avatar>
					<q-icon :name="$iconsSet.AdminMenu.DeletedElements" />
				</q-item-section>
				<q-item-section>
					<q-item-label>{{ $tl("deletedElements") }}</q-item-label>
					<q-item-label v-if="deletedElementsCaption" caption>{{
						deletedElementsCaption
					}}</q-item-label>
				</q-item-section>
			</q-item>

			<q-expansion-item
				:label="$tl('systemTools')"
				expand-separator
				:caption="systemToolsCaption"
				:icon="$iconsSet.AdminMenu.systemTools"
			>
				<q-item clickable @click="resetCache">
					<q-item-section avatar>
						<q-icon color="info" :name="$iconsSet.AdminMenu.resetCache" />
					</q-item-section>
					<q-item-section>
						<q-item-label>{{ $tl("resetCache") }}</q-item-label>
						<q-item-label v-if="resetCacheCaption" caption>{{
							resetCacheCaption
						}}</q-item-label>
					</q-item-section>
				</q-item>

				<q-item :to="{ name: 'ImagesCleaner' }">
					<q-item-section avatar>
						<q-icon :name="$iconsSet.AdminMenu.ImagesCleaner" />
					</q-item-section>
					<q-item-section>
						<q-item-label>{{ $tl("imagesCleaner") }}</q-item-label>
						<q-item-label v-if="imagesCleanerCaption" caption>{{
							imagesCleanerCaption
						}}</q-item-label>
					</q-item-section>
				</q-item>

				<q-item :to="{ name: 'CypherSecrets' }">
					<q-item-section avatar>
						<q-icon :name="$iconsSet.AdminMenu.CypherSecrets" />
					</q-item-section>
					<q-item-section>
						<q-item-label>{{ $tl("cypherSecrets") }}</q-item-label>
						<q-item-label v-if="cypherSecretsCaption" caption>{{
							cypherSecretsCaption
						}}</q-item-label>
					</q-item-section>
				</q-item>
			</q-expansion-item>
		</q-list>
	</nav>
</template>

<script>
export default {
	name: "AdminMenu",
	computed: {
		adminInformationCaption() {
			return this.$tl("adminInformationCaption") ?? null;
		},
		menuItemsCaption() {
			return this.$tl("menuItemsCaption") ?? null;
		},
		categoriesCaption() {
			return this.$tl("categoriesCaption") ?? null;
		},
		componentsCaption() {
			return this.$tl("cypherSecretsCaption") ?? null;
		},
		skinsCaption() {
			return this.$tl("skinsCaption") ?? null;
		},
		rolesUsersCaption() {
			return this.$tl("rolesUsersCaption") ?? null;
		},
		rolesPermissionsCaption() {
			return this.$tl("rolesPermissionsCaption") ?? null;
		},
		configurationCaption() {
			return this.$tl("configurationCaption") ?? null;
		},
		systemToolsCaption() {
			return this.$tl("systemToolsCaption") ?? null;
		},
		imagesCleanerCaption() {
			return this.$tl("imagesCleanerCaption") ?? null;
		},
		deletedElementsCaption() {
			return this.$tl("deletedElementsCaption") ?? null;
		},
		cypherSecretsCaption() {
			return this.$tl("cypherSecretsCaption") ?? null;
		},
		resetCacheCaption() {
			return this.$tl("resetCacheCaption") ?? null;
		}
	},
	methods: {
		resetCache() {
			this.$request(this.$AdminApi.CacheAdmin.ResetAllCache).then(response => {
				this.$successNotify(this.$tl("resetCacheSuccess"));
			});
		}
	}
};
</script>

<style lang="scss"></style>
