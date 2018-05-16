namespace SPA2

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Templating

[<JavaScript>]
module Client =

    [<SPAEntryPoint>]
    let Main () =
        button [
            attr.id "my-button"
            on.click (fun el ev ->
                JS.Alert (sprintf "You clicked %s at x = %i, y = %i." 
                    el.Id ev.ClientX ev.ClientY)
                )
            ] [ text "Click me!" ]
        |> Doc.RunById "main"
