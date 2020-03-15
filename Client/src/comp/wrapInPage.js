import { PanelWrapper } from "comp";
import { PageWrapper } from "comp";
import { extend } from "quasar";

export default function wrapInPage(name, wrapComponent, title, icon) {
	const pageWrapper = extend(true, {}, PageWrapper);

	pageWrapper.name = name;
	pageWrapper.pageTitleOption = title;
	pageWrapper.wrapComponentOption = wrapComponent;
	pageWrapper.iconOption = icon;

	pageWrapper.mixins = [Page];
	pageWrapper.created = function() {
		this.title = this.pageTitle;
	};

	return pageWrapper;
}
