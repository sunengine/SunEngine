import SettingsPanel from 'personal/SettingsPanel';
import Settings from 'personal/Settings';
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
      navigation: SettingsPanel
    }
  },
  {
    name: 'ChangeEmail',
    path: '/auth/ChangeEmail'.toLowerCase(),
    components: {
      default: ChangeEmail,
      navigation: SettingsPanel
    }
  },
  {
    name: 'ChangeLink',
    path: '/auth/ChangeLink'.toLowerCase(),
    components: {
      default: ChangeLink,
      navigation: SettingsPanel
    }
  },
  {
    name: 'ChangeName',
    path: '/auth/ChangeName'.toLowerCase(),
    components: {
      default: ChangeName,
      navigation: SettingsPanel
    }
  },
  {
    name: 'Personal',
    path: '/personal',
    components: {
      default: Settings,
      navigation: null
    }
  },
  {
    name: 'LoadPhoto',
    path: '/personal/LoadPhoto'.toLowerCase(),
    components: {
      default: LoadPhoto,
      navigation: SettingsPanel
    }
  },
  {
    name: 'EditUserProfileInformation',
    path: '/personal/EditUserProfileInformation'.toLowerCase(),
    components: {
      default: EditUserProfileInformation,
      navigation: SettingsPanel
    }
  },
  {
    path: '/personal/profile',
    components: {
      default: UserProfile,
      navigation: SettingsPanel
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
