import Vue from "vue";

export default function isJson(str) {
	try {
		JSON.parse(str);
	} catch {
		return false;
	}
	return true;
}

export function jsonRules(allowNull = false) {
	if (allowNull)
		return [
			value =>
				!value ||
				isJson(value) ||
				Vue.prototype.i18n.t("Global.validation.jsonFormatError")
		];
	else
		return [
			value =>
				isJson(value) || Vue.prototype.i18n.t("Global.validation.jsonFormatError")
		];
}
