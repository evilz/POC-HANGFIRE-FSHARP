namespace POC_HANGFIRE_FSHARP

open System.Net.Http
open FSharp.Control.Tasks.V2
open Hangfire.Console
open HangfireConsole
open System.Threading.Tasks


module Jobs = 

    let doSomething() =
        printfn "I'm Gr00t!"


    let doSomethingAsyncTask context =
        
        let logger = context |> console

        task {
          logger.SetTextColor ConsoleTextColor.Green
          logger.WriteLine "Wait please ..." // should use format 
          logger.ResetTextColor()

          let progressBar = logger.ProgressBar()
          do! Task.Delay 10000
          progressBar.SetValue(20);
          do! Task.Delay 10000
          progressBar.SetValue(60);
          do! Task.Delay 10000
          progressBar.SetValue(100);
          
          use client = new HttpClient()
          let! result = client.GetAsync "https://jsonplaceholder.typicode.com/posts"
          ConsoleExtensions.WriteLine(context,sprintf "%A" result) 
        } :> Task

        