using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvG_Abgabe_1___Webapp
{
    public class Constants
{
        // HATEAOS constants
        public const string HREF = "href";
        public const string SELF = "self";
        public const string METHOD = "method";
        public const string ADD = "add";
        public const string LIST = "list";
        public const string REL = "rel";
        public const string UPDATE = "update";
        public const string REMOVE = "remove";

        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string PATCH = "PATCH";
        public const string DELETE = "DELETE";

        // VERBESSERUNG: Templatestrings benutzen
        public const string NAME_REGEX = "[A-ZÄÖÜ][a-zäöüß]+(-[A-ZÄÖÜ][a-zäöüß]+)?";
        public const string ID_REGEX = "[\\dA-Fa-f]{8}-[\\dA-Fa-f]{4}-[\\dA-Fa-f]{4}-[\\dA-Fa-f]{4}-[\\dA-Fa-f]{12}";

    }
}
