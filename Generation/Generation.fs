module Generation

let oneDigitNumbers = [ 1..9 ] |> List.map string
let twoDigitNumbers = [ 10..99 ] |> List.map string
let threeDigitNumbers = [ 100..999 ] |> List.map string
let zeroThruNine = [ 0..9 ] |> List.map string
let operations = [ '+'; '-'; '*'; '/' ] |> List.map string

let patterns =
    [ "1#2=3" // 6
      "2#1=3"
      "2#2=2"
      "3#1=2"
      "3#2=Z"
      "1#1#1=2" // 5
      "1#1#2=Z"
      "1#2#1=Z"
      "2#1#1=Z" ]

let crossMultiply (left: string list) (right: string list) =
    List.collect (fun o -> List.map (fun t -> o + t) right) left

let translate (value: char) =
    match value with
    | 'Z' -> [ 0..9 ] |> List.map string
    | '1' -> [ 1..9 ] |> List.map string
    | '2' -> [ 10..99 ] |> List.map string
    | '3' -> [ 100..999 ] |> List.map string
    | '#' -> [ "+"; "-"; "*"; "/" ]
    | '=' -> [ "=" ]
    | _ -> failwith "Invalid argument"

let enumerate (pattern: string) =
    Seq.fold (fun acc value -> crossMultiply acc (translate value)) [""] pattern