<template>
	<div class="profile-roles">
		<div v-if="userRoles">
			<div class="profile-roles__user-groups">
				<div class="profile-roles__roles-title">{{ $tl("roles") }}</div>

				<div class="profile-roles__roles-block q-my-md">
					<span
						class="profile-roles__group"
						v-for="role in userRoles"
						:class="'group-' + role.name.toLowerCase()"
						>{{ role.title }}</span
					>
				</div>
			</div>

			<div class="flex q-gutter-md">
				<q-btn
					class="grow"
					@click="add = true"
					no-caps
					color="positive"
					:icon="$iconsSet.ProfileRoles.plus"
					:label="$tl('addRoleBtn')"
				/>

				<q-btn
					class="grow"
					@click="remove = true"
					no-caps
					color="negative"
					:icon="$iconsSet.ProfileRoles.minus"
					:label="$tl('removeRoleBtn')"
				/>

				<q-dialog ref="addRole" class="profile-roles__dialog-add" v-model="add">
					<div class="bg-white">
						<q-list>
							<q-toolbar class="bg-positive text-white shadow-2">
								<q-toolbar-title>
									<q-icon :name="$iconsSet.ProfileRoles.plus" class="q-mr-sm" />
									{{ $tl("addRoleBtn") }}
								</q-toolbar-title>
							</q-toolbar>
							<q-item
								:key="role.name"
								clickable
								@click="addToRoleConfirm(role)"
								v-for="role in availableRoles"
							>
								<q-item-section>
									<q-item-label class="text-blue">
										{{ role.title }}
									</q-item-label>
								</q-item-section>
							</q-item>
						</q-list>
						<div class="text-center q-my-md">
							<q-btn
								flat
								@click="$refs.addRole.hide()"
								class="self-center cancel-btn"
								:label="$t('Global.btn.cancel')"
							/>
						</div>
					</div>
				</q-dialog>

				<q-dialog
					ref="remove"
					class="profile-roles__dialog-remove"
					v-model="remove"
				>
					<div class="bg-white">
						<q-list>
							<q-toolbar class="bg-negative text-white shadow-2">
								<q-toolbar-title>
									<q-icon :name="$iconsSet.ProfileRoles.minus" class="q-mr-sm" />
									{{ $tl("removeRoleBtn") }}
								</q-toolbar-title>
							</q-toolbar>
							<q-item
								:key="role.name"
								clickable
								@click="removeFromRoleConfirm(role)"
								v-for="role in userRoles"
							>
								<q-item-section>
									<q-item-label>
										{{ role.title }}
									</q-item-label>
								</q-item-section>
							</q-item>
						</q-list>
						<div class="text-center q-my-md">
							<q-btn
								flat
								@click="$refs.remove.hide()"
								class="self-center cancel-btn"
								:label="$t('Global.btn.cancel')"
							/>
						</div>
					</div>
				</q-dialog>
			</div>
		</div>

		<LoaderWait v-else />
	</div>
</template>

<script>
export default {
	name: "ProfileRoles",
	props: {
		userId: {
			type: Number,
			required: true
		}
	},
	data() {
		return {
			allRoles: null,
			userRoles: null,
			availableRoles: null,
			add: false,
			remove: false
		};
	},
	methods: {
		addToRoleConfirm(role) {
			this.add = false;

			const title = this.$tl("addRoleConfirmTitle", { roleName: role.title });
			const message = this.$tl("addRoleConfirmMessage", { roleName: role.title });
			const addRoleConfirmOkBtn = this.$tl("addRoleConfirmOkBtn");
			const cancelBtn = this.$tl("addRoleConfirmCancelBtn");

			this.$q
				.dialog({
					title: title,
					message: message,
					ok: addRoleConfirmOkBtn,
					cancel: cancelBtn
				})
				.onOk(() => {
					this.addToRole(role);
				});
		},
		removeFromRoleConfirm(role) {
			this.remove = false;

			const title = this.$tl("removeRoleConfirmTitle", { roleName: role.title });
			const message = this.$tl("removeRoleConfirmMessage", {
				roleName: role.title
			});
			const removeRoleConfirmOkBtn = this.$tl("removeRoleConfirmOkBtn");
			const cancelBtn = this.$tl("removeRoleConfirmCancelBtn");

			this.$q
				.dialog({
					title: title,
					message: message,
					ok: removeRoleConfirmOkBtn,
					cancel: cancelBtn
				})
				.onOk(() => {
					this.removeFromRole(role);
				});
		},
		addToRole(role) {
			return this.$request(this.$AdminApi.UserRolesAdmin.AddUserToRole, {
				userId: this.userId,
				roleName: role.name
			}).then(() => {
				this.loadUserRoles();
			});
		},
		removeFromRole(role) {
			return this.$request(this.$AdminApi.UserRolesAdmin.RemoveUserFromRole, {
				userId: this.userId,
				roleName: role.name
			}).then(() => {
				this.loadUserRoles();
			});
		},
		loadUserRoles() {
			return this.$request(this.$AdminApi.UserRolesAdmin.GetUserRoles, {
				userId: this.userId
			}).then(response => {
				this.userRoles = response.data;
				this.availableRoles = this.allRoles.filter(
					x => !this.userRoles.some(y => y.name === x.name)
				);
			});
		},
		loadAllRoles() {
			return this.$request(this.$AdminApi.UserRolesAdmin.GetAllRoles).then(
				response => {
					this.allRoles = response.data;
				}
			);
		}
	},
	async created() {
		await this.loadAllRoles();
		await this.loadUserRoles();
	}
};
</script>

<style lang="scss">
.profile-roles__user-groups .profile-roles__group:not(:last-child):after {
	content: ", ";
	color: initial;
	font-weight: initial;
}

.profile-roles__roles-block {
	background-color: #f7fbc9;
	padding: 10px;
}
</style>
