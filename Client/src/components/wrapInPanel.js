import { extend } from "quasar";
import { PanelWrapper } from "sun";

export default function(name, wrapComponent, title, titleLink, icon) {
	const panelWrapper = extend(true, {}, PanelWrapper);

	panelWrapper.name = name;
	panelWrapper.wrapComponentOption = wrapComponent;
	panelWrapper.iconOption = icon;
	panelWrapper.titleOption = title;
	panelWrapper.titleLinkOption = titleLink;

	return panelWrapper;
}
