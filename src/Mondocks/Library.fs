namespace Mondocks

type IBuilder =
    abstract ToJSON: unit -> string

module internal Json =
    open System.Text.Json
    open System.Text.Json.Serialization

    let private defaults =
        let options = JsonSerializerOptions()
        options.Converters.Add(JsonFSharpConverter())
        options.IgnoreNullValues <- true
        options

    let Serialize<'T> (value: 'T) =
        JsonSerializer.Serialize<'T>(value, defaults)
