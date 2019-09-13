namespace POC_HANGFIRE_FSHARP

open Hangfire
open Hangfire.Console
open Hangfire.Console.Progress

module HangfireConsole =
    // Builder can be better
    type IPrintable =
        abstract member SetTextColor : ConsoleTextColor -> unit
        abstract member ResetTextColor : unit -> unit
        abstract member WriteLine : string -> unit
        abstract member WriteColorLine : ConsoleTextColor -> string -> unit
        abstract member WriteEmptyLine : unit -> unit
        abstract member ProgressBar : unit -> IProgressBar
       

    let console(context:Hangfire.Server.PerformContext) =
        { new IPrintable with
            member this.SetTextColor(color) = context.SetTextColor(color)
            member this.ResetTextColor() = context.ResetTextColor()
            member this.WriteLine(text) = context.WriteLine(text)
            member this.WriteColorLine color text = context.WriteLine(color,text)
            member this.WriteEmptyLine() = context.WriteLine()
            member this.ProgressBar() = context.WriteProgressBar()}
