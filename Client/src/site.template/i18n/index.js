import enUs from "src/i18n/en-us";
import ru from "src/i18n/ru";
import enUsSite from "./en-us";
import ruSite from "./ru";

export default {
	"en-us": { ...enUs, ...enUsSite },
	ru: { ...ru, ...ruSite }
};
