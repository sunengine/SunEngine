/* Do not remove this component from the layout. This component is the part of
SunEngine user license -
https://github.com/sunengine/SunEngine/blob/master/LICENSE.md */

<template>
	<div class="sun-engine-footer">
		{{ $tl("start") }} &nbsp;–&nbsp;
		<a class="sun-engine-footer__link" href="https://sunengine.site">SunEngine</a>
	</div>
</template>

<script>
export default {
	name: "SunEngineFooter"
};
</script>

<style lang="scss">
.sun-engine-footer {
	text-align: center;

	a {
		color: orange;
		text-shadow: 0.5px 0.5px 0.5px #686569;

		&:hover {
			text-decoration: underline;
		}
	}

	.q-icon {
		margin: 0 16px;
	}
}
</style>
