/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ['./**/*.{razor,html}'],
    darkMode: 'class',
    theme: {
        extend: {
            backgroundImage: {
                'login': "url('/images/login-pattern1.png')",
                'login2': "url('/images/login-pattern2.svg')",
            },
            colors: {
                primary: 'rgb(var(--color-primary) / <alpha-value>)',
                secondary: 'rgb(var(--color-secondary) / <alpha-value>)',
                primarytext: 'rgb(var(--color-primary-text) / <alpha-value>)',
                secondarytext: 'rgb(var(--color-secondary-text) / <alpha-value>)',
            },
        },
    },
    plugins: [],
}

