﻿namespace Rollbar.DTOs
{
    using Rollbar.Common;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Implements network tlemetry body.
    /// </summary>
    /// <seealso cref="Rollbar.DTOs.TelemetryBody" />
    public class NetworkTelemetry
        : TelemetryBody
    {
        /// <summary>
        /// The DTO reserved properties.
        /// </summary>
        public static class ReservedProperties
        {
            public const string Subtype = "subtype";
            public const string Method = "method";
            public const string Url = "url";
            public const string StatusCode = "status_code";
            public const string StartTimestamp = "start_timestamp_ms";
            public const string EndTimestamp = "end_timestamp_ms";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkTelemetry" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="eventStart">The event start.</param>
        /// <param name="eventEnd">The event end.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="subtype">The subtype.</param>
        /// <param name="arbitraryKeyValuePairs">The arbitrary key value pairs.</param>
        public NetworkTelemetry(
            string method
            , string url
            , DateTime? eventStart = null
            , DateTime? eventEnd = null
            , int? statusCode = null
            , string subtype = null
            , IDictionary<string, object> arbitraryKeyValuePairs = null
            )
            : base(TelemetryType.Network, arbitraryKeyValuePairs)
        {
            this.Method = method;
            this.Url = url;
            this.StatusCode = statusCode.HasValue ? $"{statusCode}" : null;

            if (eventStart.HasValue)
            {
                this.StartTimestamp = DateTimeUtil.ConvertToUnixTimestampInMilliseconds(eventStart.Value);
            }
            else
            {
                this.StartTimestamp = DateTimeUtil.ConvertToUnixTimestampInMilliseconds(DateTime.UtcNow);
            }

            if (eventEnd.HasValue)
            {
                this.EndTimestamp = DateTimeUtil.ConvertToUnixTimestampInMilliseconds(eventEnd.Value);
            }

            if (!string.IsNullOrWhiteSpace(subtype))
            {
                this.Subtype = subtype;
            }
        }

        /// <summary>
        /// Finalizes the event.
        /// </summary>
        public void FinalizeEvent()
        {
            if (!this.EndTimestamp.HasValue) // should not be able to finalize more than once...
            {
                this.EndTimestamp = DateTimeUtil.ConvertToUnixTimestampInMilliseconds(DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Gets the subtype.
        /// </summary>
        /// <value>
        /// The subtype.
        /// </value>
        public string Subtype
        {
            get { return this[ReservedProperties.Subtype] as string; }
            private set { this[ReservedProperties.Subtype] = value; }
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public string Method
        {
            get { return this[ReservedProperties.Method] as string; }
            private set { this[ReservedProperties.Method] = value; }
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url
        {
            get { return this[ReservedProperties.Url] as string; }
            private set { this[ReservedProperties.Url] = value; }
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public string StatusCode
        {
            get { return this[ReservedProperties.StatusCode] as string; }
            set { this[ReservedProperties.StatusCode] = value; }
        }

        /// <summary>
        /// Gets the start timestamp.
        /// </summary>
        /// <value>
        /// The start timestamp.
        /// </value>
        public long StartTimestamp
        {
            get { return (long) this[ReservedProperties.StartTimestamp]; }
            private set { this[ReservedProperties.StartTimestamp] = value; }
        }

        /// <summary>
        /// Gets the end timestamp.
        /// </summary>
        /// <value>
        /// The end timestamp.
        /// </value>
        public long? EndTimestamp
        {
            get { return this[ReservedProperties.EndTimestamp] as long?; }
            private set { this[ReservedProperties.EndTimestamp] = value; }
        }

    }
}
