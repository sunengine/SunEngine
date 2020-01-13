import Vue from "vue";

export default function(state, layout) {
	if (!state.all) state.all = {};
	Vue.set(state.all, layout.name.toLowerCase(), layout);
}
