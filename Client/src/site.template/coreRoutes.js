import {authRoutes} from 'sun'
import {accountRoutes} from 'sun'
import {miscRoutes} from 'sun'
import {personalRoutes} from 'sun'
import {adminRoutes} from 'sun'


export default [...authRoutes, ...accountRoutes, ...miscRoutes, ...personalRoutes, ...adminRoutes];
