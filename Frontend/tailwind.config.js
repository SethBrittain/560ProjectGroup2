/** @type {import('tailwindcss').Config} */
module.exports = {
  mode: process.env.TAILWIND_MODE ? 'jit':'',
  darkMode: 'media',
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    fontFamily: {
      sans: ['Karla', 'sans-serif'],
    },
    extend: {},
  },
  plugins: [
  ],
}

