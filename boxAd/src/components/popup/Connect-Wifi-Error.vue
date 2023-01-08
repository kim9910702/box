<template>
	<div class="popup">
		<!-- wifi 값이 없을때 -->
		<q-card
			v-if="popupPageType === 'wifiNone'"
			class="text-white bg-dark"
		>
			<q-card-section
				class="pop-contents q-px-xl text-center"
			>
				WiFi와 연결되지 않았습니다.
				<br>
				WiFi를 확인해 주세요.
			</q-card-section>
			<q-card-actions
				align="right"
			>
				<q-btn
					flat
					label="닫기"
					color="primary"
					@click="cancel"
				/>
			</q-card-actions>
		</q-card>

		<!-- wifi 값이 잘못되었을때 -->
		<q-card
			v-else
			class="text-white bg-dark"
		>
			<q-card-section
				class="pop-contents q-px-xl text-center"
			>
				굿바이 ATM에서 제공하는
				<br>
				WiFi와 연결되지 않았습니다.
			</q-card-section>
			<q-card-actions
				align="right"
			>
				<q-btn
					flat
					label="닫기"
					color="primary"
					@click="cancel"
				/>
			</q-card-actions>
		</q-card>
	</div>
</template>

<script>
export default {
	// ----------------------------------------------------------------------------------------------
	props: {
		popupPageType: {
			type: String,
			default: ''
		},
		cancel: {
			type: Function,
			required: true
		}

	}

}
</script>

<style lang="scss" scoped>
.popup {
	background: #000;
}

.text-h5 {
	text-decoration: underline;
	font-size: 26px;
}

.pop-contents {
	font-family: NotoSansKR;
	font-size: 18px;
	line-height: 1.4em;
}

.q-btn {
	font-size: 14px;
}
</style>
