// --------------------------------------------------------------------------------
// Util
// --------------------------------------------------------------------------------

// --------------------------------------------------------------------------------
import Base64 from 'src/utils/Base64'

// --------------------------------------------------------------------------------
const _UTIL_LOG_NONE = 'background:transparent'
const _UTIL_LOG_ERROR = 'background:#c80032; padding: 1px; border-radius: 3px;  color: #fff'
// const _UTIL_LOG = 'background:#056dba; padding: 1px; border-radius: 3px;  color: #fff'

// --------------------------------------------------------------------------------
export const RHash = Math.floor(Math.random() * 90000) + 10000

// --------------------------------------------------------------------------------
export default {
	name: 'Util',

	// ------------------------------------------------------------------------------
	// Util functions
	// ------------------------------------------------------------------------------
	// JDecode
	JDecode(obj) {
		try {
			if (obj instanceof Array) {
				for (const att of obj) { // OF
					this.JDecode(att)
				}
				return obj
			} else {
				for (const att in obj) { // IN
					if (Object.prototype.hasOwnProperty.call(obj, att)) {
						const desc = Object.getOwnPropertyDescriptor(obj, att) || {}
						// 쓰기 가능한 프로퍼티만
						if (typeof desc.writable !== 'undefined' && desc.writable === true) {
							const aval = obj[att]
							if (typeof aval === 'string') {
								// Decode
								obj[att] = Base64.Decode(aval)
							} else if (typeof aval === 'object') {
								// Recursive
								this.JDecode(aval)
							}
						}
					}
				}
				return obj
			}
		} catch (error) {
			console.log(`%c#[${this.GetTime()}] Protocol.JDecode exception has occurred. [${error}]:[${obj}]%c`, _UTIL_LOG_ERROR, _UTIL_LOG_NONE)
			console.trace()
			return obj
		}
	},

	// ----------------------------------------------------------------------------------------------
	// JParse
	JParse(data) {
		try {
			if (typeof data !== 'string') {
				throw new Error('Invalid source data')
			}

			// 빈문자열은 오류 없이 그냥 리턴
			if (data.trim().length === 0) {
				return {}
			}

			// 기본 변환
			const obj = JSON.parse(data || '')
			if (typeof obj === 'string') {
				throw new Error('Invalid source data after parsing')
			}

			// 변환 하여 리턴
			return obj
		} catch (error) {
			console.log(`%c#[${this.GetTime()}] Protocol.JParse exception has occurred. [${error}]:[${data}]%c`, _UTIL_LOG_ERROR, _UTIL_LOG_NONE)
			console.trace()
			return {}
		}
	},

	// ----------------------------------------------------------------------------------------------
	// GetTime()
	GetTime(timeValue) {
		let time
		if (typeof timeValue === 'number') {
			time = new Date(timeValue)
		} else {
			time = new Date()
		}

		let yy = ('0000' + time.getFullYear()).toString()
		let mm = ('00' + (time.getMonth() + 1)).toString()
		let dd = ('00' + time.getDate()).toString()
		let h = ('00' + time.getHours()).toString()
		let m = ('00' + time.getMinutes()).toString()
		let s = ('00' + time.getSeconds()).toString()
		let mil = ('000000' + time.getMilliseconds()).toString()
		yy = yy.substring(yy.length - 4, yy.length)
		mm = mm.substring(mm.length - 2, mm.length)
		dd = dd.substring(dd.length - 2, dd.length)
		h = h.substring(h.length - 2, h.length)
		m = m.substring(m.length - 2, m.length)
		s = s.substring(s.length - 2, s.length)
		mil = mil.substring(mil.length - 6, mil.length)

		return `${yy}-${mm}-${dd} ${h}:${m}:${s}.${mil}`
	},

	// ----------------------------------------------------------------------------------------------
	// UUID: UUID 생성
	UUID() {
	 	function _s4() {
			return ((1 + Math.random()) * 0x10000 | 0).toString(16).substring(1) // 5자리에서 뒤 4자리
	 	}
	 	function _t4() {
			const t = ((new Date()).getTime()).toString(16)
			return t.substring(t.length - 4, t.length) // 뒤 4자리
		}
		// 3e39a312-2824-7fe4-5ddf-168818b34f68
		return _s4() + _s4() + '-' + _s4() + '-' + _s4() + '-' + _s4() + '-' + _s4() + _s4() + _t4()
		// return _s4() + _s4() + '-' + _s4() + _s4() + _t4()
	},

	// ----------------------------------------------------------------------------------------------
	// GetUUID: 저장된 UUID, 없으면 생성
	GetUUID() {
		let uuid = localStorage.getItem('B_UUID') || ''
		if (uuid.length < 1) {
			uuid = this.UUID()
			localStorage.setItem('B_UUID', uuid)
		}
		return uuid
	},

	// ------------------------------------------------------------------------------------------------
	// 부모 클래스 확인
	HasClass(element, className) {
		do {
			if (element.classList && element.classList.contains(className)) {
				if (element.disabled !== true) {
					return element
				}
			}
			element = element.parentNode
		}	while (element)
		return null
	},

	// ------------------------------------------------------------------------------------------------
	// 부모 클래스 확인
	HasTag(element, tagName) {
		tagName = (tagName || '').toUpperCase()

		do {
			if (element.tagName && element.tagName.toUpperCase() === tagName) {
				if (element.disabled !== true) {
					return element
				}
			}
			element = element.parentNode
		}	while (element)
		return null
	},

	// ----------------------------------------------------------------------------------------------
	// Format: {0} {1}...
	Format() {
		const args = Array.prototype.slice.call(arguments, 1)
		const fmt = arguments[0] || ''
		return fmt.replace(/{(\d+)}/g, (match, number) => {
			return typeof args[number] !== 'undefined' ? args[number] : match
		})
	}

	// ----------------------------------------------------------------------------------------------
}
