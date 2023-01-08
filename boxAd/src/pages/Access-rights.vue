<template>
	<q-page>
		<div class="page-title-area bg-white">
			<div class="wrapper">
				<div class="page-title">
					원활한 검사를 위해
					<br>
					<span class="text-bold"> 접근 권한 </span>
					의 허용이 필요합니다.
				</div>
				<div class="page-title-detail">
					굿바이 서비스는 편리하고 안전한 단말기 상태 진단을 위해 아래의 접근 권한을
					요청합니다.
				</div>
			</div>
		</div>
		<div class="page-body">
			<div class="wrapper">
				<div class="page-body-inner">
					<div class="row items-center q-col-gutter-x-md right-group">
						<div class="col-auto">
							<q-avatar
								round
								color="grey-4"
							>
								<q-icon
									name="o_phone"
									color="grey-8"
								/>
							</q-avatar>
						</div>
						<div class="col">
							<div class="page-body-title">
								전화 <span class="text-primary small-title">(필수)</span>
							</div>
							<div
								class="page-body-text"
								style="margin-top: 14px"
							>
								단말기 식별번호(IMEI)로 분실, 도난 여부 확인
							</div>
						</div>
					</div>
					<div class="row items-center q-col-gutter-x-md right-group">
						<div class="col-auto">
							<q-avatar
								round
								color="grey-4"
							>
								<q-icon
									name="o_place"
									color="grey-8"
								/>
							</q-avatar>
						</div>
						<div class="col">
							<div class="page-body-title">
								위치 <span class="text-primary small-title">(필수)</span>
							</div>
							<div
								class="page-body-text"
								style="margin-top: 14px"
							>
								위치(GPS) 기능 확인
							</div>
						</div>
					</div>
					<div class="row items-center q-col-gutter-x-md right-group">
						<div class="col-auto">
							<q-avatar
								round
								color="grey-4"
							>
								<q-icon
									name="mic_none"
									color="grey-8"
								/>
							</q-avatar>
						</div>
						<div class="col">
							<div class="page-body-title">
								오디오, 녹음 <span class="text-primary small-title">(필수)</span>
							</div>
							<div
								class="page-body-text"
								style="margin-top: 14px"
							>
								스피커, 마이크 기능 확인
							</div>
						</div>
					</div>
					<div class="row items-center q-col-gutter-x-md right-group">
						<div class="col-auto">
							<q-avatar
								round
								color="grey-4"
							>
								<q-icon
									name="o_insert_photo"
									color="grey-8"
								/>
							</q-avatar>
						</div>
						<div class="col">
							<div class="page-body-title">
								사진, 동영상 <span class="text-primary small-title">(필수)</span>
							</div>
							<div
								class="page-body-text"
								style="margin-top: 14px"
							>
								전면, 후면 카메라 확인
							</div>
						</div>
					</div>
					<div class="btn-area q-mt-xl">
						<q-btn
							unelevated
							rounded
							color="primary"
							label="확인"
							class="full-width"
							@click="GetNRequestPermissions"
						/>
					</div>
				</div>
			</div>
		</div>
	</q-page>
</template>

<script>
// ----------------------------------------------------------------------------------------------
export default {
	// ----------------------------------------------------------------------------------------------
	mounted() {
		// 권한얻는 페이지 부분에서는 밝기, 화면 고정 안되는것 같음
	},

	// ----------------------------------------------------------------------------------------------
	methods: {

		// ----------------------------------------------------------------------------------------------
		// 확인과 요청을 모두 실행
		GetNRequestPermissions() {
			this.$M.evalGetPermissions(this.evalGetPermissionsResult)
		},

		// ----------------------------------------------------------------------------------------------
		// 결과값 확인
		// ----------------------------------------------------------------------------------------------
	   evalGetPermissionsResult(data) {
			let needPermissions = false // 권한 요청 필요함 여부
			this.$C.UpdatePermissions(JSON.parse(data || ''))
			const obj = this.$C.permissions

			Object.keys(obj).forEach(function(key) {
				if (obj[key] === false) { // 권한없음
					needPermissions = true
					return false
				}
			})

			// ----------------------------------------------------------------------------------------------
			if (needPermissions === false) {
				this.updateConfirm(this).then(() => { this.$router.push('kiosk') })
			} else {
				this.$M.evalRequestPermissions() //   [확인] 버튼 눌렀을때 권한 요청함
			}
		},

		// ----------------------------------------------------------------------------------------------
		updateConfirm(thisA) {
			return new Promise((resolve) => {
				thisA.$C.UpdateConfirmPage(thisA.$P.tCPStep3, thisA.$P.tCPStep3)
				resolve()
			})
		}
		// ----------------------------------------------------------------------------------------------

		// ----------------------------------------------------------------------------------------------
	}
}
</script>
