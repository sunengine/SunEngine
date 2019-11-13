import {router} from 'sun'

export default function (ell, className) {
  const el = ell.getElementsByClassName(className)[0];
  const links = el.getElementsByTagName('a');


  for (const link of links) {
    if (link.href.startsWith(config.SiteUrl)) {
      link.addEventListener('click', (e) => {
        e.preventDefault();
        const url = link.href.substring(config.SiteUrl.length);
        this.$router.push(url);
      });
    } else {
      if (config.OpenExternalLinksAtNewTab)
        link.setAttribute("target", "_blank");
    }
  }
}
