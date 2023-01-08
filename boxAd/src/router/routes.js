
const routes = [
	{
		path: '/',
		component: () => import('layouts/MainLayout.vue'),
		children: [
			{
				name: 'step1',
				path: '/',
				component: () => import('pages/IndexPage.vue')
			},
			{
				name: 'step2',
				path: '/code',
				component: () => import('pages/DeviceStorage-QRCode.vue')
			},
			{
				name: 'step3',
				path: '/access',
				component: () => import('pages/Access-rights.vue')
			},
			{
				name: 'step4',
				path: '/kiosk',
				component: () => import('pages/Connect-Kiosk-Device.vue')
			}
		]
	},

	// Always leave this as last one,
	// but you can also remove it
	{
		path: '/:catchAll(.*)*',
		component: () => import('pages/ErrorNotFound.vue')
	}
]

export default routes
