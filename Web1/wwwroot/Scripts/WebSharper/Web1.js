(function()
{
 "use strict";
 var Global,Web1,Client,Site,System,Guid,WebSharper,List,UI,Templating,Runtime,Server,Handler,TemplateInstance,Runtime$1,Var,Submitter,View,Remoting,AjaxRemotingProvider,Concurrency,Doc,AttrProxy;
 Global=window;
 Web1=Global.Web1=Global.Web1||{};
 Client=Web1.Client=Web1.Client||{};
 Site=Web1.Site=Web1.Site||{};
 System=Global.System;
 Guid=System&&System.Guid;
 WebSharper=Global.WebSharper;
 List=WebSharper&&WebSharper.List;
 UI=WebSharper&&WebSharper.UI;
 Templating=UI&&UI.Templating;
 Runtime=Templating&&Templating.Runtime;
 Server=Runtime&&Runtime.Server;
 Handler=Server&&Server.Handler;
 TemplateInstance=Server&&Server.TemplateInstance;
 Runtime$1=Server&&Server.Runtime;
 Var=UI&&UI.Var;
 Submitter=UI&&UI.Submitter;
 View=UI&&UI.View;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Doc=UI&&UI.Doc;
 AttrProxy=UI&&UI.AttrProxy;
 Client.Startup$53$19=function()
 {
  return function()
  {
   Global.alert("Clicked!");
  };
 };
 Client.Startup=function()
 {
  var p,p$1,$1,c,p$2;
  function f(a)
  {
   Global.alert("Clicked!");
  }
  p=(p$1=($1=[null],[$1,(c=Guid.NewGuid(),Global.String(c)),new List.T({
   $:1,
   $0:{
    $:5,
    $0:"click",
    $1:true,
    $2:function(el)
    {
     return function(ev)
     {
      return f({
       Vars:$1[0],
       Target:el,
       Event:ev
      });
     };
    }
   },
   $1:List.T.Empty
  })]),[p$1[0],p$1[1],new List.T({
   $:1,
   $0:{
    $:1,
    $0:"clicktext",
    $1:"Click me!"
   },
   $1:p$1[2]
  })]);
  p$2=Handler.CompleteHoles(p[1],p[2],[]);
  p[0][0]=new TemplateInstance.New(p$2[1],Runtime$1.RunTemplate(p$2[0]));
 };
 Client.Main=function()
 {
  var rvInput,submit,vReversed;
  rvInput=Var.Create$1("");
  submit=Submitter.CreateOption(rvInput.get_View());
  vReversed=View.MapAsync(function(a)
  {
   var b;
   return a!=null&&a.$==1?(new AjaxRemotingProvider.New()).Async("Web1:Web1.Server.DoSomething:-1840423385",[a.$0]):(b=null,Concurrency.Delay(function()
   {
    return Concurrency.Return("");
   }));
  },submit.view);
  return Doc.Element("div",[],[Doc.Input([],rvInput),Doc.Button("Send",[],function()
  {
   submit.Trigger();
  }),Doc.Element("hr",[],[]),Doc.Element("h4",[AttrProxy.Create("class","text-muted")],[Doc.TextNode("The server responded:")]),Doc.Element("div",[AttrProxy.Create("class","jumbotron")],[Doc.Element("h1",[],[Doc.TextView(vReversed)])])]);
 };
 Site.Main$66$31=function()
 {
  return function()
  {
   Client.Startup();
  };
 };
}());
