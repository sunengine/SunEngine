<template>
	<div class="user-menu sun-second-menu">
		<q-item :to="{ name: 'User', params: { link: user.link } }" v-close-popup>
			<q-item-section avatar>
				<q-icon :name="$iconsSet.UserMenu.profile" />
			</q-item-section>
			<q-item-section>
				<q-item-label>
					{{ $tl("profile") }}
				</q-item-label>
			</q-item-section>
		</q-item>
		<q-item :to="{ name: 'Personal' }" v-close-popup>
			<q-item-section avatar>
				<q-icon :name="$iconsSet.UserMenu.personal" />
			</q-item-section>
			<q-item-section>
				<q-item-label :lines="1">
					{{ $tl("yourAccount") }}
				</q-item-label>
			</q-item-section>
		</q-item>
		<q-item v-if="isAdmin" :to="{ name: 'AdminInformation' }" v-close-popup>
			<q-item-section avatar>
				<q-icon :name="$iconsSet.UserMenu.admin" />
			</q-item-section>
			<q-item-section>
				<q-item-label>
					{{ $tl("adminPanel") }}
				</q-item-label>
			</q-item-section>
		</q-item>
		<q-item @click.native="logout()" clickable v-close-popup>
			<q-item-section avatar>
				<q-icon :name="$iconsSet.UserMenu.signOut"  />
			</q-item-section>
			<q-item-section>
				<q-item-label>
					{{ $tl("exit") }}
				</q-item-label>
			</q-item-section>
		</q-item>
	</div>
</template>

<script>
import { mapState } from "vuex";

export default {
	name: "UserMenu",
	computed: {
		isAdmin() {
			return this.$store.state.auth.roles.some(x => x === "Admin");
		},
		...mapState({
			user: state => state.auth.user
		})
	},
	methods: {
		logout() {
			const logoutNotify = this.$tl("logoutNotify");
			this.$store.dispatch("logout").then(() => {
				this.$q.notify({
					message: logoutNotify,
					timeout: 2000,
					color: "info",
					position: "top"
				});
			});
		}
	}
};
</script>

<style lang="scss"></style>
