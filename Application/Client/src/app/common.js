import { StyleSheet } from 'react-native';

export default StyleSheet.create({
  'body': {
    'backgroundColor': '#ffffff',
    'fontFamily': 'pen Sans',
    'fontSize': [{ 'unit': 'px', 'value': 14 }],
    'lineHeight': [{ 'unit': 'px', 'value': 1 }]
  },
  'fontFace': {
    'fontFamily': 'ArvoItalic',
    'src': 'url("/src/assets/fonts/ArvoItalic.eot")',
    'src': 'url("/src/assets/fonts/ArvoItalic.eot") format("embedded-opentype"), url("/assets/fonts/ArvoItalic.woff2") format("woff2"), url("/src/assets/fonts/ArvoItalic.woff") format("woff"), url("/src/assets/fonts/ArvoItalic.ttf") format("truetype"), url("/src/assets/fonts/ArvoItalic.svg#ArvoItalic") format("svg")'
  },
  'fontFace': {
    'fontFamily': 'ArvoBold',
    'src': 'url("/src/assets/fonts/ArvoBold.eot")',
    'src': 'url("/src/assets/fonts/ArvoBold.eot") format("embedded-opentype"), url("/assets/fonts/ArvoBold.woff2") format("woff2"), url("/src/assets/fonts/ArvoBold.woff") format("woff"), url("/src/assets/fonts/ArvoBold.ttf") format("truetype"), url("/src/assets/fonts/ArvoBold.svg#ArvoBold") format("svg")'
  },
  'fontFace': {
    'fontFamily': 'ArvoBoldItalic',
    'src': 'url("/src/assets/fonts/ArvoBoldItalic.eot")',
    'src': 'url("/src/assets/fonts/ArvoBoldItalic.eot") format("embedded-opentype"), url("/src/assets/fonts/ArvoBoldItalic.woff2") format("woff2"), url("/src/assets/fonts/ArvoBoldItalic.woff") format("woff"), url("/src/assets/fonts/ArvoBoldItalic.ttf") format("truetype"), url("/src/assets/fonts/ArvoBoldItalic.svg#ArvoBoldItalic") format("svg")'
  },
  'fontFace': {
    'fontFamily': 'ArvoRegular',
    'src': 'url("/src/assets/fonts/ArvoRegular.eot")',
    'src': 'url("/src/assets/fonts/ArvoRegular.eot") format("embedded-opentype"), url("/assets/fonts/ArvoRegular.woff2") format("woff2"), url("/src/assets/fonts/ArvoRegular.woff") format("woff"), url("/src/assets/fonts/ArvoRegular.ttf") format("truetype"), url("/src/assets/fonts/ArvoRegular.svg#ArvoRegular") format("svg")'
  },
  'fontFace': {
    'fontFamily': 'icomoon',
    'src': 'url("/assets/fonts/icon-fonts/icomoon.eot?2now9z")',
    'src': 'url("/assets/fonts/icon-fonts/icomoon.eot?2now9z#iefix") format("embedded-opentype"), url("/assets/fonts/icon-fonts/icomoon.ttf?2now9z") format("truetype"), url("/assets/fonts/icon-fonts/icomoon.woff?2now9z") format("woff"), url("/assets/fonts/icon-fonts/icomoon.svg?2now9z#icomoon") format("svg")',
    'fontWeight': 'normal',
    'fontStyle': 'normal'
  },
  '[class^="icon-"]': {
    // use !important to prevent issues with browser extensions that change fonts
    'fontFamily': 'icomoon' ,
    'speak': 'none',
    'fontStyle': 'normal',
    'fontWeight': 'normal',
    'fontVariant': 'normal',
    'textTransform': 'none',
    'lineHeight': [{ 'unit': 'px', 'value': 1 }],
    // Better Font Rendering ===========
    'WebkitFontSmoothing': 'antialiased',
    'MozOsxFontSmoothing': 'grayscale'
  },
  '[class*=" icon-"]': {
    // use !important to prevent issues with browser extensions that change fonts
    'fontFamily': 'icomoon',
    'speak': 'none',
    'fontStyle': 'normal',
    'fontWeight': 'normal',
    'fontVariant': 'normal',
    'textTransform': 'none',
    'lineHeight': [{ 'unit': 'px', 'value': 1 }],
    // Better Font Rendering ===========
    'WebkitFontSmoothing': 'antialiased',
    'MozOsxFontSmoothing': 'grayscale'
  },
  'icon-clock:before': {
    'content': '"\e900"'
  },
  'icon-flag:before': {
    'content': '"\e901"'
  },
  'icon-like-icon:before': {
    'content': '"\e902"'
  },
  'icon-menu-icon:before': {
    'content': '"\e903"'
  },
  'icon-search-icon:before': {
    'content': '"\e904"'
  },
  'spriteimage': {
    'background': 'url("/assets/img/banner-css-sprite.png") no-repeat'
  },
  'electronicIcon': {
    'width': [{ 'unit': 'px', 'value': 91 }],
    'height': [{ 'unit': 'px', 'value': 80 }],
    'backgroundPosition': '0 0',
    'display': 'block'
  },
  'housingIcon': {
    'width': [{ 'unit': 'px', 'value': 78 }],
    'height': [{ 'unit': 'px', 'value': 80 }],
    'backgroundPosition': '-92px 0',
    'display': 'block'
  },
  'automotiveIcon': {
    'width': [{ 'unit': 'px', 'value': 91 }],
    'height': [{ 'unit': 'px', 'value': 80 }],
    'backgroundPosition': '-171px 0',
    'display': 'block'
  },
  'furnitureIcon': {
    'width': [{ 'unit': 'px', 'value': 97 }],
    'height': [{ 'unit': 'px', 'value': 80 }],
    'backgroundPosition': '-264px 0',
    'display': 'block'
  },
  'assortedIcon': {
    'width': [{ 'unit': 'px', 'value': 89 }],
    'height': [{ 'unit': 'px', 'value': 80 }],
    'backgroundPosition': '-363px 0',
    'display': 'block'
  }
});
