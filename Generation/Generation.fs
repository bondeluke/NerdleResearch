module Generation

let hello name = printfn "Hello %s" name

let oneDigitNumbers = [ 0..9 ]
let twoDigitNumbers = [ 10..99 ]
let threeDigitNumbers = [ 100..999 ]
let operations = [ '+'; '-'; '*'; '/' ]

//let private patterns =
//    [ "1#2=3"
//      "2#1=3"
//      "2#2=2"
//      "3#1=2"
//      "3#2=1"
//      "1#1#1=2"
//      "1#1#2=1"
//      "1#2#1=1"
//      "2#1#1=1" ]

let patterns =
    [ "123" // 6
      "213"
      "222"
      "312"
      "321"
      "1112" // 5
      "1121"
      "1211"
      "2111" ]

let lastCharacter(input: string)=
    input.[(String.length input) - 1] |> string

let enumerateOptions(pattern: string)=
    lastCharacter pattern
    //pattern |> (String.map (fun char -> char.ToString() |> ); char)

let printPatterns () =
    for pattern in patterns do
        printfn "%s" pattern
        printfn "%s" (enumerateOptions pattern)
