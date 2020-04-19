import Vue from "vue";

export function passwordRules() {
	const rez = [
		value =>
			!!value || Vue.prototype.i18n.t("Global.validation.password.required"),
		value =>
			value.length >= config.PasswordValidation.RequiredLength ||
			Vue.prototype.i18n.t("Global.validation.password.requiredLength", {
				requiredLength: config.PasswordValidation.RequiredLength
			})
	];
debugger;
	if (config.PasswordValidation.RequiredUniqueChars)
		rez.push(
			value =>
				[...new Set(value.split(""))].length >=
					config.PasswordValidation.RequiredUniqueChars ||
				Vue.prototype.i18n.t("Global.validation.password.requiredUniqueChars", {
					requiredUniqueChars: config.PasswordValidation.RequiredUniqueChars
				})
		);
	
	if (config.PasswordValidation.RequireDigit)
		rez.push(
			value =>
				/\d/.test(value) ||
				Vue.prototype.i18n.t("Global.validation.password.requireDigit")
		);

	if (config.PasswordValidation.RequireLowercase)
		rez.push(
			value =>
				/[a-z]/.test(value) ||
				Vue.prototype.i18n.t("Global.validation.password.requireLowercase")
		);

	if (config.PasswordValidation.RequireUppercase)
		rez.push(
			value =>
				/[A-Z]/.test(value) ||
				Vue.prototype.i18n.t("Global.validation.password.requireUppercase")
		);

	if (config.PasswordValidation.RequireNonAlphanumeric)
		rez.push(
			value =>
				/[^a-zA-Z0-9]/.test(value) ||
				Vue.prototype.i18n.t("Global.validation.password.requireNonAlphanumeric")
		);

	return rez;
}

export function userNameRules() {
	return [
		value =>
			!!value || Vue.prototype.i18n.t("Global.validation.userName.required"),
		value =>
			value.length >= 3 ||
			Vue.prototype.i18n.t("Global.validation.userName.minLength", {
				minLength: 3
			}),
		value =>
			value.length <= config.DbColumnSizes.Users_UserName ||
			Vue.prototype.i18n.t("Global.validation.userName.maxLength", {
				maxLength: config.DbColumnSizes.Users_UserName
			}),
		value =>
			new RegExp("^[" + config.Register.AllowedUserNameCharacters + "]+$").test(
				value
			) ||
			Vue.prototype.i18n.t(
				"Global.validation.userName.allowedUserNameCharacters",
				{
					allowedUserNameCharacters: config.Register.AllowedUserNameCharacters
				}
			),
		value =>
			!this.userNameInDb ||
			Vue.prototype.i18n.t("Global.validation.userName.nameInDb")
	];
}

export function emailRules() {
	return [
		value => !!value || Vue.prototype.i18n.t("Global.validation.email.required"),
		value =>
			/.+@.+/.test(value) ||
			Vue.prototype.i18n.t("Global.validation.email.emailSig"),
		value =>
			value.length <= config.DbColumnSizes.Users_Email ||
			Vue.prototype.i18n.t("Global.validation.email.maxLength", {
				maxLength: config.DbColumnSizes.Users_Email
			})
	];
}
