//Part 1
let input = @"1, 1
1, 6
8, 3
3, 4
5, 5
8, 9"
open System
let coords =
    input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun line -> line.Split([|','|]) |> Array.map int |> (fun a -> a.[0], a.[1]))

let (l,t,r,b) = coords |> Array.fold (fun (l,t,r,b) (x, y) -> min l x, min t y, max r x, max b y) (Int32.MaxValue, Int32.MaxValue, Int32.MinValue, Int32.MinValue)

let manhattan (x1,y1) (x2,y2) = abs (x2 - x1) + abs (y2 - y1)

let store = Array.init (Array.length coords) (fun _ -> 0)

for x in [l .. r] do
   for y in [t .. b] do
     let distances = coords |> Array.mapi (fun id (cx,cy) -> id, manhattan (x,y) (cx,cy))
     let closest = distances |> Array.groupBy snd |> Array.sortBy fst |> Array.item 0 |> snd |> List.ofArray
     match closest with
     | (id,_)::[] -> store.[id] <- store.[id] + 1
     | _ -> ()
     
store |> Array.max

//Parallel and immutable and faster:
//get id of point which is closest to xy (or nothing if there are two equidistant points)
let closest points xy =
    points |> Array.mapi (fun id p -> id, manhattan p xy)
           |> Array.groupBy snd
           |> Array.sortBy fst
           |> Array.item 0
           |> snd
           |> List.ofArray
           |> function (id,_)::[] -> Some(id) | _ -> None

//parallel for each row
[|l .. r |] |> Array.Parallel.map (fun x -> [|t .. b|] |> Array.map (fun y -> closest coords (x,y))) 
            |> Array.concat 
            |> Array.choose id 
            |> Array.groupBy id
            |> Array.map snd
            |> Array.map Array.length
            |> Array.max
