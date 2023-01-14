<template>
	<router-view />
</template>

<script>
// ----------------------------------------------------------------------------------------------
import { defineComponent } from 'vue'

// ----------------------------------------------------------------------------------------------
export default defineComponent({
// ----------------------------------------------------------------------------------------------
	name: 'App',

	data() {
		return {
			readLocalStorageData: '', // 스토리지 정보
			needPermissionsData: false, // 권한 요청 필요 여부  ( ture: 필요함. false: 필요없음.)
			getSSIDData: ''
		}
	},
	// ----------------------------------------------------------------------------------------------
	mounted() {
		// 외부 js 링크 : 아이폰에서는 invokeHelper.js 통해 사용가능한 기능이 몇가지 있음 ex) 권한, WIFI, IMEI
		const scriptElement = document.createElement('script')
		scriptElement.setAttribute('src', '/js/invokeHelper.js')
		document.head.appendChild(scriptElement)

		this.$M.Init(this)

		// 전역 객체 색성
		this.initGlobal()

		// ----------------------------------------------------------------------------------------------
		// this.initPageHandle()

		// ----------------------------------------------------------------------------------------------
	},

	// ----------------------------------------------------------------------------------------------
	methods: {
		// ----------------------------------------------------------------------------------------------
		// 전역 객체 색성
		initGlobal() {
			// ----------------------------------------------------------------------------------------------
			// Set global APP: 윈도우 객체를 통해 메인 app를 억세스 하기
			const self = this
			Object.defineProperty(Window.prototype, '$APP', {
				get() {
					return self
				},
				configurable: true
			})
		},

		// ----------------------------------------------------------------------------------------------
		// 상태에 따른 페이지 핸들링
		initPageHandle() {
			this.timerID = setInterval(() => {
				this.checkPages()
			}, 2000)
			this.checkPages() // 바로 변경 시도
		},

		// ----------------------------------------------------------------------------------------------
		// 상태에 따른 페이지 핸들링
		checkPages() {
			// #1: 로고 화면
			if (this.goPath(this.$P.tCPStep1)) {
				return
			}

			// ----------------------------------------------------------------------------------------------
			this.readLocalStorage(this).then((data) => {
				// #2: QR Code
				// dskey 값이 있으면 QR Code 페이지로 이동
				if ((data || '') !== '') {
					this.$C.UpdateConfirmPage(this.$P.tCPStep2, null)
				} else {
					this.$C.UpdateConfirmPage(this.$P.tCPStep2, this.$P.tCPStep2)
				}
				if (this.goPath(this.$P.tCPStep2)) {
					return false
				}

				// ----------------------------------------------------------------------------------------------
				// #3: 접근 권한 설정
				this.getPermissions(this).then((data) => {
					// 접근 권한 설정이 필요 있으면 접근 권한 페이지로 이동
					if (data === true) {
						this.$C.UpdateConfirmPage(this.$P.tCPStep3, null)
					} else {
						this.$C.UpdateConfirmPage(this.$P.tCPStep3, this.$P.tCPStep3)
					}

					if (this.goPath(this.$P.tCPStep3)) {
						return false
					}

					// ----------------------------------------------------------------------------------------------
					// #4: Wifi 연결
					this.getSSID(this).then((data) => {
						// ssid 값이 없으면 kiosk 페이지 에서 멈춤
						if ((data || '') === '') {
							this.$C.UpdateConfirmPage(this.$P.tCPStep4, null)
						} else {
							// ssid 값이 저장된 값과 같으면 다음페이지로 이동
							if (data === this.$M.ssidname) {
								this.$C.UpdateConfirmPage(this.$P.tCPStep4, this.$P.tCPStep4)
								window.location.href = 'http://' + this.$M.gbmmobileaddressport
							} else {
								this.$C.UpdateConfirmPage(this.$P.tCPStep4, null)
							}
						}

						if (this.goPath(this.$P.tCPStep4)) {
							return false
						}
					})
				})
			})

			// #5: 모바일웹으로 이동
		},

		// --------------------------------------------------------------------------------------------
		// 이동 및 체크
		goPath(routeName) {
			const cp = this.$C.confirm_page[routeName] || null // 처리가 완료됨을 의미, 들어가면 안됨
			if (cp === null) {
				if (this.$route.name !== undefined) {
					if (this.$route.name.indexOf(routeName) < 0) {
						this.$router.push({ name: routeName })
					}
				}
				return true
			}
			return false
		},

		// ----------------------------------------------------------------------------------------------
		// 스토리지에 dskey 값이 있는지 확인하고 핸들러 처리
		readLocalStorage(thisA) {
			return new Promise((resolve) => {
				thisA.$M.evalReadLocalStorage(thisA.$M.storagekey, thisA.evalReadLocalStorageResult)// 스토리지 읽기

				resolve(this.readLocalStorageData)
			})
		},
		// ----------------------------------------------------------------------------------------------
		// 스토리지  읽기 결과값
		// 디바이스 스토리지에 dskey 값이 있으면 QR Code, 없으면 step3로 이동
		evalReadLocalStorageResult(data) {
			// ----------------------------------------------------------------------------------------------
			// 페이지 핸들링 스토리 1: 처음 등록해서 ds가 없음  -> qr 안나옴, 자동 핸들링 시작
			// 페이지 핸들링 스토리 2: 단말기 수집 끝남(ds값 등록됨) ->  qr 나옴, 자동 핸들링 qr페이지에서 멈춤
			// 페이지 핸들링 스토리 3: qr페이지 "다시시작" -> qr 안나옴, 자동 핸들링 시작
			if ((data || '') !== '') {
				this.$C.UpdateDeviceStorage(data) // 스토리지 정보 등록
			}
			this.readLocalStorageData = data
		},

		// ----------------------------------------------------------------------------------------------
		// 접근 권한 설정 요청
		getPermissions(thisA) {
			return new Promise((resolve) => {
				thisA.$M.evalGetPermissions(thisA.evalGetPermissionsResult)//  접근 권한 설정 요청
				resolve(this.needPermissionsData)
			})
		},

		// ----------------------------------------------------------------------------------------------
		//  접근 권한 설정 결과
		evalGetPermissionsResult(data) {
			let temp = false
			this.$C.UpdatePermissions(JSON.parse(data || ''))
			const obj = this.$C.permissions

			Object.keys(obj).forEach(function(key) {
				if (obj[key] === false) {	// 권한없음
					temp = true
					return false
				}
			})

			this.needPermissionsData = temp
		},

		// ----------------------------------------------------------------------------------------------
		// 연결된 Wifi의 ssid 정보 호출
		getSSID(thisA) {
			return new Promise((resolve) => {
				thisA.$M.evalGetSSID(thisA.evalGetSSIDResult) // 연결된 Wifi의 ssid 정보 호출
				resolve(this.getSSIDData)
			})
		},

		// ----------------------------------------------------------------------------------------------
		//  기존 연결된 Wifi의 ssid가 올바른지 확인
		evalGetSSIDResult(data) {
			let getResult = 0	// 결과 성공 여부
			let getSsid = ''	// 결과 값
			this.$C.UpdateSSID(JSON.parse(data || ''))
			const obj = this.$C.ssid
			Object.keys(obj).forEach(function(key) {
				if (key === 'result') { getResult = obj[key] }
				if (key === 'ssid') { getSsid = obj[key] }
			})

			// 페이지 핸들링
			if (getResult === 1) {
				this.getSSIDData = getSsid
			}
		}

		// ----------------------------------------------------------------------------------------------
	}

// ----------------------------------------------------------------------------------------------
})

// ------------------------------------------------------------------------------
</script>
<style lang="scss">
.b1r{
border:solid 1px red
}
</style>
