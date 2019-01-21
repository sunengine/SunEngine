import PersonalInfo from 'personal/PersonalInfo';
import SettingsMenu from 'layout/SettingsMenu';
import LoadPhoto from 'personal/LoadPhoto';
import EditUserProfileInformation from 'personal/EditUserProfileInformation';
import ChangePassword from 'auth/Password/ChangePassword.vue';
import ChangeEmail from 'auth/ChangeEmail.vue';
import ChangeLink from 'auth/ChangeLink.vue';
import ChangeName from 'auth/ChangeName.vue';
import UserProfile from 'profile/Profile.vue';

import {store} from 'store';


const routes = [
  {
    name: 'ChangePassword',
    path: '/auth/ChangePassword'.toLowerCase(),
    components: {
      default: ChangePassword,
      navigation: SettingsMenu
    }
  },
  {
    name: 'ChangeEmail',
    path: '/auth/ChangeEmail'.toLowerCase(),
    components: {
      default: ChangeEmail,
      navigation: SettingsMenu
    }
  },
  {
    name: 'ChangeLink',
    path: '/auth/ChangeLink'.toLowerCase(),
    components: {
      default: ChangeLink,
      navigation: SettingsMenu
    }
  },
  {
    name: 'ChangeName',
    path: '/auth/ChangeName'.toLowerCase(),
    components: {
      default: ChangeName,
      navigation: SettingsMenu
    }
  },
  {
    name: 'Personal',
    path: '/personal',
    components: {
      default: PersonalInfo,
      navigation: null
    }
  },
  {
    name: 'LoadPhoto',
    path: '/personal/LoadPhoto'.toLowerCase(),
    components: {
      default: LoadPhoto,
      navigation: SettingsMenu
    }
  },
  {
    name: 'EditUserProfileInformation',
    path: '/personal/EditUserProfileInformation'.toLowerCase(),
    components: {
      default: EditUserProfileInformation,
      navigation: SettingsMenu
    }
  },
  {
    path: '/personal/profile',
    components: {
      default: UserProfile,
      navigation: SettingsMenu
    },
    props: {
      default: () => {return { link: store.state.auth.user?.link }}
    }
  },
]

for (let rote of routes) {
  if (!rote.meta) {
    rote.meta = {
      roles: ["Registered"]
    };
  }
}

export default routes
