namespace Mondocks.Fable

open Fable.Core.JS
open Fable.Core.JsInterop

module Json =
    let Serialize value = toPlainJsObj value |> JSON.stringify

    let ToCommand cmd = JSON.parse cmd
