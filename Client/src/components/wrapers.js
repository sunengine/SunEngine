import {PanelWrapper} from 'sun'
import {PageWrapper} from 'sun'
import {Page} from 'mixins'
import {extend} from 'quasar'


export function wrapInPanel(name, wrapComponent, title, titleLink, icon) {
  const panelWrapper = extend(true, {}, PanelWrapper);

  panelWrapper.name = name;
  panelWrapper.wrapComponentOption = wrapComponent;
  panelWrapper.iconOption = icon;
  panelWrapper.titleOption = title;
  panelWrapper.titleLinkOption = titleLink;

  return panelWrapper;
}

export function wrapInPage(name, wrapComponent, title, icon) {
  const pageWrapper = extend(true, {}, PageWrapper);

  pageWrapper.name = name;
  pageWrapper.pageTitleOption = title;
  pageWrapper.wrapComponentOption = wrapComponent;
  pageWrapper.iconOption = icon;

  pageWrapper.mixins = [Page];
  pageWrapper.created = function () {
    this.title = this.pageTitle;
  };

  return pageWrapper;
}
