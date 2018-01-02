﻿namespace Rollbar.DTOs
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Rollbar.Diagnostics;

    /// <summary>
    /// Models Rollbar Data DTO.
    /// </summary>
    /// <seealso cref="Rollbar.DTOs.DtoBase" />
    public class Data
        : DtoBase
    {
        private static readonly string NotifierAssemblyVersion = 
            typeof(Data).Assembly.GetName().Version.ToString(3);

        /// <summary>
        /// Gets or sets the default platform.
        /// </summary>
        /// <value>
        /// The default platform.
        /// </value>
        public static string DefaultPlatform { get; set; } = "windows";

        /// <summary>
        /// Gets or sets the default language.
        /// </summary>
        /// <value>
        /// The default language.
        /// </value>
        public static string DefaultLanguage { get; set; } = "c#";

        /// <summary>
        /// Initializes a new instance of the <see cref="Data"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="body">The body.</param>
        public Data(string environment, Body body)
        {
            Assumption.AssertNotNullOrWhiteSpace(environment, nameof(environment));
            Assumption.AssertNotNull(body, nameof(body));

            Environment = environment;
            Body = body;
            Timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            Platform = DefaultPlatform;
            Language = DefaultLanguage;
        }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        [JsonProperty("environment", Required = Required.Always)]
        public string Environment { get; private set; }

        /// <summary>
        /// Gets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        [JsonProperty("body", Required = Required.Always)]
        public Body Body { get; private set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        [JsonProperty("level", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ErrorLevel? Level { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        [JsonProperty("timestamp", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the code version.
        /// </summary>
        /// <value>
        /// The code version.
        /// </value>
        [JsonProperty("code_version", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CodeVersion { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>
        /// The platform.
        /// </value>
        [JsonProperty("platform", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        [JsonProperty("language", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the framework.
        /// </summary>
        /// <value>
        /// The framework.
        /// </value>
        [JsonProperty("framework", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        [JsonProperty("context", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets the request.
        /// </summary>
        /// <value>
        /// The request.
        /// </value>
        [JsonProperty("request", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Request Request { get; set; }

        /// <summary>
        /// Gets or sets the person.
        /// </summary>
        /// <value>
        /// The person.
        /// </value>
        [JsonProperty("person", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Person Person { get; set; }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        [JsonProperty("server", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Server Server { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        [JsonProperty("client", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Client Client { get; set; }

        /// <summary>
        /// Gets or sets the custom.
        /// </summary>
        /// <value>
        /// The custom.
        /// </value>
        [JsonProperty("custom", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, object> Custom { get; set; }

        /// <summary>
        /// Gets or sets the fingerprint.
        /// </summary>
        /// <value>
        /// The fingerprint.
        /// </value>
        [JsonProperty("fingerprint", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Fingerprint { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [JsonProperty("title", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the UUID.
        /// </summary>
        /// <value>
        /// The UUID.
        /// </value>
        [JsonProperty("uuid", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Uuid { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier UUID.
        /// </summary>
        /// <value>
        /// The unique identifier UUID.
        /// </value>
        [JsonIgnore]
        public Guid? GuidUuid
        {
            get { return Uuid == null ? (Guid?)null : Guid.Parse(Uuid); }
            set { Uuid = value == null ? null : value.Value.ToString("N"); }
        }

        /// <summary>
        /// Gets the notifier.
        /// </summary>
        /// <value>
        /// The notifier.
        /// </value>
        [JsonProperty("notifier", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, string> Notifier
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "name", "Rollbar.NET" },
                    { "version", NotifierAssemblyVersion },
                };
            }
        }
    }
}