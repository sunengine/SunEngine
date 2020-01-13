export default function(ell, className) {
	const el = ell.getElementsByClassName(className)[0];
	const links = el.getElementsByTagName("a");

	for (const link of links) {
		link.classList.add("link");
		if (link.href.startsWith(config.Global.SiteUrl)) {
			link.addEventListener("click", e => {
				e.preventDefault();
				const url = link.href.substring(config.Global.SiteUrl.length);
				this.$router.push(url);
			});
		} else {
			if (config.Global.OpenExternalLinksAtNewTab)
				link.setAttribute("target", "_blank");
		}
	}
}
