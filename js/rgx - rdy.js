(/^\d{4}\/\d{2}\/\d{2}$/)    // date string with forward slashes
(/^[\d.,;-]*$/)              // only digits, dot, comma, semicolon and hyphen (0 or more)
(/^\s*(\b|\B)/)              // leading whitespace characters
(/^(?!.*hello.*).*/)         // all lines except those with "hello" in them
(/^\n/)                      // all empty lines
(/^(\d+\s?)+$/)              // space-separated string of positive integers
(/<div id="a">(.*?)<\/div>/) // content of div (until next              closing div)
(/<div id="a">(.*)<\/div>/)  // ...            (until I don't know what closing div)
(\/\*\*\s*\n([^\*]|\*[^\/])*\*\/) // jsdoc comments

// logical AND multiword search
(word1[\s\S\n]*word2)|(word2[\s\S\n]*word1)
(word1[\s\S]*word2)|(word2[\s\S]*word1) // on same line

// replace comma with dot
(/(\d+),(\d{1,})/g, '$1.$2')

// separate last 3 digits with comma
(/(\d+)(\d{3})/, '$1,$2')

// fa date string from 1380 to 1419 (without mil & cent)
(/^[0189]{1}\d{1}\/(1[0-2]|[1-9])\/([1-9]|[1-2]\d|3[01])$/)


// number between
(/^[1-9]$|^1[0-4]$/)            // 1-14
(/^\d$|^\d\d$/)                 // 0-99
(/^([1-9]|[12]\d|3[0-6])$/)     // 1-36
(/^[0-9]|[0-2][0-9]|3[0-6]$/)   // 0-36
(/^(1[0-2]|[1-9])$/)            // 1-12
(/^([1-9]|[1-2]\d|3[01])$/)     // 1-31
(/^(3[01]|[12][0-9]|[1-9])$/)   // 1-31

(/^(2[0-4]|1[0-9]|[1-9])$/)     // 1-24
(/^(5[0-3]|[1-4][0-9]|[1-9])$/) // 1-53
(/^[1-5]?[0-9]$/)               // 0-59
(/^(100|[1-9]?[0-9])$/)         // 0-100
(/^(12[0-7]|1[01][0-9]|[1-9]?[0-9])$/)       // 0-127
(/^(12[0-6]|1[01][0-9]|[4-9][0-9]|3[2-9])$/) // 32-126