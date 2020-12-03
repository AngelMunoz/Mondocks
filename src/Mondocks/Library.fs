namespace Mondocks

open System.Text.RegularExpressions

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

    type DateTimeOffsetConverter() =
        inherit JsonConverter<DateTimeOffset>()

        override _.Read(reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            let deserialized =
                JsonSerializer.Deserialize<{| ``$date``: {| ``$numberLong``: string |} |}>(reader.GetString())

            DateTimeOffset.FromUnixTimeMilliseconds(deserialized.``$date``.``$numberLong`` |> int64)

        override _.Write(writer: Utf8JsonWriter, value: DateTimeOffset, options: JsonSerializerOptions) =
            let stringValue = value.ToUnixTimeMilliseconds() |> string

            writer.WriteStartObject()
            writer.WritePropertyName("$date")
            writer.WriteStartObject()
            writer.WritePropertyName("$numberLong")
            writer.WriteStringValue(stringValue)
            writer.WriteEndObject()
            writer.WriteEndObject()

    type Decimal128Converter() =
        inherit JsonConverter<Decimal128>()

        override _.Read(reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            let deserialized =
                JsonSerializer.Deserialize<{| ``$numberDecimal``: string |}>(reader.GetString())

            Decimal128.Parse deserialized.``$numberDecimal``

        override _.Write(writer: Utf8JsonWriter, value: Decimal128, options: JsonSerializerOptions) =
            let stringValue = value |> string

            writer.WriteStartObject()
            writer.WritePropertyName("$numberDecimal")
            writer.WriteStringValue(stringValue)
            writer.WriteEndObject()

    type DoubleConverter() =
        inherit JsonConverter<double>()

        override _.Read(reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            let deserialized =
                JsonSerializer.Deserialize<{| ``$numberDouble``: string |}>(reader.GetString())

            match deserialized.``$numberDouble`` with
            | "Infinity" -> Double.PositiveInfinity
            | "-Infinity" -> Double.NegativeInfinity
            | "NaN" -> Double.NaN
            | value -> Double.Parse value

        override _.Write(writer: Utf8JsonWriter, value: Double, options: JsonSerializerOptions) =
            let stringValue = value |> string

            writer.WriteStartObject()
            writer.WritePropertyName("$numberDouble")
            writer.WriteStringValue(stringValue)
            writer.WriteEndObject()

    type Int64Converter() =
        inherit JsonConverter<int64>()

        override _.Read(reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            let deserialized =
                JsonSerializer.Deserialize<{| ``$numberLong``: string |}>(reader.GetString())

            deserialized.``$numberLong`` |> int64

        override _.Write(writer: Utf8JsonWriter, value: int64, options: JsonSerializerOptions) =
            let stringValue = value |> string

            writer.WriteStartObject()
            writer.WritePropertyName("$numberLong")
            writer.WriteStringValue(stringValue)
            writer.WriteEndObject()


    type Int32Converter() =
        inherit JsonConverter<int32>()

        override _.Read(reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            let deserialized =
                JsonSerializer.Deserialize<{| ``$numberInt``: string |}>(reader.GetString())

            deserialized.``$numberInt`` |> int32

        override _.Write(writer: Utf8JsonWriter, value: int32, options: JsonSerializerOptions) =
            let stringValue = value |> string

            writer.WriteStartObject()
            writer.WritePropertyName("$numberInt")
            writer.WriteStringValue(stringValue)
            writer.WriteEndObject()

    type RegexConverter() =
        inherit JsonConverter<Regex>()

        override _.Read(reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            let deserialized =
                JsonSerializer.Deserialize<{| ``$regularExpression``: {| ``$pattern``: string
                                                                         ``$options``: string |} |}>
                    (reader.GetString())

            match deserialized.``$regularExpression``.``$options``
                  |> ValueOption.ofObj with
            | ValueSome options ->
                Regex
                    (deserialized.``$regularExpression``.``$pattern``,
                     RegexOptions.Parse(typeof<RegexOptions>, options) :?> RegexOptions)
            | ValueNone -> Regex(deserialized.``$regularExpression``.``$pattern``)

        override _.Write(writer: Utf8JsonWriter, value: Regex, options: JsonSerializerOptions) =
            let stringValue = value |> string

            writer.WriteStartObject() //
            writer.WritePropertyName("$regularExpression")
            writer.WriteStartObject() // //
            writer.WritePropertyName("$pattern") // //
            writer.WriteStringValue(value.ToString()) // //
            writer.WriteEndObject() // //
            writer.WriteStartObject() // //
            writer.WritePropertyName("$options") // //
            writer.WriteStringValue(value.Options.ToString()) // //
            writer.WriteEndObject() // //
            writer.WriteEndObject() //



    let private defaults =
        let options = JsonSerializerOptions()
        options.Converters.Add(JsonFSharpConverter())
        options.Converters.Add(ObjectIdConverter())
        options.Converters.Add(DateTimeConverter())
        options.Converters.Add(DateTimeOffsetConverter())
        options.Converters.Add(Decimal128Converter())
        options.Converters.Add(DoubleConverter())
        options.Converters.Add(Int64Converter())
        options.Converters.Add(Int32Converter())
        options.Converters.Add(RegexConverter())
        options.IgnoreNullValues <- true
        options

    let Serialize<'T> (value: 'T) =
        JsonSerializer.Serialize<'T>(value, defaults)
