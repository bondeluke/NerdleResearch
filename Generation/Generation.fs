module Generation

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

let translate (value: char) =
    match value with
    | 'Z' -> [ 0..9 ] |> List.map string
    | '1' -> [ 1..9 ] |> List.map string
    | '2' -> [ 10..99 ] |> List.map string
    | '3' -> [ 100..999 ] |> List.map string
    | '#' -> [ "+"; "-"; "*"; "/" ]
    | '=' -> [ "=" ]
    | _ -> failwith "Invalid argument"

let multiply (left: string list) (right: string list) =
    List.collect (fun l -> List.map (fun r ->  l + r) right) left

let enumerate (pattern: string) =
    Seq.fold (fun acc value -> multiply acc (translate value)) [""] pattern