//PART 1
let input = @"abcdef
bababc
abbcde
abcccd
aabcdd
abcdee
ababab
"

let twosOrThrees (box:string) =
    let trueIsOne bool = if bool then 1 else 0
    let counts = box.ToCharArray() |> Array.groupBy id |> Array.groupBy (snd >> Array.length) |> Array.map fst |> Set.ofArray
    (counts |> Set.contains 2 |> trueIsOne, counts |> Set.contains 3 |> trueIsOne)

let checksum (input:string) = 
    let twos, threes = input.Split([|'\n';'\r'|]) |> Array.Parallel.map twosOrThrees |> Array.unzip
    (Array.sum twos) * (Array.sum threes)
    
//PART 2
let input = @"abcde
fghij
klmno
pqrst
fguij
axcye
wvxyz
"

let crossJoin ls = [| for a in ls do for b in ls -> (a,b) |]
         
input.Split([|'\n';'\r'|], System.StringSplitOptions.RemoveEmptyEntries) 
    |> Array.map (fun id -> id.ToCharArray()) 
    |> crossJoin 
    |> Array.Parallel.map (fun (a,b) -> Array.zip a b |> Array.choose (fun (a,b) -> if a = b then Some a else None))
    |> Array.groupBy (fun s -> s.Length)
    |> Array.sortByDescending fst
    |> Array.item 1
    |> snd 
    |> Array.item 0
    |> System.String.Concat
        
