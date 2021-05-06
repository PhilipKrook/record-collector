const defaultTheme = require('tailwindcss/defaultTheme')

module.exports = {
	purge: {
		enabled: process.env.NODE_ENV === 'production',
		content: ['./src/Krompaco.RecordCollector.Web/Views/**/*.cshtml', './docs/content-record-collector-net/**/*.html']
	},
	theme: {
		extend: {
			fontFamily: {
				sans: ['LeagueSpartanVariable', ...defaultTheme.fontFamily.sans],
				mono: ['JetBrainsMono', ...defaultTheme.fontFamily.mono],
			},
			typography: {
				DEFAULT: {
					css: {
						'pre code::before': {
							content: 'none',
						},
						'pre code::after': {
							content: 'none',
						},
					},
				},
			}
		},
	},
	variants: {
		transitionProperty: ['responsive', 'motion-safe', 'motion-reduce'],
		extend: {
			outline: ['hover', 'active'],
			ringColor: ['hover', 'active'],
			ringOffsetColor: ['hover', 'active'],
			ringOffsetWidth: ['hover', 'active'],
			ringOpacity: ['hover', 'active'],
			ringWidth: ['hover', 'active'],
		},
	},
	plugins: [
		require('@tailwindcss/typography')
	],
};