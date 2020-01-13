<template>
	<nav>
		<span class="links-menu">
			<template
				v-if="menuItem.subMenuItems"
				v-for="(subItem, index) of menuItem.subMenuItems"
			>
				<router-link :class="classes" v-if="subItem.to" :to="subItem.to">{{
					subItem.title
				}}</router-link>
				<a
					:class="classes"
					:href="subItem.externalUrl"
					target="_blank"
					v-else-if="subItem.externalUrl"
					>{{ subItem.title }}</a
				>
				<span v-else>{{ subItem.title }}</span>
				<span class="links-menu__separator mi1">
					<slot v-if="index !== menuItem.subMenuItems.length - 1"> </slot>
				</span>
			</template>
		</span>
	</nav>
</template>

<script>
export default {
	name: "LinksMenu",
	props: {
		menuItem: {
			type: Object,
			required: true
		},
		linkClasses: {
			type: String,
			required: false
		}
	},
	computed: {
		classes() {
			let rez = "links-menu__link";
			if (this.linkClasses) return (rez += " " + this.linkClasses);
			return rez;
		}
	},
	methods: {
		removeEdgeDelimeters() {
			const spans = this.$el.childNodes;

			spans.forEach(span => {
				if (span.classList.contains("hidden")) span.classList.remove("hidden");

				if (span.classList.contains("mbr")) span.remove();
			});

			spans.forEach((span, i) => {
				if (!span.classList.contains("links-menu__separator")) return;

				const currentSpanL = span.getBoundingClientRect().left;
				const currentSpanR = span.getBoundingClientRect().right;

				if (!spans[i + 1] || !spans[i - 1]) return;

				if (
					currentSpanL > spans[i + 1].getBoundingClientRect().left ||
					currentSpanR < spans[i - 1].getBoundingClientRect().right
				) {
					span.classList.add("hidden");
					const br = document.createElement("br");
					br.classList.add("mbr");
					span.parentNode.insertBefore(br, span.nextSibling);
				}
			});
		}
	},
	mounted() {
		this.removeEdgeDelimeters = this.$throttle(this.removeEdgeDelimeters, 200);
		this.removeEdgeDelimeters();
		window.addEventListener("resize", this.removeEdgeDelimeters, false);
	},
	beforeDestroy() {
		window.removeEventListener("resize", this.removeEdgeDelimeters);
	}
};
</script>

<style lang="scss">
.links-menu {
	a:hover {
		text-decoration: underline;
	}
}
</style>
