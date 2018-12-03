//PART 1
let input = @"#1 @ 1,3: 4x4
#2 @ 3,1: 4x4
#3 @ 5,5: 2x2
"

let fabric = Array.init 1000 (fun _ -> Array.init 1000 (fun _ -> 0))

input.Split([|'\n';'\r'|], System.StringSplitOptions.RemoveEmptyEntries) 
    |> Array.map (fun l -> l.Split([|'#';'@';',';'x';':'|], System.StringSplitOptions.RemoveEmptyEntries))
    |> Array.map (Array.map int)
    |> Array.map (fun arr -> (arr.[1], arr.[2], arr.[3], arr.[4]))
    |> Array.iter (fun (left, top, w, h) -> 
                        for x = left to left + w - 1 do
                            for y = top to top + h - 1 do
                                fabric.[x].[y] <- fabric.[x].[y] + 1)
fabric |> Array.sumBy (fun line -> line |> Array.sumBy (fun c -> if c > 1 then 1 else 0))

//PART 2
let input = @"#1 @ 1,3: 4x4
#2 @ 3,1: 4x4
#3 @ 5,5: 2x2
"

let taken = Array.init 1000 (fun _ -> Array.init 1000 (fun _ -> []))
let claims = 
    input.Split([|'\n';'\r'|], System.StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun l -> l.Split([|'#';'@';',';'x';':'|], System.StringSplitOptions.RemoveEmptyEntries))
    |> Array.map (Array.map int)

let intact = Array.init (Array.length claims) (fun _ -> true)

claims
    |> Array.map (fun arr -> (arr.[0], arr.[1], arr.[2], arr.[3], arr.[4]))
    |> Array.iter (fun (id, left, top, width, height) ->
                        for x = left to left + width - 1 do
                            for y = top to top + height - 1 do
                                if taken.[x].[y] = [] then
                                    taken.[x].[y] <- [id]
                                else
                                    taken.[x].[y] <- id :: taken.[x].[y]
                                    List.iter (fun i -> intact.[i - 1] <- false) taken.[x].[y])

let answer = (intact |> Array.findIndex id) + 1 //assuming ids are contiguous and start at 1
