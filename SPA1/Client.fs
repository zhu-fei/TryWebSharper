namespace Samples

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client

[<JavaScript>]
module HelloWorld =

    [<SPAEntryPoint>]
    let Main =
        let welcome = p [] [text "Hello!"]
        div [] [
            welcome
            button [
                on.click (fun _ _ ->
                    welcome.Text <- "Hello, world!")
            ] [
                text "Click Me!"
            ]
        ]
        |> Doc.RunById "main"