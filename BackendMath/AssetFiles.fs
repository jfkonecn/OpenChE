namespace EngineeringMath
open System.IO
open System.Reflection

module internal AssetFiles =
    let fetchStream resourceName = 
        Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) 


    let getAssetFileContents fileName =   
        let readFile (stream:Stream) =
            use x = stream
            use reader = new StreamReader(x)
            reader.ReadToEndAsync()

        let loadFile resouceName = fetchStream resouceName |> readFile |> Async.AwaitTask

        Assembly.GetExecutingAssembly().GetManifestResourceNames()
        |> Array.filter (fun x -> x.EndsWith(fileName)) 
        |> Array.tryExactlyOne 
        |> Option.map loadFile
