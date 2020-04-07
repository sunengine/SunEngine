import { router } from "router";

export default function(ell, className) {
	const el = ell.getElementsByClassName(className)[0];
	const links = el.getElementsByTagName("a");

	for (const link of links) {
		link.classList.add("link");
		if (link.href.startsWith(config.UrlPaths.Site)) {
			link.addEventListener("click", e => {
				e.preventDefault();
				const url = link.href.substring(config.UrlPaths.Site.length);
				router.push(url);
			});
		} else {
			if (config.Global.OpenExternalLinksAtNewTab)
				link.setAttribute("target", "_blank");
		}
	}
}
