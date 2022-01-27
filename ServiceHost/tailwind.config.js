const colors = require('tailwindcss/colors');
const defaultTheme = require('tailwindcss/defaultTheme');

module.exports = {
    content: ["./**/*.{html,js}"],
    theme: {
        extend: {
            colors: {
                teal: colors.teal,
                cyan: colors.cyan,
                sky: colors.sky,
                transparent: 'transparent',
                current: 'currentColor',
                black: colors.black,
                white: colors.white,
                gray: colors.slate,
                green: colors.emerald,
                purple: colors.violet,
                yellow: colors.amber,
                pink: colors.fuchsia,
            },
            fontFamily: {
                sans: ['Inter var', ...defaultTheme.fontFamily.sans],
            },
            transitionDuration: {
                '0': '0ms',
                '300': '300ms',
            },
            transitionTimingFunction: {
                'in-expo': 'cubic-bezier(0.95, 0.05, 0.795, 0.035)',
                'out-expo': 'cubic-bezier(0.19, 1, 0.22, 1)',
            },
            transitionProperty: {
                'height': 'height',
                'spacing': 'margin, padding',
                'display':'none'
            }
        },
    },
    variants: {
        animation: ['responsive', 'motion-safe', 'motion-reduce', 'hover']
    },
    plugins: [
        // ...
        require('@tailwindcss/forms'),
        require('@tailwindcss/aspect-ratio'),
        require('tailwindcss-font-inter')
    ],
}

