<template>
	<div
		:class="{
			'menu-admin-item': true,
			'menu-admin-item--hidden': menuItem.isHidden
		}"
	>
		<span class="item-block">
			<span class="menu-admin-item__up-down">
				<q-btn
					:icon="$iconsSet.MenuAdminItem.up"
					:disabled="isFirst"
					@click="$emit('up', menuItem)"
					color="positive"
					dense
					size="10px"
					flat
				>
					<q-tooltip :delay="1000">
						{{ $tl("moveUpBtnTooltip") }}
					</q-tooltip>
				</q-btn>

				<q-btn
					class="menu-admin-item__btn-down"
					:disabled="isLast"
					@click="$emit('down', menuItem)"
					color="positive"
					dense
					size="10px"
					flat
					:icon="$iconsSet.MenuAdminItem.down"
				>
					<q-tooltip :delay="1000">
						{{ $tl("moveDownBtnTooltip") }}
					</q-tooltip>
				</q-btn>
			</span>

			<q-icon
				:name="menuItem.icon ? menuItem.icon : $iconsSet.MenuAdminItem.blank"
				class="q-ml-md"
				color="grey-6"
			/>
			<span class="q-ml-md q-mr-lg menu-admin-item__text">{{
				menuItem.title
			}}</span>

			<q-btn
				class="menu-admin-item__btn-edit"
				@click="$emit('edit', menuItem)"
				:icon="$iconsSet.MenuAdminItem.edit"
				color="info"
				dense
				size="10px"
				flat
			>
				<q-tooltip :delay="1000">
					{{ $tl("editBtnTooltip") }}
				</q-tooltip>
			</q-btn>

			<q-btn
				class="menu-admin-item__btn-change-is-hidden"
				@click="$emit('changeIsHidden', menuItem)"
				:icon="!menuItem.isHidden ? $iconsSet.MenuAdminItem.eye : $iconsSet.MenuAdminItem.eyeSlash"
				:color="!menuItem.isHidden ? 'info' : 'grey-5'"
				dense
				size="10px"
				flat
			>
				<q-tooltip :delay="1000">
					{{ $tl("changeIsHiddenBtnTooltip") }}
				</q-tooltip>
			</q-btn>

			<q-btn
				class="menu-admin-item__btn-add"
				@click="$emit('add', menuItem)"
				:icon="$iconsSet.MenuAdminItem.add"
				color="info"
				dense
				size="10px"
				flat
			>
				<q-tooltip :delay="1000">
					{{ $tl("addSubMenuItemBtnTooltip") }}
				</q-tooltip>
			</q-btn>

			<q-btn
				class="menu-admin-item__btn-to"
				:disabled="!(to || menuItem.externalUrl)"
				type="a"
				:to="to"
				@click="goExternal"
				:icon="$iconsSet.MenuAdminItem.goTo"
				color="info"
				dense
				size="10px"
				flat
			>
				<q-tooltip :delay="1000">
					{{ $tl("goToBtnTooltip") }}
				</q-tooltip>
			</q-btn>

			<q-btn
				class="menu-admin-item__btn-delete"
				@click="$emit('deleteMenuItem', menuItem)"
				:icon="$iconsSet.MenuAdminItem.delete"
				color="warning"
				dense
				size="10px"
				flat
			>
				<q-tooltip :delay="1000">
					{{ $tl("deleteBtnTooltip") }}
				</q-tooltip>
			</q-btn>

			<span v-if="menuItem.name" class="menu-admin-item__text q-ml-md"
				>[ {{ menuItem.name }} ]</span
			>
		</span>

		<div v-if="menuItem.subMenuItems" class="padding-mi">
			<MenuAdminItem
				:menuItem="subMenuItem"
				:isFirst="index === 0"
				:isLast="index === lastIndex"
				:key="subMenuItem.id"
				v-for="(subMenuItem, index) in menuItem.subMenuItems"
				v-on="$listeners"
			/>
		</div>
	</div>
</template>

<script>
export default {
	name: "MenuAdminItem",
	props: {
		menuItem: {
			type: Object,
			required: true
		},
		isFirst: {
			type: Boolean,
			required: true
		},
		isLast: {
			type: Boolean,
			required: true
		}
	},
	computed: {
		to() {
			if (this.menuItem.routeName) {
				let rez = {
					name: this.menuItem.routeName
				};

				if (this.menuItem.routeParamsJson) {
					try {
						rez.params = JSON.parse(this.menuItem.routeParamsJson);
					} catch (e) {}
				}

				return rez;
			}
		},
		lastIndex() {
			return this.menuItem.subMenuItems.length - 1;
		}
	},
	methods: {
		goExternal() {
			if (this.menuItem.externalUrl) window.open(this.menuItem.externalUrl);
		}
	},
	beforeCreate() {
		this.$options.components.MenuAdminItem = require("sun").MenuAdminItem;
	}
};
</script>

<style lang="scss">
.menu-admin-item__text {
}

.menu-admin-item--hidden {
	* {
		color: $grey-5;
	}
}

.menu-admin-item {
	.q-btn:disabled,
	.q-btn[disabled] {
		color: $grey-5;
	}

	.padding-mi {
		padding-left: 25px;
	}
}
</style>
