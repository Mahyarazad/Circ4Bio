const colors = require('tailwindcss/colors');

module.exports = {
    content: ["./**/*.{html,js}"],
    theme: {
        extend: {
            colors: {
                teal: colors.teal,
                cyan: colors.cyan,
                sky: colors.sky,
            },
        },
    },
    plugins: [
        // ...
        require('@tailwindcss/forms'),
        require('@tailwindcss/aspect-ratio'),
    ],
}

