<template>
	<div
		class="fullscreen q-pa-md flex flex-center"
		@click="goNext"
	>
		<div>
			112233
			<q-img
				src="@/assets/images/logo-gbm.svg"
				width="200px"
			/>
			112233
		</div>
	</div>
</template>
<script>
// ----------------------------------------------------------------------------------------------
const _WAIT_SECOND = 1
// ----------------------------------------------------------------------------------------------
export default {
	// ----------------------------------------------------------------------------------------------
	mounted() {
		this.$M.evalSetScreenBrightness(1)
		this.$M.evalSetDeviceDisplayKeepOn(true)
		// this.$M.evalGetIMEIInfo(this.evalGetIMEIInfoResult) // IMEIInfo 웹뷰 정보 호출시 핸드폰 정보가 표시되기 때문에 주석으로 처리
	},

	// ----------------------------------------------------------------------------------------------
	beforeUnmount() {
		if (this.timerID !== null) {
			clearInterval(this.timerID)
		}
	},

	// ----------------------------------------------------------------------------------------------
	methods: {
		goNext() {
			this.$C.UpdateConfirmPage(this.$P.tCPStep1, this.$P.tCPStep1)
		},

		// --------------------------------------------------------------------------------------------
		//  IMEI 읽기 결과값
		evalGetIMEIInfoResult(data) {
			this.$C.UpdateIMEI(data || '')
			this.setTimer()
		},

		// --------------------------------------------------------------------------------------------
		//
		setTimer() {
			this.timerCount = _WAIT_SECOND

			if (!this.timerID) {
				this.timerID = setInterval(() => {
					this.timerCount--
					if (this.timerCount < 0) {
						this.$C.UpdateConfirmPage(this.$P.tCPStep1, this.$P.tCPStep1)
					 clearInterval(this.timerID)
					}
				}, 1000)
			} else {
				clearInterval(this.timerID)
			}
		}
	}

	// ----------------------------------------------------------------------------------------------
}
// ----------------------------------------------------------------------------------------------
</script>

<style lang="scss" scoped></style>
