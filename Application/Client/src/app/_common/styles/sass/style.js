import { StyleSheet } from 'react-native';

export default StyleSheet.create({
  // Media queries breakpoints
  // Extra, extra small screen  phone
  // Extra small screen  phone
  // Small screen  tablet
  // Medium screen  desktop
  // Large screen  wide desktop
  // Large screen  wide desktop
  // Extra small screen / phone
/* Deprecated `$screen-xs` as of v3.0.1
  // Deprecated `$screen-xs-min` as of v3.2.0
  // Deprecated `$screen-phone` as of v3.0.1
  // Small screen / tablet
/* Deprecated `$screen-sm` as of v3.0.1
  // Deprecated `$screen-tablet` as of v3.0.1
  // Medium screen / desktop
/*  Deprecated `$screen-md` as of v3.0.1
  // Deprecated `$screen-desktop` as of v3.0.1
  // Large screen / wide desktop
/*  Deprecated `$screen-lg` as of v3.0.1
  // Deprecated `$screen-lg-desktop` as of v3.0.1
  // So media queries don't overlap when required, provide a maximum
  'fontFace': {
    'fontFamily': ''ArvoItalic'',
    'src': 'url("/src/assets/fonts/ArvoItalic.eot")',
    'src': 'url("/src/assets/fonts/ArvoItalic.eot") format("embedded-opentype"), url("/assets/fonts/ArvoItalic.woff2") format("woff2"), url("/src/assets/fonts/ArvoItalic.woff") format("woff"), url("/src/assets/fonts/ArvoItalic.ttf") format("truetype"), url("/src/assets/fonts/ArvoItalic.svg#ArvoItalic") format("svg")'
  },
  'fontFace': {
    'fontFamily': ''ArvoBold'',
    'src': 'url("/src/assets/fonts/ArvoBold.eot")',
    'src': 'url("/src/assets/fonts/ArvoBold.eot") format("embedded-opentype"), url("/assets/fonts/ArvoBold.woff2") format("woff2"), url("/src/assets/fonts/ArvoBold.woff") format("woff"), url("/src/assets/fonts/ArvoBold.ttf") format("truetype"), url("/src/assets/fonts/ArvoBold.svg#ArvoBold") format("svg")'
  },
  'fontFace': {
    'fontFamily': ''ArvoBoldItalic'',
    'src': 'url("/src/assets/fonts/ArvoBoldItalic.eot")',
    'src': 'url("/src/assets/fonts/ArvoBoldItalic.eot") format("embedded-opentype"), url("/src/assets/fonts/ArvoBoldItalic.woff2") format("woff2"), url("/src/assets/fonts/ArvoBoldItalic.woff") format("woff"), url("/src/assets/fonts/ArvoBoldItalic.ttf") format("truetype"), url("/src/assets/fonts/ArvoBoldItalic.svg#ArvoBoldItalic") format("svg")'
  },
  'fontFace': {
    'fontFamily': ''ArvoRegular'',
    'src': 'url("/src/assets/fonts/ArvoRegular.eot")',
    'src': 'url("/src/assets/fonts/ArvoRegular.eot") format("embedded-opentype"), url("/assets/fonts/ArvoRegular.woff2") format("woff2"), url("/src/assets/fonts/ArvoRegular.woff") format("woff"), url("/src/assets/fonts/ArvoRegular.ttf") format("truetype"), url("/src/assets/fonts/ArvoRegular.svg#ArvoRegular") format("svg")'
  },
  'fontFace': {
    'fontFamily': ''icomoon'',
    'src': 'url("/assets/fonts/icon-fonts/icomoon.eot?2now9z")',
    'src': 'url("/assets/fonts/icon-fonts/icomoon.eot?2now9z#iefix") format("embedded-opentype"), url("/assets/fonts/icon-fonts/icomoon.ttf?2now9z") format("truetype"), url("/assets/fonts/icon-fonts/icomoon.woff?2now9z") format("woff"), url("/assets/fonts/icon-fonts/icomoon.svg?2now9z#icomoon") format("svg")',
    'fontWeight': 'normal',
    'fontStyle': 'normal'
  },
  'input': {
    'fontSize': [{ 'unit': 'px', 'value': 14 }],
    'fontFamily': '"Open Sans"',
    'fontWeight': '400'
  },
  'select': {
    'fontSize': [{ 'unit': 'px', 'value': 14 }],
    'fontFamily': '"Open Sans"',
    'fontWeight': '400'
  },
  'blue-tag li a': {
    'fontSize': [{ 'unit': 'em', 'value': 0.7142 }],
    'fontFamily': '"Open Sans"',
    'fontWeight': '900'
  },
  'grey-tag li a': {
    'fontSize': [{ 'unit': 'em', 'value': 0.7142 }],
    'fontFamily': '"Open Sans"',
    'fontWeight': '900'
  },
  'small-green-button': {
    'fontSize': [{ 'unit': 'em', 'value': 0.59 }],
    'fontFamily': '"Open Sans"',
    'fontWeight': '900'
  },
  '[class^="icon-"]': {
    // use !important to prevent issues with browser extensions that change fonts
    'fontFamily': ''icomoon' !important',
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
    'fontFamily': ''icomoon' !important',
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
  'icon-Arrow:before': {
    'content': '"\e906"',
    'color': 'red'
  },
  'icon-bubbles:before': {
    'content': '"\e907"'
  },
  'icon-checkbox-with-tick:before': {
    'content': '"\e908"'
  },
  'icon-checkbox:before': {
    'content': '"\e909"'
  },
  'icon-close-button:before': {
    'content': '"\e90a"'
  },
  'icon-close-without-circle:before': {
    'content': '"\e90b"'
  },
  'icon-G + :before': {
    'content': '"\e90c"'
  },
  'icon-mappin:before': {
    'content': '"\e90d"'
  },
  'icon-User-icon:before': {
    'content': '"\e90e"'
  },
  'icon-Mobile_BackArrow:before': {
    'content': '"\e90f"'
  },
  'icon-Mobile_DownArrow:before': {
    'content': '"\e910"'
  },
  'icon-Mobile_EditIcon:before': {
    'content': '"\e911"'
  },
  'icon-Mobile_PlusIcon:before': {
    'content': '"\e912"'
  },
  'icon-Mobile_QuoteIcon:before': {
    'content': '"\e913"'
  },
  'icon-Mobile_Tick:before': {
    'content': '"\e914"'
  },
  'icon-Mobile_UpArrow:before': {
    'content': '"\e915"'
  },
  'icon-profile:before': {
    'content': '"\e905"'
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
  'body': {
    'backgroundColor': '#ffffff',
    'fontFamily': '"Open Sans"',
    'fontSize': [{ 'unit': 'px', 'value': 14 }],
    'lineHeight': [{ 'unit': 'px', 'value': 1 }]
  },
  'spriteimage': {
    'background': 'url("/assets/img/banner-css-sprite.png") no-repeat'
  },
  'ElectronicsIcon': {
    'width': [{ 'unit': 'px', 'value': 91 }],
    'height': [{ 'unit': 'px', 'value': 80 }],
    'backgroundPosition': '0 0',
    'display': 'block'
  },
  'HousingIcon': {
    'width': [{ 'unit': 'px', 'value': 78 }],
    'height': [{ 'unit': 'px', 'value': 80 }],
    'backgroundPosition': '-92px 0',
    'display': 'block'
  },
  'AutomotiveIcon': {
    'width': [{ 'unit': 'px', 'value': 91 }],
    'height': [{ 'unit': 'px', 'value': 80 }],
    'backgroundPosition': '-171px 0',
    'display': 'block'
  },
  'FurnitureIcon': {
    'width': [{ 'unit': 'px', 'value': 97 }],
    'height': [{ 'unit': 'px', 'value': 80 }],
    'backgroundPosition': '-264px 0',
    'display': 'block'
  },
  'OtherIcon': {
    'width': [{ 'unit': 'px', 'value': 89 }],
    'height': [{ 'unit': 'px', 'value': 80 }],
    'backgroundPosition': '-363px 0',
    'display': 'block'
  },
  'fixed': {
    'position': 'fixed !important',
    'left': [{ 'unit': 'px', 'value': 0 }],
    'right': [{ 'unit': 'px', 'value': 0 }],
    'zIndex': '1',
    'width': [{ 'unit': '%H', 'value': 1 }]
  },
  'no-padding': {
    'padding': [{ 'unit': 'px', 'value': 0 }, { 'unit': 'px', 'value': 0 }, { 'unit': 'px', 'value': 0 }, { 'unit': 'px', 'value': 0 }]
  },
  'no-padding-left': {
    'paddingLeft': [{ 'unit': 'px', 'value': 0 }]
  },
  'no-padding-right': {
    'paddingRight': [{ 'unit': 'px', 'value': 0 }]
  },
  'pad-topbot-17': {
    'padding': [{ 'unit': 'px', 'value': 17 }, { 'unit': 'px', 'value': 0 }, { 'unit': 'px', 'value': 17 }, { 'unit': 'px', 'value': 0 }]
  },
  'pad-topbot-20': {
    'padding': [{ 'unit': 'px', 'value': 20 }, { 'unit': 'px', 'value': 0 }, { 'unit': 'px', 'value': 20 }, { 'unit': 'px', 'value': 0 }]
  },
  'pad-bot-20': {
    'paddingBottom': [{ 'unit': 'px', 'value': 20 }]
  },
  'pad-bot-35': {
    'paddingBottom': [{ 'unit': 'px', 'value': 35 }]
  },
  'pad-right-20': {
    'paddingRight': [{ 'unit': 'px', 'value': 20 }]
  },
  'pad-top-40': {
    'paddingTop': [{ 'unit': 'px', 'value': 40 }]
  },
  'inputfull-width': {
    'width': [{ 'unit': '%H', 'value': 1 }]
  },
  'blue-tag': {
    'paddingLeft': [{ 'unit': 'px', 'value': 0 }],
    'margin': [{ 'unit': 'px', 'value': 20 }, { 'unit': 'px', 'value': 0 }, { 'unit': 'px', 'value': 20 }, { 'unit': 'px', 'value': 0 }],
    'textAlign': 'center',
    'listStyle': 'none',
    'maxHeight': [{ 'unit': 'px', 'value': 141 }],
    'overflowY': 'auto'
  },
  'blue-tag li': {
    'margin': [{ 'unit': 'px', 'value': 10 }, { 'unit': 'px', 'value': 10 }, { 'unit': 'px', 'value': 10 }, { 'unit': 'px', 'value': 0 }],
    'display': 'inline-block'
  },
  'blue-tag li a': {
    'color': '#ffffff',
    'backgroundColor': '#495574',
    'border': [{ 'unit': 'px', 'value': 1 }, { 'unit': 'string', 'value': 'solid' }, { 'unit': 'string', 'value': '#495574' }],
    'padding': [{ 'unit': 'px', 'value': 8 }, { 'unit': 'px', 'value': 23 }, { 'unit': 'px', 'value': 8 }, { 'unit': 'px', 'value': 23 }],
    'textTransform': 'uppercase',
    'display': 'inline-block',
    'borderRadius': '15px'
  },
  'blue-tag li a:hover': {
    'backgroundColor': '#495574'
  },
  'input': {
    'backgroundColor': '#fafafa',
    'padding': [{ 'unit': 'px', 'value': 10 }, { 'unit': 'px', 'value': 10 }, { 'unit': 'px', 'value': 10 }, { 'unit': 'px', 'value': 10 }],
    'marginTop': [{ 'unit': 'px', 'value': 0 }],
    'border': [{ 'unit': 'px', 'value': 2 }, { 'unit': 'string', 'value': 'solid' }, { 'unit': 'string', 'value': '#efeff6' }],
    'borderRadius': '5px',
    'color': '#9fa3a7'
  },
  'select': {
    'backgroundColor': '#fafafa',
    'padding': [{ 'unit': 'px', 'value': 10 }, { 'unit': 'px', 'value': 10 }, { 'unit': 'px', 'value': 10 }, { 'unit': 'px', 'value': 10 }],
    'marginTop': [{ 'unit': 'px', 'value': 0 }],
    'border': [{ 'unit': 'px', 'value': 2 }, { 'unit': 'string', 'value': 'solid' }, { 'unit': 'string', 'value': '#efeff6' }],
    'borderRadius': '5px',
    'color': '#9fa3a7'
  },
  'green-button': {
    'padding': [{ 'unit': 'px', 'value': 15 }, { 'unit': 'px', 'value': 25 }, { 'unit': 'px', 'value': 15 }, { 'unit': 'px', 'value': 25 }],
    'borderRadius': '30px',
    'background': '#c1d72f',
    'color': '#ffffff',
    'border': [{ 'unit': 'string', 'value': 'none' }],
    'textTransform': 'uppercase',
    '<w768': {
      'width': [{ 'unit': '%H', 'value': 1 }],
      'margin': [{ 'unit': 'px', 'value': 20 }, { 'unit': 'px', 'value': 0 }, { 'unit': 'px', 'value': 20 }, { 'unit': 'px', 'value': 0 }]
    }
  },
  'green-circle-button': {
    'width': [{ 'unit': 'px', 'value': 30 }],
    'height': [{ 'unit': 'px', 'value': 30 }],
    'borderRadius': '50%',
    'background': '#c1d72f',
    'color': '#ffffff',
    'color': '#ffffff',
    'border': [{ 'unit': 'string', 'value': 'none' }]
  },
  'small-green-button': {
    'padding': [{ 'unit': 'px', 'value': 6 }, { 'unit': 'px', 'value': 15 }, { 'unit': 'px', 'value': 6 }, { 'unit': 'px', 'value': 15 }],
    'borderRadius': '30px',
    'background': '#c1d72f',
    'color': '#ffffff',
    'border': [{ 'unit': 'string', 'value': 'none' }],
    'textTransform': 'uppercase',
    'letterSpacing': [{ 'unit': 'px', 'value': 1 }]
  },
  'orange-border-button': {
    'padding': [{ 'unit': 'px', 'value': 15 }, { 'unit': 'px', 'value': 40 }, { 'unit': 'px', 'value': 15 }, { 'unit': 'px', 'value': 40 }],
    'borderRadius': '50px',
    'background': '#ffffff',
    'border': [{ 'unit': 'px', 'value': 3 }, { 'unit': 'string', 'value': 'solid' }, { 'unit': 'string', 'value': '#ffc80a' }],
    'textTransform': 'uppercase',
    'letterSpacing': [{ 'unit': 'px', 'value': 3 }],
    'color': '#292929'
  },
  'grey-tag li a': {
    'color': '#e1e1e1',
    'backgroundColor': '#fafafa',
    'border': [{ 'unit': 'px', 'value': 1 }, { 'unit': 'string', 'value': 'solid' }, { 'unit': 'string', 'value': '#e1e1e1' }],
    'padding': [{ 'unit': 'px', 'value': 8 }, { 'unit': 'px', 'value': 13 }, { 'unit': 'px', 'value': 8 }, { 'unit': 'px', 'value': 13 }]
  },
  'grey-tag li a:hover': {
    'backgroundColor': '#fafafa'
  },
  'grey-tag li a i': {
    'paddingLeft': [{ 'unit': 'px', 'value': 10 }]
  }
});
