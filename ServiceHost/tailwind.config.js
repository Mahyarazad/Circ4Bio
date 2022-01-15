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

