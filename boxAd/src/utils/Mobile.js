// ----------------------------------------------------------------------------------------------
import Util from 'src/utils/Util'

// ----------------------------------------------------------------------------------------------
const _PROTOCOL_MOBILE_NONE = 'background:transparent'
const _PROTOCOL_MOBILE_ERROR = 'background:#c80032; padding: 1px; border-radius: 3px;  color: #fff'

// ----------------------------------------------------------------------------------------------
const _STORAGEKEY = 'dsKey' 																										// 스토리지 키값

/* const _SSIDNAME = process.env.DEV ? 'IGSGKOREA' : 'WIFI_GBKIOSK' 								//  와이파이 이름
const _SSIDPW = process.env.DEV ? 'innogswireless!' : 'gb12345678' 							//  와이파이 비밀번호
const _GBMMOBILEADDRESS = process.env.DEV ? '192.168.15.61' : '10.234.105.250' 	// 완료후 연결 주소
const _GBMMOBILEPORT = process.env.DEV ? '9001' : '7500' // 완료후 연결 포트 */

const _SSIDNAME = 'IGSGKOREA'
const _SSIDPW = 'innogswireless!'
const _GBMMOBILEADDRESS = '192.168.15.61'
const _GBMMOBILEPORT = '9000'

export default {
	// ----------------------------------------------------------------------------------------------
	name: 'Mobile',

	// ----------------------------------------------------------------------------------------------
	app: null, 					// APP.vue this 객체

	storagekey: _STORAGEKEY.toLowerCase(),
	ssidname: _SSIDNAME,
	ssidpw: _SSIDPW,
	gbmmobileaddressport: _GBMMOBILEADDRESS + ':' + _GBMMOBILEPORT,

	// ----------------------------------------------------------------------------------------------
	// Init
	// ----------------------------------------------------------------------------------------------
	Init(app) {
		// Set APP
		this.app = app
	},

	// ----------------------------------------------------------------------------------------------
	// ScreenBrightness 정보
	// ----------------------------------------------------------------------------------------------
	evalGetScreenBrightness(onGetScreenBrightness) {
		try {
			window.GBM.returnGetScreenBrightness = onGetScreenBrightness
			window.GBM.GetScreenBrightness()
		} catch (e) {
			if (process.env.DEV) {
				// ex) 1: 화면 밝기 최대
				onGetScreenBrightness('1')
			} else {
				// 값 실패
				onGetScreenBrightness('0')
			}
		}
	},

	// ----------------------------------------------------------------------------------------------
	// ScreenBrightness 세팅 (값 : 1(최대) , 0.5(반) )
	// ----------------------------------------------------------------------------------------------
	evalSetScreenBrightness(brightness) {
		try {
			window.GBM.SetScreenBrightness(brightness)
		} catch (e) {
			console.log(`%c#[${Util.GetTime()}] evalSetScreenBrightness: Failed. [${brightness}][${e}]%c`, _PROTOCOL_MOBILE_ERROR, _PROTOCOL_MOBILE_NONE)
		}
	},

	// ----------------------------------------------------------------------------------------------
	// DeviceDisplayInfo/확인
	// ----------------------------------------------------------------------------------------------
	evalGetDeviceDisplayKeepOn(onGetDeviceDisplayKeepOn) {
		try {
			window.GBM.returnGetDeviceDisplayKeepOn = onGetDeviceDisplayKeepOn
			window.GBM.GetDeviceDisplayKeepOn()
		} catch (e) {
			if (process.env.DEV) {
				// 예제값
				onGetDeviceDisplayKeepOn('true')
			} else {
				// 값 실패
				onGetDeviceDisplayKeepOn('')
			}
		}
	},

	// ----------------------------------------------------------------------------------------------
	// DeviceDisplayInfo/세팅
	// keepon : 켬 : true , 끔 : false
	// ----------------------------------------------------------------------------------------------
	evalSetDeviceDisplayKeepOn(keepon) {
		try {
			window.GBM.SetDeviceDisplayKeepOn(keepon)
		} catch (e) {
			console.log(`%c#[${Util.GetTime()}] evalSetDeviceDisplayKeepOn: Failed. [${keepon}][${e}]%c`, _PROTOCOL_MOBILE_ERROR, _PROTOCOL_MOBILE_NONE)
		}
	},

	// ----------------------------------------------------------------------------------------------
	// SSID
	// ----------------------------------------------------------------------------------------------
	 evalGetSSID(onGetSSID) {
		try {
			window.GBM.returnGetSSID = onGetSSID
			window.GBM.GetSSID()
		} catch (e) {
			if (process.env.DEV) {
				 // onGetSSID('{"result":1,"ssid":"' + this.ssidname + '"}') // 성공 예제값
				  onGetSSID('{"result":1,"ssid":"AndroidWifi"}') // 실패  예제값
				// onGetSSID('{"result":0,"ssid":""}') // 실패  예제값
			} else {
				onGetSSID('{"result":0,"ssid":""}') // 값 실패
			}
		}
	},

	// ----------------------------------------------------------------------------------------------
	// IMEI
	// ----------------------------------------------------------------------------------------------
	evalGetIMEIInfo(onGetIMEIInfo) {
		try {
			window.GBM.returnGetIMEIInfo = onGetIMEIInfo
			window.GBM.GetIMEIInfo()
		} catch (e) {
			if (process.env.DEV) {
				onGetIMEIInfo('["358240051111110","112233051111110"]')
				// onGetIMEIInfo('["358240051111110"]')
				// onGetIMEIInfo('[]')
			} else {
				onGetIMEIInfo('[]')
			}
		}
	},

	// ----------------------------------------------------------------------------------------------
	// Permission 접근 권한 설정 상태 확인
	// ----------------------------------------------------------------------------------------------
	 evalGetPermissions(onGetPermissions) {
		try {
			window.GBM.returnGetPermissions = onGetPermissions
			window.GBM.GetPermissions()
		} 		catch (e) {
			if (process.env.DEV) {
				onGetPermissions('{"PermissionPhoneState":true,"PermissionLocation":true,"PermissionRecord":true,"PermissionCamera":true}')
			} else {
				onGetPermissions('{"PermissionPhoneState":false,"PermissionLocation":false,"PermissionRecord":false,"PermissionCamera":false}')
			}
		}
	},

	// ----------------------------------------------------------------------------------------------
	// Permission 접근 권한 요청 (팝업)
	// ----------------------------------------------------------------------------------------------
	 evalRequestPermissions() {
		try {
			window.GBM.RequestPermissions()
		} catch (e) {
			 console.log(`%c#[${Util.GetTime()}] evalRequestPermissions: Failed. [${e}]%c`, _PROTOCOL_MOBILE_ERROR, _PROTOCOL_MOBILE_NONE)
		}
	},

	// ----------------------------------------------------------------------------------------------
	// Wifi 확인
	// 리턴값(string) =  맞음 : true , 틀림 : false
	// ----------------------------------------------------------------------------------------------
	/* evalRequestConnectWifi(ssidName, ssidPw, onRequestConnectWifi) {
		try {
			window.GBM.returnRequestConnectWifi = onRequestConnectWifi
			window.GBM.RequestConnectWifi(ssidName, ssidPw)
		} 		catch (e) {
			if (process.env.DEV) {
				onRequestConnectWifi('false')
			} else {
				onRequestConnectWifi('')
			}
		}
	}, */

	// ----------------------------------------------------------------------------------------------
	// storage use (읽기)
	// Parameter = dsKey : 키값
	// 리턴값(string)
	// ----------------------------------------------------------------------------------------------
	evalReadLocalStorage(dsKey, onReadLocalStorage) {
		try {
			window.GBM.returnReadLocalStorage = onReadLocalStorage
			window.GBM.ReadLocalStorage(dsKey)
		} catch (e) {
			if (process.env.DEV) {
				onReadLocalStorage(localStorage.getItem(dsKey))
			} else {
				onReadLocalStorage('')
			}
		}
	},

	// ----------------------------------------------------------------------------------------------
	// storage use (쓰기)
	// Parameter = dsKey : 키값
	// 리턴값 = 없음
	// ----------------------------------------------------------------------------------------------
	evalWriteLocalStorage(dsKey, dsVaule) {
		try {
			window.GBM.WriteLocalStorage(dsKey, dsVaule)
		} catch (e) {
			localStorage.setItem(dsKey, dsVaule)
		}
	}

	// ----------------------------------------------------------------------------------------------
}
