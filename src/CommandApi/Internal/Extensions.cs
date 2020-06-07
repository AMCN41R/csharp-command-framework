namespace CommandApi.Internal
{
    using System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Useful extension methods.
    /// </summary>
    internal static class Extensions
    {
        private static JsonSerializerSettings DefaultSettings =>
            new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

        /// <summary>
        /// Serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string? ToJson(this object value)
        {
            return value.ToJson(DefaultSettings);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string using the given settings.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="settings">The serializer settings to use.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string? ToJson(this object value, JsonSerializerSettings settings)
        {
            return value == null
                ? null
                : JsonConvert.SerializeObject(value, settings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The string to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static T? FromJson<T>(this string value)
            where T : class
        {
            return string.IsNullOrWhiteSpace(value)
                ? default
                : JsonConvert.DeserializeObject<T>(value, DefaultSettings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <param name="value">The string to deserialize.</param>
        /// <param name="type">The type of the object to deserialize to.</param>
        /// <returns>The deserialized object.</returns>
        public static object? FromJson(this string value, Type type)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : JsonConvert.DeserializeObject(value, type, DefaultSettings);
        }

        /// <summary>
        /// Attempts to convert the given string to a boolean.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="defaultValue">The value to return if the conversion fails.</param>
        /// <returns>
        /// If the conversion succeeds, the converted value is returned.
        /// If the conversion fails, the given default value is returned.
        /// </returns>
        public static bool TryParseBool(this string str, bool defaultValue = false)
        {
            return bool.TryParse(str, out var result) ? result : defaultValue;
        }
    }
}
