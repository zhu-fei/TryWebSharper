namespace Web4

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI
open WebSharper.UI.Server

type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/about">] About
    | [<EndPoint "/authenticated">] Authenticated
    | [<EndPoint "/login">] Login of EndPoint option
    | [<EndPoint "/logout">] Logout

module Templating =
    open WebSharper.UI.Html

    type MainTemplate = Templating.Template<"Main.html">

    // Compute a menubar where the menu item for the given endpoint is active
    let MenuBarAnon (ctx: Context<EndPoint>) endpoint : Doc list =
        let ( => ) txt act =
             li [if endpoint = act then yield attr.``class`` "active"] [
                a [attr.href (ctx.Link act)] [text txt]
             ]
        [
            "Home" => EndPoint.Home
            "Login" => EndPoint.Login 
            "About" => EndPoint.About
        ]

    let MenuBarLogged (ctx: Context<EndPoint>) endpoint : Doc list =
            [li [if endpoint = Profile then yield attr.``class`` "active"][a [attr.href (ctx.Link Profile)] [text "Profile"]];
             li [if endpoint = About then yield attr.``class`` "active"] [a [attr.href (ctx.Link About)] [text "About"]];
             li [if endpoint = Courses then yield attr.``class`` "active"] [a [attr.href (ctx.Link Courses)] [text "Courses"]];
             li [on.click (fun _ _ -> RegClient.LogOutUser())][a [attr.href "#"] [text "Log Out"]]
            ]    

    let Main ctx action (title: string) (body: Doc list) =
        Content.Page(
            MainTemplate()
                .Title(title)
                .MenuBar(MenuBar ctx action)
                .Body(body)
                .Doc()
        )

module Site =
    open WebSharper.UI.Html

    module Pages =
        let R url =
            url + "?d=" + System.Uri.EscapeUriString (System.DateTime.Now.ToString())

        let Links (ctx: Context<_>) =
            let ( => ) txt act = a [attr.href (ctx.Link act)] [text txt]
            let user = ctx.UserSession.GetLoggedInUser() |> Async.RunSynchronously
            ul [
                li ["Home" => EndPoint.Home]
                li ["Authenticated" => EndPoint.Authenticated]
                (if user.IsNone then
                    li ["Login" => EndPoint.Login None]
                else
                    li ["Logout" => EndPoint.Logout])
            ]

        let Home ctx =
            Content.Page(
                Title = "Home",
                Body = [Links ctx; h1 [text "Home page, use links above"]])

        let Authenticated ctx =
            Content.Page(
                Title = "Authenticated",
                Body = [Links ctx; h1 [text "This page requires a login!"]])

        let Logout ctx =
            Content.Page(
                Title = "Logout",
                Body = [Links ctx; h1 [text "You have been logged out."]])

        let Login ctx endpoint =
            let redirectUrl =
                match endpoint with
                | None -> EndPoint.Home
                | Some ep -> ep
                |> ctx.Link
                |> R
            Content.Page(
                Title = "Login",
                Body = [
                    h1 [text "Login"]
                    p [text "and... you are logged in magically..."]
                    aAttr [attr.href redirectUrl] [text "Proceed further"]
                ])


    let HomePage ctx =
        Templating.Main ctx EndPoint.Home "Home" [
            h1 [] [text "Say Hi to the server!"]
            div [] [client <@ Client.Main() @>]
        ]

    let AboutPage ctx =
        Templating.Main ctx EndPoint.About "About" [
            h1 [] [text "About"]
            p [] [text "This is my fouth step in websharper..."]
        ]

    [<Website>]
    let Main =
        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Home -> HomePage ctx
            | EndPoint.About -> AboutPage ctx
        )
