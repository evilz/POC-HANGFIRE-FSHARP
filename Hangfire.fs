namespace POC_HANGFIRE_FSHARP

open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Builder
open System
open Hangfire
open Hangfire.PostgreSql
open Hangfire.Console
open Hangfire.Dashboard


module Hangfire =
    
    type private MyAuthorizationFilter() =
        interface IDashboardAuthorizationFilter with 
            member this.Authorize (_: DashboardContext) = 
                true

    let configureService (connectionString:string) (services: IServiceCollection)  =
         services.AddHangfire(fun provider configuration -> 
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170) 
                         .UseSimpleAssemblyNameTypeSerializer()
                         .UseRecommendedSerializerSettings()
                         .UseConsole()
                         .UsePostgreSqlStorage(connectionString)
                         |> ignore
         ) |> ignore

    let configureApplication (app: IApplicationBuilder) = 
        // let dashboardOptions = DashboardOptions()
        // dashboardOptions.Authorization <- [| MyAuthorizationFilter() |] 
        // app.UseHangfireDashboard("/", dashboardOptions) |> ignore

        // let option = BackgroundJobServerOptions()
        // option.WorkerCount <- 1
        // app.UseHangfireServer(option) |> ignore

        app.UseHangfireServer() |> ignore
        app.UseHangfireDashboard() |> ignore
    
        RecurringJob.AddOrUpdate(Jobs.doSomething, Cron.Minutely)
        RecurringJob.AddOrUpdate( (fun () -> Jobs.doSomethingAsyncTask(null)) , Cron.Minutely)
