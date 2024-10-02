/** @type {import('tailwindcss').Config} */
const withMT = require("@material-tailwind/react/utils/withMT");

module.exports = withMT({
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      screens: {
        'custom': '1141px',
        'custom-xl': '1550px',
        'custom-modal-xl':'1290px',
        'cs': '650 px',
        'zero': '1 px'
      },
      colors: {
        'surface-darkest': '#1a1625',
        'surface-dark': '#2f2b3a',
        'surface-mid': '#46424f',
        'surface-mid-dark': '#5e5a66',
        'surface-mid-light': '#76737e',
        'surface-light-dark': '#908d96',
        'surface-light' : '#b4b1ba',
        'primary': '#ba9ffb',
        'secondary': '#a688fa',
        'surface-light-green':"#7ae582",
        'surface-dark-green':"#006400",
        'surface-white':"#FFFFFF",
        "surface-black":"#000000",
        "surface-off-white":"#FAF9F6",
        "surface-darker-white":"#daded9"
      },
    },
  },
  plugins: [

  ],
});
