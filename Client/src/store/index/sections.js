import sectionsState from "store/sections/sectionsState";
import loadAllSections from "store/sections/actions/loadAllSections";
import makeRoutesFromSections from "store/sections/actions/makeRoutesFromSections";
import registerAllSectionsTypes from "store/sections/actions/registerAllSectionsTypes";
import getSection from "store/sections/getters/getSection";
import getSectionType from "store/sections/getters/getSectionType";
import registerSectionType from "store/sections/mutations/registerSectionType";

export default {
	state: sectionsState,
	actions: {
		loadAllSections,
		makeRoutesFromSections,
		registerAllSectionsTypes
	},
	getters: {
		getSection,
		getSectionType
	},
	mutations: {
		registerSectionType
	}
};
