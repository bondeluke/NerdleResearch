module Generation

let oneDigitNumbers = [ 0..9 ] |> List.map string
let twoDigitNumbers = [ 10..99 ] |> List.map string
let threeDigitNumbers = [ 100..999 ] |> List.map string
let operations = [ '+'; '-'; '*'; '/' ] |> List.map string

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

let lastCharacter (input: string) =
    input.[(String.length input) - 1] |> string

let enumerateOptions (pattern: string) = lastCharacter pattern
//pattern |> (String.map (fun char -> char.ToString() |> ); char)

let printPatterns () =
    for pattern in patterns do
        printfn "%s" pattern
        printfn "%s" (enumerateOptions pattern)

let crossMultiply (left: string list) (right: string list) =
    List.collect (fun o -> List.map (fun t -> o + t) right) left

let convert (value: char) =
    match value with
    | '1' -> oneDigitNumbers
    | '2' -> twoDigitNumbers
    | '3' -> threeDigitNumbers
    | '#' -> operations
    | _ -> failwith "Invalid argument"
