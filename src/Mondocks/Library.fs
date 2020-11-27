namespace Mondocks

type IBuilder =
    abstract ToJSON: unit -> string

module internal Json =
    open System
    open System.Text.Json
    open System.Text.Json.Serialization
    open MongoDB.Bson

    type ObjectIdConverter() =
        inherit JsonConverter<ObjectId>()

        override _.Read(reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            ObjectId.Parse(reader.GetString())

        override _.Write(writer: Utf8JsonWriter, value: ObjectId, options: JsonSerializerOptions) =
            writer.WriteStringValue(value.ToString())

    let private defaults =
        let options = JsonSerializerOptions()
        options.Converters.Add(JsonFSharpConverter())
        options.Converters.Add(ObjectIdConverter())
        options.IgnoreNullValues <- true
        options

    let Serialize<'T> (value: 'T) =
        JsonSerializer.Serialize<'T>(value, defaults)
