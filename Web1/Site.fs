namespace Web1

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI
open WebSharper.UI.Server
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.JavaScript
open WebSharper.UI.Html

type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/about">] About



module Templating =

    type MainTemplate = Template<"Main.html", ClientLoad.FromDocument>
    // Compute a menubar where the menu item for the given endpoint is active
    let MenuBar (ctx: Context<EndPoint>) endpoint : Doc list =
        let ( => ) txt act =
             li [if endpoint = act then yield attr.``class`` "active"] [
                a [attr.href (ctx.Link act)] [text txt]
             ]
        [
            "Home" => EndPoint.Home
            "About" => EndPoint.About
        ]

[<JavaScript>]
module Client =

    let Main () =
        let rvInput = Var.Create ""
        let submit = Submitter.CreateOption rvInput.View
        let vReversed =
            submit.View.MapAsync(function
                | None -> async { return "" }
                | Some input -> Server.DoSomething input
            )
        div [] [
            Doc.Input [] rvInput
            Doc.Button "Send" [] submit.Trigger
            hr [] []
            h4 [attr.``class`` "text-muted"] [text "The server responded:"]
            div [attr.``class`` "jumbotron"] [h1 [] [textView vReversed]]
        ]

    let Startup() =
        Templating.MainTemplate()
            .Click(fun _ -> JS.Alert "Clicked!")
            .ClickText("Click me!")
            .Bind()

module Site =

    let Main ctx action (title: string) (body: Doc list) =
        Content.Page(
            Templating.MainTemplate()
                .Title(title)
                .MenuBar(Templating.MenuBar ctx action)
                .Body(body)
                .Elt(keepUnfilled = true)
                .OnAfterRender(fun _ -> Client.Startup())
        )
    
    let HomePage ctx =
        Main ctx EndPoint.Home "Home" [
            h1 [] [text "Say Hi to the server!"]
            div [] [client <@ Client.Main() @>]
        ]

    let AboutPage ctx =
        Main ctx EndPoint.About "About" [
            h1 [] [text "About"]
            p [] [text "This is a template WebSharper client-server application."]
        ]

    [<Website>]
    let MySite =
        Sitelet.Infer <| fun ctx endpoint ->
            match endpoint with
            | EndPoint.Home -> HomePage ctx
            | EndPoint.About -> AboutPage ctx
