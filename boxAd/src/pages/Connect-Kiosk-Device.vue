<template>
	<q-page>
		<div class="page-title-area bg-white">
			<div class="wrapper">
				<div class="page-title">
					굿바이 ATM과 단말기를
					<br><span class="text-bold"> 연결 </span>
					해주세요<span @click="onVersion()">.</span>
				</div>
			</div>
		</div>
		<div class="page-body">
			<div class="wrapper">
				<div class="page-body-inner">
					<div class="row items-start justify-center q-col-gutter-lg relative-position">
						<div class="col text-center">
							<q-img src="@/assets/images/kiosk.png" />
						</div>
						<div class="col text-center">
							<q-img src="@/assets/images/pad-phone.png" />
						</div>
						<q-icon
							name="multiple_stop"
							size="64px"
							class="absolute-center"
						/>
					</div>
					<div class="page-small-title text-center connect-description">
						단말기를 검사하기 위해 단말기의 WiFi 설정을 카메라를 통해서 굿바이 ATM의
						화면에 보이는 WiFi QR코드로 설정하거나 아래의 Wifi로 해주세요.
					</div>
					<div class="text-center q-mt-lg">
						<div class="page-small-title wifi-description">
							<p>
								WiFi :
								{{ $M.ssidname }}
							</p>
							<p>
								Password :
								{{ $M.ssidpw }}
							</p>
						</div>
					</div>
					<div class="btn-area q-mt-lg">
						<q-btn
							unelevated
							rounded
							color="primary"
							label="확인"
							class="full-width"
							:disable="connectComplet"
							@click="goNext()"
						/>
					</div>
				</div>
			</div>
		</div>
	</q-page>
</template>

<script>
// ----------------------------------------------------------------------------------------------
const _WAIT_SECOND = 2

// ----------------------------------------------------------------------------------------------
export default {
	// ----------------------------------------------------------------------------------------------
	data() {
		return {
			connectComplet: true // 연결 확인 버튼 비활성화 여부
		}
	},

	// ----------------------------------------------------------------------------------------------
	mounted() {
		this.$M.evalSetScreenBrightness(1)
		this.$M.evalSetDeviceDisplayKeepOn(true)

		// ----------------------------------------------------------------------------------------------
		this.timerCount = _WAIT_SECOND
		if (!this.timerID) {
			this.timerID = setInterval(() => {
				this.timerCount--
				if (this.timerCount < 0) {
					this.$M.evalGetSSID(this.evalGetSSIDResult) // 기존 연결된 Wifi의 ssid가 올바른지 확인
				}
				if (this.timerCount <= -1000) {
					clearInterval(this.timerID)
				}
			}, 1000)
		} else {
			clearInterval(this.timerID)
		}
	},

	// ----------------------------------------------------------------------------------------------
	methods: {
		onVersion() {
			alert('version :' + this.$C.version)
		},
		// ----------------------------------------------------------------------------------------------
		goNext() {
			if (this.timerID !== null) {
				clearInterval(this.timerID)
			}
			this.updateConfirm(this).then(() => { window.location.href = 'http://' + this.$M.gbmmobileaddressport })
		},

		updateConfirm(thisA) {
			return new Promise((resolve) => {
				this.$C.UpdateConfirmPage(this.$P.tCPStep4, this.$P.tCPStep4)
				resolve()
			})
		},

		// ----------------------------------------------------------------------------------------------
		// Wifi 명칭 확인
		evalGetSSIDResult(data) {
			let getResult = 0	// 결과 성공 여부
			let getSsid = ''	// 결과 값
			this.$C.UpdateSSID(JSON.parse(data || ''))
			const obj = this.$C.ssid

			Object.keys(obj).forEach(key => {
				if (key === 'result') { getResult = obj[key] }
				if (key === 'ssid') { getSsid = obj[key] }
			})

			// ssidName이 같은지 확인해서 맞으면 숨김취소(false)
			if (getResult === 1 && getSsid === this.$M.ssidname) {
				this.connectComplet = false
			} else {
				this.connectComplet = true
			}
		}
	}

// ----------------------------------------------------------------------------------------------
}
</script>

<style lang="scss" scoped>
.q-img {
width: 100%;
max-width: 326px;
}

.page-small-title.wifi-description {
font-family: NotoSansKR;
margin: 0 auto;
background: rgba(123, 97, 255, 0.2);
border-radius: 6px;
padding: 24px;
display: inline-block;
}

.connect-description {
margin-top: 40px;
}

@media (max-width: $breakpoint-xs-max) {
}
</style>
