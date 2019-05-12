import Profile from 'profile/Profile';
import CreateMaterial from 'material/CreateMaterial';
import EditMaterial from 'material/EditMaterial';
import SendPrivateMessage from 'profile/SendPrivateMessage';

const routes = [
  {
    name: "CreateMaterial",
    path: '/CreateMaterial/'.toLowerCase()+':categoriesNames/:initialCategoryName?',
    components: {
      default: CreateMaterial,
      navigation: null
    },
    props: {
      default: true,
    },
    meta: {
      roles: ["Registered"]
    }
  },
  {
    name: "EditMaterial",
    path: '/EditMaterial/'.toLowerCase()+':id',
    components: {
      default: EditMaterial,
      navigation: null
    },
    props: {
      default: (route) => {
        return {
          id: +route.params.id
        }
      },
      navigation: null
    },
    meta: {
      roles: ["Registered"]
    }
  },
  {
    name: 'SendPrivateMessage',
    path: '/SendPrivateMessage'.toLowerCase(),
    components: {
      default: SendPrivateMessage,
      navigation: null
    },
    props: {
      default: (route) => {
        return {
          userId: route.query.userId,
          userName: route.query.userName
        }
      }
    }
  },
  {
    name: 'User',
    path: '/user/:link',
    components: {
      default: Profile,
      navigation: null
    },
    props: {
      default: true
    }
  },
];




export default routes;

