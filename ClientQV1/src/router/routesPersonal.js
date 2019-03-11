import SettingsPanel from 'personal/SettingsPanel';
import SettingsPage from 'personal/SettingsPage';
import LoadPhoto from 'personal/LoadPhoto';
import EditProfileInformation from 'personal/EditProfileInformation';
//import ChangePassword from 'account/Password/ChangePassword.vue';
//import ChangeEmail from 'account/ChangeEmail.vue';
import ChangeLink from 'personal/ChangeLink.vue';
import ChangeName from 'personal/ChangeName.vue';
import MyBanList from 'personal/MyBanList.vue';
import Profile from 'profile/Profile.vue';

import {store} from 'store';


const routes = [
/*  {
    name: 'ChangePassword',
    path: '/account/ChangePassword'.toLowerCase(),
    components: {
      default: ChangePassword,
      navigation: SettingsPanel
    }
  },
  {
    name: 'ChangeEmail',
    path: '/account/ChangeEmail'.toLowerCase(),
    components: {
      default: ChangeEmail,
      navigation: SettingsPanel
    }
  },*/
  {
    name: 'ChangeLink',
    path: '/personal/ChangeLink'.toLowerCase(),
    components: {
      default: ChangeLink,
      navigation: SettingsPanel
    }
  },
  {
    name: 'ChangeName',
    path: '/personal/ChangeName'.toLowerCase(),
    components: {
      default: ChangeName,
      navigation: SettingsPanel
    }
  },
  {
    name: 'Personal',
    path: '/personal',
    components: {
      default: SettingsPage,
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
    name: 'EditProfileInformation',
    path: '/personal/EditProfileInformation'.toLowerCase(),
    components: {
      default: EditProfileInformation,
      navigation: SettingsPanel
    }
  },
  {
    name: 'MyBanList',
    path: '/personal/MyBanList'.toLowerCase(),
    components: {
      default: MyBanList,
      navigation: SettingsPanel
    }
  },
  {
    name: 'ProfileInSettings',
    path: '/personal/Profile'.toLowerCase(),
    components: {
      default: Profile,
      navigation: SettingsPanel
    },
    props: {
      default: () => {return { link: store.state.auth.userInfo?.link }}
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
