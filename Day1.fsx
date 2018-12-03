//PART 1
open System
let input = @"+1
+1
-2"
input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries) |> Array.sumBy int

//PART 2
open System
let input = @"+1
+1
-2"

let seq = input.Split([|',';'\n';'\r'|], StringSplitOptions.RemoveEmptyEntries) |> Seq.map int

Seq.initInfinite (fun _ -> seq)
  |> Seq.concat
  |> Seq.scan (fun (set,sum,found) next -> 
                let freq = sum + next
                if Set.contains freq set then 
                   (set,freq,true)
                else
                   (Set.add freq set, freq, false)
                ) (Set.empty<int>, 0, false)
  |> Seq.find (fun (_,_,found) -> found)
  |> fun(_,freq,_) -> freq
