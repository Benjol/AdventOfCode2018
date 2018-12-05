//PART 1
let input = @"[1518-11-01 00:00] Guard #10 begins shift
[1518-11-01 00:05] falls asleep
[1518-11-01 00:25] wakes up
[1518-11-01 00:30] falls asleep
[1518-11-01 00:55] wakes up
[1518-11-01 23:58] Guard #99 begins shift
[1518-11-02 00:40] falls asleep
[1518-11-02 00:50] wakes up
[1518-11-03 00:05] Guard #10 begins shift
[1518-11-03 00:24] falls asleep
[1518-11-03 00:29] wakes up
[1518-11-04 00:02] Guard #99 begins shift
[1518-11-04 00:36] falls asleep
[1518-11-04 00:46] wakes up
[1518-11-05 00:03] Guard #99 begins shift
[1518-11-05 00:45] falls asleep
[1518-11-05 00:55] wakes up
"

open System.Text.RegularExpressions
type Action = 
    | Sleep of int
    | Wake of int
    | BeginShift of int
    static member Parse (s:string) = 
        match s.Substring(19, 4) with
        | "Guar" -> BeginShift(Regex.Match(s.Substring(19), @"(\d+)").Value |> int)
        | "wake" -> Wake(Regex.Match(s.Substring(15,2), @"(\d+)").Value |> int)
        | "fall" -> Sleep(Regex.Match(s.Substring(15,2), @"(\d+)").Value |> int)
        | _ -> failwith ("Parse error: " + s)
 
input.Split([|'\n';'\r'|], System.StringSplitOptions.RemoveEmptyEntries)
|> Array.sort
|> Array.map Action.Parse
|> Array.fold (fun (map,id,start) action -> 
                match action with
                | BeginShift(guard) -> 
                    if Map.containsKey guard map then
                        map, guard, 0
                    else
                        Map.add guard [] map, guard, 0
                | Sleep(sleeptime) -> map, id, sleeptime
                | Wake(waketime) -> 
                    Map.add id ([start .. (waketime - 1)] @ Map.find id map) map, id, 0
                ) (Map.empty,0,0)
|> (fun (map,_,_) -> map)
|> Map.toArray
|> Array.sortByDescending (fun (k,v) -> List.length v)
|> Array.item 0
|> (fun (k,v) -> k, v |> List.groupBy id |> List.sortByDescending (fun (_, l) -> List.length l) |> List.item 0)
|> (fun (guard,(k,v)) -> guard * k)

//PART 2
let input = @"[1518-11-01 00:00] Guard #10 begins shift
[1518-11-01 00:05] falls asleep
[1518-11-01 00:25] wakes up
[1518-11-01 00:30] falls asleep
[1518-11-01 00:55] wakes up
[1518-11-01 23:58] Guard #99 begins shift
[1518-11-02 00:40] falls asleep
[1518-11-02 00:50] wakes up
[1518-11-03 00:05] Guard #10 begins shift
[1518-11-03 00:24] falls asleep
[1518-11-03 00:29] wakes up
[1518-11-04 00:02] Guard #99 begins shift
[1518-11-04 00:36] falls asleep
[1518-11-04 00:46] wakes up
[1518-11-05 00:03] Guard #99 begins shift
[1518-11-05 00:45] falls asleep
[1518-11-05 00:55] wakes up
"

open System.Text.RegularExpressions
type Action = 
    | Sleep of int
    | Wake of int
    | BeginShift of int
    static member Parse (s:string) = 
        match s.Substring(19, 4) with
        | "Guar" -> BeginShift(Regex.Match(s.Substring(19), @"(\d+)").Value |> int)
        | "wake" -> Wake(Regex.Match(s.Substring(15,2), @"(\d+)").Value |> int)
        | "fall" -> Sleep(Regex.Match(s.Substring(15,2), @"(\d+)").Value |> int)
        | _ -> failwith ("Parse error: " + s)
 
input.Split([|'\n';'\r'|], System.StringSplitOptions.RemoveEmptyEntries)
|> Array.sort
|> Array.map Action.Parse
|> Array.fold (fun (map,id,start) action -> 
                match action with
                | BeginShift(guard) -> 
                    if Map.containsKey guard map then
                        map, guard, 0
                    else
                        Map.add guard [] map, guard, 0
                | Sleep(sleeptime) -> map, id, sleeptime
                | Wake(waketime) -> 
                    Map.add id ([start .. (waketime - 1)] @ Map.find id map) map, id, 0
                ) (Map.empty,0,0)
|> (fun (map,_,_) -> map)
|> Map.toArray
|> Array.filter (fun (guard, minutes) -> (List.length minutes) > 0)
|> Array.map (fun (guard,minutes) -> guard, minutes |> List.groupBy id |> List.map (fun (minute,instances) -> minute, List.length instances) |> List.sortByDescending snd |> List.item 0)
|> Array.sortByDescending (fun (guard,(minute,length)) -> length)
|> Array.item 0
|> (fun (guard,(minute,length)) -> guard * minute)
