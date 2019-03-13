import Profile from 'profile/Profile';
import AddEditMaterial from 'material/AddEditMaterial';
import SendPrivateMessage from 'profile/SendPrivateMessage';

const routes = [
  {
    name: "AddEditMaterial",
    path: '/AddEditMaterial'.toLowerCase(),
    components: {
      default: AddEditMaterial,
      navigation: null
    },
    props: {
      default: (route) => {
        return {
          categoryName: route.query.categoryName,
          id: +route.query.id
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

