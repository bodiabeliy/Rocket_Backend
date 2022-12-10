namespace RetrievePlugin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Level
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.
        public string? ImageUrl { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.

        public int? CategoryCode { get; set; }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.
        public string? Category { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.
        public string? SvgXML { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.
        public string? SvgWidth { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.
        public string? SvgHeight { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.
        public string? SvgViewBox { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.
        public string? SvgPreserveAspectRatio { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.
        public string? SvgSchemaJsonUrl { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.
        public string? ParentCategory { get; set; }

        public string? JsonData { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' context.
    }
}
