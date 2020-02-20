import Vue from "vue";

export default function(state, section) {
	
	if (!state.sectionsTypes) state.sectionsTypes = {};

	Vue.set(state.sectionsTypes, section.name.toLowerCase(), section);
}
