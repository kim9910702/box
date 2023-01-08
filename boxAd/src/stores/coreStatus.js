import { defineStore } from 'pinia'
import { version } from '../../version.json'
import Util from 'src/utils/Util'

// --------------------------------------------------------------------------------------------
const _CORESTATUS_LOG_NONE = 'background:transparent'
const _CORESTATUS_LOG = 'background:#bf00bf; padding: 1px; border-radius: 3px;  color: #fff'

// --------------------------------------------------------------------------------------------
export const useCoreStatus = defineStore('coreStatus', {
	// --------------------------------------------------------------------------------------------
	state: () => ({
		version, 							// 빌드 버전
		confirm_page: [], 		// 확인을 해야 되는 페이지 저장 값
		bright_data: '', 			// 밝기 값
		display_keep: '', 		// 화면 유지 상태
		permissions: {}, 			// 접근 권한
		ssid: {}, 						// ssid
		imei: [], 						// IMEI
		device_storage_dskey: '' 	// 디바이스 저장소
	}),

	// --------------------------------------------------------------------------------------------
	actions: {
		// --------------------------------------------------------------------------------------------
		// 확인을 해야 되는 페이지 저장 값
		UpdateConfirmPage(key, data) {
			key = key || ''
			data = data || null
			this.confirm_page[key] = data
			console.log(`%c#[${Util.GetTime()}] UpdateConfirmPage: [${key}][${data}]%c`, _CORESTATUS_LOG, _CORESTATUS_LOG_NONE, this.confirm_page)
		},

		// --------------------------------------------------------------------------------------------
		//  확인을 해야 되는 페이지 모두 취소
		ClearConfirmPage() {
			this.confirm_page = []
			console.log(`%c#[${Util.GetTime()}] ClearConfirmPage.%c`, _CORESTATUS_LOG, _CORESTATUS_LOG_NONE, this.confirm_page)
		},

		// --------------------------------------------------------------------------------------------
		// 화면 밝기
		UpdateBrightData(data) {
			data = data || ''
			if (this.bright_data !== data) {
				console.log(`%c#[${Util.GetTime()}] UpdateBrightData: [${data}]%c`, _CORESTATUS_LOG, _CORESTATUS_LOG_NONE)
			}
			this.bright_data = data
		},

		// --------------------------------------------------------------------------------------------
		// 화면 유지 상태
		UpdateDisplayKeep(data) {
			data = data || ''
			if (this.display_keep !== data) {
				console.log(`%c#[${Util.GetTime()}] UpdateDisplayKeep: [${data}]%c`, _CORESTATUS_LOG, _CORESTATUS_LOG_NONE)
			}
			this.display_keep = data
		},

		// --------------------------------------------------------------------------------------------
		// 접근 권한 정보
		UpdatePermissions(data) {
			data = data || {}
			if (this.permissions !== data) {
				console.log(`%c#[${Util.GetTime()}] UpdatePermissions: [${data}]%c`, _CORESTATUS_LOG, _CORESTATUS_LOG_NONE)
			}
			this.permissions = data
		},

		// --------------------------------------------------------------------------------------------
		//  ssid 정보
		UpdateSSID(data) {
			data = data || {}
			if (this.ssid !== data) {
				console.log(`%c#[${Util.GetTime()}] UpdateSSID: [${data}]%c`, _CORESTATUS_LOG, _CORESTATUS_LOG_NONE)
			}
			this.ssid = data
		},

		// --------------------------------------------------------------------------------------------
		//  IMEI 정보
		UpdateIMEI(data) {
			// data = data || ''
			data = (JSON.parse(data).length === 0 ? '[]' : JSON.parse(data)) || []
			if (this.imei !== data) {
				console.log(`%c#[${Util.GetTime()}] UpdateIMEI: [${data}]%c`, _CORESTATUS_LOG, _CORESTATUS_LOG_NONE)
			}
			this.imei = data
		},

		// --------------------------------------------------------------------------------------------
		// 스토리지값
		UpdateDeviceStorage(data) {
			data = data || ''
			if (this.device_storage_dskey !== data) {
				console.log(`%c#[${Util.GetTime()}] UpdateDeviceStorage: [${data}]%c`, _CORESTATUS_LOG, _CORESTATUS_LOG_NONE)
			}
			this.device_storage_dskey = data
		}

		// --------------------------------------------------------------------------------------------
	}

})
