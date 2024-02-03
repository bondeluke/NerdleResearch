module Generation

let private patterns =
    [ "123"; "213"; "222"; "312"; "321"; "1112"; "1121"; "1211"; "2111" ]

let private enhance (pattern: string) : string =
    pattern
    |> Seq.mapi (fun index digit ->
        if index < (pattern.Length - 2) then $"{digit}#"
        elif index = (pattern.Length - 2) then $"{digit}="
        elif digit = '1' then "Z" // The last digit can be zero
        else $"{digit}")
    |> String.concat ""

let private translate (value: char) : string list =
    match value with
    | 'Z' -> [ 0..9 ] |> List.map string
    | '1' -> [ 1..9 ] |> List.map string
    | '2' -> [ 10..99 ] |> List.map string
    | '3' -> [ 100..999 ] |> List.map string
    | '#' -> [ "+"; "-"; "*"; "/" ]
    | '=' -> [ "=" ]
    | _ -> failwith $"Invalid argument '{value}'"


let private multiply (left: string list) (right: string list) : string list =
    List.collect (fun l -> List.map (fun r -> l + r) right) left

let private enumerate (pattern: string) =
    Seq.fold (fun acc value -> translate value |> multiply acc) [ "" ] pattern

let possibleSolutions = (List.map enhance >> List.collect enumerate) patterns
