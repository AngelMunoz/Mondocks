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
            writer.WriteStartObject()
            writer.WritePropertyName("$oid")
            writer.WriteStringValue(value.ToString())
            writer.WriteEndObject()

    type DateTimeConverter() =
        inherit JsonConverter<DateTime>()

        override _.Read(reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            let deserialized =
                JsonSerializer.Deserialize<{| ``$date``: {| ``$numberLong``: string |} |}>(reader.GetString())

            DateTimeOffset
                .FromUnixTimeMilliseconds(deserialized.``$date``.``$numberLong`` |> int64)
                .Date

        override _.Write(writer: Utf8JsonWriter, value: DateTime, options: JsonSerializerOptions) =
            let stringValue =
                DateTimeOffset
                    .op_Implicit(value)
                    .ToUnixTimeMilliseconds()
                |> string

            writer.WriteStartObject()
            writer.WritePropertyName("$date")
            writer.WriteStartObject()
            writer.WritePropertyName("$numberLong")
            writer.WriteStringValue(stringValue)
            writer.WriteEndObject()
            writer.WriteEndObject()

    let private defaults =
        let options = JsonSerializerOptions()
        options.Converters.Add(JsonFSharpConverter())
        options.Converters.Add(ObjectIdConverter())
        options.Converters.Add(DateTimeConverter())
        options.IgnoreNullValues <- true
        options

    let Serialize<'T> (value: 'T) =
        JsonSerializer.Serialize<'T>(value, defaults)
