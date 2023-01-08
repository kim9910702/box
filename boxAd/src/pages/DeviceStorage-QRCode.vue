<template>
	<q-page>
		<div class="page-title-area bg-white">
			<div class="wrapper">
				<div class="page-title">
					장비 확인을 위해 <span class="text-bold"> QR Code </span> 를 스캔해주세요.
				</div>
				<!-- <div class="page-title-detail">QR Code 번호는 고유정보 입니다.</div> -->
			</div>
		</div>
		<div class="page-body">
			<div
				class="wrapper row justify-center"
			>
				<canvas
					id="qr-code"
				/>
			</div>
			<div
				class="wrapper"
			>
				<div
					class="btn-area q-mt-lg"
				>
					<q-btn
						unelevated
						rounded
						color="primary"
						label="다시 시작"
						class="full-width"
						@click="goReStart()"
					/>
				</div>
				<!-- <br> -->

				<div
					v-for="item in barcodeItems"
					:key="item"
				>
					<BarcodeGenerator
						id="bar-code"
						:format="'CODE39'"
						:line-color="'#000'"
						:element-tag="'img'"
						:value="item"
						class="full-width"
					/>
				</div>
			</div>
		</div>
	</q-page>
</template>
<script>
// ----------------------------------------------------------------------------------------------
import QRious from 'qrious'
import BarcodeGenerator from 'components/BarcodeGenerator.vue'

// ----------------------------------------------------------------------------------------------
export default {
	// ----------------------------------------------------------------------------------------------
	components: {
		BarcodeGenerator
	},
	data() {
		return {
			qrTemp: '',
			barcodeItems: this.$C.imei || this.generateBarCode()
		}
	},

	beforeMount() {
		 this.beforemount()
	},

	// ----------------------------------------------------------------------------------------------
	mounted() {
		this.init()
	},

	// ----------------------------------------------------------------------------------------------
	methods: {
		// ----------------------------------------------------------------------------------------------
		beforemount() {
			// 처음 들오고 && 스토리지 결과 값이 있으면
			if (this.$C.device_storage_dskey.length > 0) {
				this.generateQrCode(this.$C.device_storage_dskey)
			} else {
				 this.$M.evalReadLocalStorage(this.$M.storagekey, this.evalReadLocalStorageResult)// 스토리지 읽기
			}
		},

		// ----------------------------------------------------------------------------------------------
		//
		init() {
			if ((this.$C.device_storage_dskey || '') !== '') {
				this.generateQrCode(this.$C.device_storage_dskey)
			} else {
				this.goReStart()
			}
		},

		// ----------------------------------------------------------------------------------------------
		// QR Code 생성
		generateQrCode(data) {
			const canvas = document.getElementById('qr-code')
			this.qrTemp = new QRious({
				level: 'H',
				padding: 40,
				size: 320,
				element: canvas,
				value: data || this.$C.device_storage_dskey
			})
		},

		// ----------------------------------------------------------------------------------------------
		// Bar Code 생성위해  IMEI 호출
		generateBarCode(data) {
			if ((data || '') === '') {
				this.$M.evalGetIMEIInfo(this.evalGetIMEIInfoResult) // IMEI 읽기
			}
		},

		// ----------------------------------------------------------------------------------------------
		// 스토리지  읽기 결과값
		evalReadLocalStorageResult(data) {
			if ((data || '') !== '') {
				this.$C.UpdateDeviceStorage(data) // 스토리지 정보 등록
				if (this.$C.device_storage_dskey.length > 0) {
					this.generateQrCode(this.$C.device_storage_dskey)
					this.init()
				} else {
					this.$C.UpdateConfirmPage(this.$P.tCPStep2, this.$P.tCPStep2)
				}
			}
		},

		// --------------------------------------------------------------------------------------------
		//  IMEI 읽기 결과값
		evalGetIMEIInfoResult(data) {
			this.$C.UpdateIMEI(data || '')
		},

		goReStart() {
			this.updateConfirm(this).then(() => {
				this.$router.push('access')
			})
		},

		updateConfirm(thisA) {
			return new Promise((resolve) => {
				this.$M.evalWriteLocalStorage(this.$M.storagekey, '') // 스토리지 데이터를 삭제
		  	this.$C.UpdateDeviceStorage('') // 스토리지 정보 등록
		  	this.$C.UpdateConfirmPage(this.$P.tCPStep1, this.$P.tCPStep1)
		  	this.$C.UpdateConfirmPage(this.$P.tCPStep2, this.$P.tCPStep2)
				resolve()
			})
		}

		// ---------------------------------------------------------------------------------
	}
}
</script>
