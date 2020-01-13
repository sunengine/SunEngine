<template>
	<span class="loader-sent"
		><q-spinner slot="loading" size="20px" class="q-mr-xs" />
		<slot>
			{{ $tl("sending") }}
		</slot>
	</span>
</template>

<script>
export default {
	name: "LoaderSent"
};
</script>

<style lang="scss"></style>
