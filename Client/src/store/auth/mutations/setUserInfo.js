import Vue from "vue";

import { avatarPath } from "sun";

export default function(state, data) {
	data.photo = avatarPath(data.photo);
	data.avatar = avatarPath(data.avatar);

	Vue.set(state, "roles", data.roles);

	delete data.roles;

	Vue.set(state, "user", data);
}
