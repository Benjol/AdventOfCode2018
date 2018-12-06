let input = System.IO.File.ReadAllText(@"C:\Temp\ReallyTemp\input.txt")
open System.Text
open System.Text.RegularExpressions

let regex = Regex("(?i)(.)\1(?-i)(?<=[a-z][A-Z]|[A-Z][a-z])")
let rec reducePolymer polymerlist =
    let matches = regex.Matches(polymerlist)
    if matches.Count = 0 then
        polymerlist
    else
        let end_index = polymerlist.Length
        let next =
            matches
               |> Seq.cast<Match>
               |> Seq.filter (fun m -> m.Value.[0] <> m.Value.[1])
               |> Seq.map (fun m -> m.Index)
               |> (fun seq -> Seq.append seq [end_index]) //otherwise order inversed
               |> Seq.fold (fun (start_index, sb:StringBuilder) match_index ->
                                match_index + 2, sb.Append(polymerlist.Substring(start_index, match_index - start_index))
                                ) (0, StringBuilder())
               |> snd |> string
        if next = polymerlist then
            next
        else
            reducePolymer next

let reduced = reducePolymer input
let part1 = reduced.Length

let part2 = [|'a' .. 'z'|] |> Array.Parallel.map (fun c -> Regex.Replace(reduced, "(?i)" + string c, "")
                                                           |> reducePolymer |> String.length) |> Array.min
