<template>
	<q-layout class="layout" view="lHh LpR lfr">
		<header>
			<q-header class="layout__header">
				<q-toolbar class="layout__toolbar">
					<q-btn
						flat
						dense
						round
						@click="leftDrawerOpen = !leftDrawerOpen"
						aria-label="Menu"
					>
						<q-icon
							:name="$iconsSet.Layout.mainMenu"
							class="layout__toolbar__menu-btn"
						/>
					</q-btn>

					<q-toolbar-title class="layout__title-block">
						<router-link class="layout__title-link block" :to="{ name: 'Home' }">
							<div class="layout__title">{{ siteTitle }}</div>
							<div class="layout__sub-title" v-if="siteSubTitle">
								{{ siteSubTitle }}
							</div>
						</router-link>
					</q-toolbar-title>

					<template v-if="userName">
						<q-btn-dropdown
							no-caps
							v-if="$q.screen.gt.xs"
							flat
							class="layout__toolbar__user-btn"
						>
							<template slot="label">
								<q-avatar class="avatar layout__avatar q-mr-sm">
									<img :src="userAvatar" />
								</q-avatar>
								{{ userName }}
							</template>
							<UserMenu style="width:180px;" />
						</q-btn-dropdown>

						<q-btn v-else flat dense round>
							<q-avatar class="avatar layout__avatar">
								<img :src="userAvatar" />
							</q-avatar>

							<q-menu>
								<q-list class="sun-second-menu q-py-sm">
									<q-item class="avatar-menu-item">
										<q-item-section avatar>
											<q-avatar class="avatar layout__avatar">
												<img :src="userAvatar" />
											</q-avatar>
										</q-item-section>
										<q-item-section>
											<q-item-label>
												{{ userName }}
											</q-item-label>
										</q-item-section>
									</q-item>
									<UserMenu style="width:180px;" />
								</q-list>
							</q-menu>
						</q-btn>
					</template>

					<q-btn v-else flat dense round>
						<q-icon :name="$iconsSet.Layout.user" class="toolbar-user-btn" />
						<q-menu>
							<LoginRegisterMenu v-close-popup />
						</q-menu>
					</q-btn>

					<q-btn
						class="q-ml-sm"
						flat
						dense
						round
						@click="rightDrawerOpen = !rightDrawerOpen"
						aria-label="Menu"
						v-if="rightDrawerIs"
					>
						<q-icon
							:name="$iconsSet.Layout.secondMenu"
							class="layout__toolbar__menu-btn"
						/>
					</q-btn>
				</q-toolbar>
			</q-header>
		</header>

		<q-drawer v-model="leftDrawerOpen" bordered content-class="main-menu-drawer">
			<MainMenu />
		</q-drawer>

		<q-drawer
			v-if="rightDrawerIs"
			bordered
			side="right"
			v-model="rightDrawerOpen"
			content-class="side-menu-drawer"
		>
			<router-view name="navigation" />
		</q-drawer>

		<q-page-container :class="{ 'center-container': centered }">
			<q-toolbar
				id="toolbarBreadcrumbs"
				:class="{ 'page-padding': true, hidden: hideBreadcrumbs }"
			>
				<Breadcrumbs
					v-if="!hideBreadcrumbs"
					:category="breadcrumbsCategory"
					:pageTitle="pageTitle"
				/>
			</q-toolbar>

			<div class="inner-container">
				<router-view ref="rv" />
			</div>
			<q-resize-observer @resize="onResize" />
		</q-page-container>

		<footer>
			<q-footer class="layout__footer">
				<LinksMenu
					v-if="footerMenuItem"
					class="layout__footer-line"
					linkClasses="layout__footer-link"
					:menuItem="footerMenuItem"
				>
					<q-icon
						:name="$iconsSet.Layout.heart"
						class="layout__footer-separator-icon"
						size="12px"
					/>
				</LinksMenu>

				<!-- Do not remove this component from the layout. -->
				<!-- This component is the part of SunEngine user license - https://github.com/sunengine/SunEngine/blob/master/LICENSE.md . -->
				<SunEngineFooter class="layout__footer-line" />
			</q-footer>
		</footer>
	</q-layout>
</template>

<script>
import { mapState } from "vuex";

export default {
	name: "Layout",
	data() {
		return {
			leftDrawerOpen: this.$q.platform.is.desktop,
			rightDrawerOpen: this.$q.platform.is.desktop,
			centered: false,
			breadcrumbsHeight: null
		};
	},
	watch: {
		$route: function() {
			this.$nextTick(() => {
				this.centered = this.$refs?.rv?.$options?.centered;
			});
		}
	},
	computed: {
		siteTitle() {
			return config.Global.SiteTitle;
		},
		pageTitle() {
			return this.$store.state.currentPage?.title;
		},
		hideBreadcrumbs() {
			return this.$store.state.currentPage?.hideBreadcrumbs;
		},
		breadcrumbsCategory() {
			if (!this.$store.state.mounted) return null;
			return this.$refs?.rv?.breadcrumbsCategory ?? this.$refs?.rv?.category;
		},
		siteSubTitle() {
			return config.Global.SiteSubTitle;
		},
		rightDrawerIs: function() {
			return !!this.$route?.matched?.[0]?.components?.navigation;
		},
		footerMenuItem() {
			return this.$store.getters.getMenu("FooterMenu");
		},
		...mapState({
			userName: state => state.auth.user?.name,
			userAvatar: state => state.auth.user?.avatar
		})
	},
	beforeCreate() {
		this.$options.components.Breadcrumbs = require("sun").Breadcrumbs;
		this.$options.components.UserMenu = require("sun").UserMenu;
		this.$options.components.LoginRegisterMenu = require("sun").LoginRegisterMenu;
		this.$options.components.MainMenu = require("sun").MainMenu;
		this.$options.components.SunEngineFooter = require("sun").SunEngineFooter;
		this.$options.components.LinksMenu = require("sun").LinksMenu;
	},
	mounted() {
		const toolbarBreadcrumbs = document.getElementById("toolbarBreadcrumbs");
		this.breadcrumbsHeight = parseInt(
			window.getComputedStyle(toolbarBreadcrumbs).height
		);
	},
	created() {
	    this.$root.$layout = this;
	}
};
</script>

<style lang="scss"></style>
