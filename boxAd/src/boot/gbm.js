// ----------------------------------------------------------------------------------------------
import { boot } from 'quasar/wrappers'
import { useCoreStatus } from 'stores/coreStatus'

import Mobile from 'src/utils/Mobile'
import Util from 'src/utils/Util'
import Protocol from 'src/utils/Protocol'

// ----------------------------------------------------------------------------------------------
export default boot(({ app }) => {
	app.config.globalProperties.$M = Mobile
	app.config.globalProperties.$U = Util
	app.config.globalProperties.$P = Protocol
	app.config.globalProperties.$C = useCoreStatus()
})

// ----------------------------------------------------------------------------------------------
