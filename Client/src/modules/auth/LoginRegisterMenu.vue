<template>
	<q-list class="login-register-menu sun-second-menu q-py-sm">
		<q-item :to="{ name: 'Login', params: { ret: ret } }">
			<q-item-section avatar>
				<q-icon  :name="$iconsSet.LoginRegisterMenu.signIn"  />
			</q-item-section>
			<q-item-section>
				<q-item-label>{{ $tl("enter") }}</q-item-label>
			</q-item-section>
		</q-item>
		<q-item :to="{ name: 'Register' }">
			<q-item-section avatar>
				<q-icon  :name="$iconsSet.LoginRegisterMenu.register" />
			</q-item-section>
			<q-item-section>
				<q-item-label>{{ $tl("register") }}</q-item-label>
			</q-item-section>
		</q-item>
	</q-list>
</template>

<script>
export default {
	name: "LoginRegisterMenu",
	computed: {
		ret() {
			return this.$route.path;
		}
	}
};
</script>

<style lang="scss"></style>
