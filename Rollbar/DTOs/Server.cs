﻿namespace Rollbar.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Rollbar.Serialization.Json;

    /// <summary>
    /// Models Rollbar Server DTO.
    /// </summary>
    /// <seealso cref="Rollbar.DTOs.ExtendableDtoBase" />
    public class Server 
        : HostBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        public Server()
            : this(null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        /// <param name="arbitraryKeyValuePairs">The arbitrary key value pairs.</param>
        public Server(IDictionary<string, object> arbitraryKeyValuePairs) 
            : base(arbitraryKeyValuePairs)
        {
        }

        /// <summary>
        /// The DTO reserved properties.
        /// </summary>
        public static class ReservedProperties
        {
            /// <summary>
            /// The host
            /// </summary>
            public const string Host = "host";
            /// <summary>
            /// The root
            /// </summary>
            public const string Root = "root";
            /// <summary>
            /// The branch
            /// </summary>
            public const string Branch = "branch";
            /// <summary>
            /// The code version
            /// </summary>
            public const string CodeVersion = "code_version";
        }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host
        {
            get { return this._keyedValues[ReservedProperties.Host] as string; }
            set { this._keyedValues[ReservedProperties.Host] = value; }
        }

        /// <summary>
        /// Gets or sets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        public string Root
        {
            get { return this._keyedValues[ReservedProperties.Root] as string; }
            set { this._keyedValues[ReservedProperties.Root] = value; }
        }

        /// <summary>
        /// Gets or sets the branch.
        /// </summary>
        /// <value>
        /// The branch.
        /// </value>
        public string Branch
        {
            get { return this._keyedValues[ReservedProperties.Branch] as string; }
            set { this._keyedValues[ReservedProperties.Branch] = value; }
        }

        /// <summary>
        /// Gets or sets the code version.
        /// </summary>
        /// <value>
        /// The code version.
        /// </value>
        public string CodeVersion
        {
            get { return this._keyedValues[ReservedProperties.CodeVersion] as string; }
            set { this._keyedValues[ReservedProperties.CodeVersion] = value; }
        }
    }
}
