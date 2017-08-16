using System;
using System.Reflection;

namespace genBTC.FileTime
{
    /// <summary>
    /// This class uses the System.Reflection.Assembly class to
    /// access assembly meta-data
    /// </summary>
    public class AssemblyVersion
    {
        #region Constructor...

        // Used by Helper Functions to access information from Assembly Attributes
        private readonly Type assemblyType;

        /// <summary>
        /// Constructor
        /// </summary>
        public AssemblyVersion()
        {
            assemblyType = GetType();
        }

        #endregion Constructor...

        #region Assembly Attributes...

        /// <summary>
        /// Describes an assembly's unique identity in full.
        /// </summary>
        public string AssemblyName
        {
            get { return assemblyType.Assembly.GetName().Name; }
        }

        /// <summary>
        /// Gets the full name of the assembly, also known as the display name.
        /// </summary>
        public string AssemblyFullName
        {
            get { return assemblyType.Assembly.GetName().FullName; }
        }

        /// <summary>
        /// Gets the location of the assembly as specified originally, for example, in an AssemblyName object.
        /// </summary>
        public string CodeBase
        {
            get { return assemblyType.Assembly.CodeBase; }
        }

        /// <summary>
        /// Defines a copyright custom attribute for an assembly manifest.
        /// </summary>
        public string Copyright
        {
            get
            {
                var at = typeof(AssemblyCopyrightAttribute);
                var r = assemblyType.Assembly.GetCustomAttributes(at, false);
                var ct = (AssemblyCopyrightAttribute)r[0];
                return ct.Copyright;
            }
        }

        /// <summary>
        /// Defines a company name custom attribute for an assembly manifest.
        /// </summary>
        public string Company
        {
            get
            {
                var at = typeof(AssemblyCompanyAttribute);
                var r = assemblyType.Assembly.GetCustomAttributes(at, false);
                var ct = (AssemblyCompanyAttribute)r[0];
                return ct.Company;
            }
        }

        /// <summary>
        /// Defines an assembly description custom attribute for an assembly manifest.
        /// </summary>
        public string Description
        {
            get
            {
                var at = typeof(AssemblyDescriptionAttribute);
                var r = assemblyType.Assembly.GetCustomAttributes(at, false);
                var da = (AssemblyDescriptionAttribute)r[0];
                return da.Description;
            }
        }

        /// <summary>
        /// Defines a product name custom attribute for an assembly manifest.
        /// </summary>
        public string Product
        {
            get
            {
                var at = typeof(AssemblyProductAttribute);
                var r = assemblyType.Assembly.GetCustomAttributes(at, false);
                var pt = (AssemblyProductAttribute)r[0];
                return pt.Product;
            }
        }

        /// <summary>
        /// Defines an assembly title custom attribute for an assembly manifest.
        /// </summary>
        public string Title
        {
            get
            {
                var at = typeof(AssemblyTitleAttribute);
                var r = assemblyType.Assembly.GetCustomAttributes(at, false);
                var ta = (AssemblyTitleAttribute)r[0];
                return ta.Title;
            }
        }

        /// <summary>
        /// Defines a trademark custom attribute for an assembly manifest.
        /// </summary>
        public string Trademark
        {
            get
            {
                var at = typeof(AssemblyTrademarkAttribute);
                var r = assemblyType.Assembly.GetCustomAttributes(at, false);
                var ta = (AssemblyTrademarkAttribute)r[0];
                return ta.Trademark;
            }
        }

        /// <summary>
        /// Gets the major, minor, revision, and build numbers of the assembly.
        /// </summary>
        public string Version
        {
            get { return assemblyType.Assembly.GetName().Version.ToString(); }
        }

        /// <summary>
        /// Gets the major, minor, revision, and build numbers of the assembly.
        /// </summary>
        public string VersionDetails
        {
            get
            {
                return assemblyType.Assembly.GetName().Version.Major + "." +
                       assemblyType.Assembly.GetName().Version.Minor + " (Build: " +
                       assemblyType.Assembly.GetName().Version.Build + ") (Revision: " +
                       assemblyType.Assembly.GetName().Version.Revision + ")";
            }
        }

        #endregion Assembly Attributes...

        #region Display Functions...

        /// <summary>
        /// Returns a string with the assembly version attributes
        /// </summary>
        /// <returns>String with the assembly version attributes</returns>
        public override string ToString()
        {
            string str;
            str = "Title: " + Title + "\r\n\r\n";
            str += "Assembly Name: " + AssemblyName + "\r\n\r\n";
            str += "Assembly Full Name: " + AssemblyFullName + "\r\n\r\n";
            str += "CodeBase: " + CodeBase + "\r\n\r\n";
            str += "Copyright: " + Copyright + "\r\n\r\n";
            str += "Company: " + Company + "\r\n\r\n";
            str += "Description: " + Description + "\r\n\r\n";
            str += "Product: " + Product + "\r\n\r\n";
            str += "Version: " + Version + "\r\n\r\n";
            str += "Version: " + VersionDetails + "\r\n\r\n";
            return str;
        }

        /// <summary>
        /// Returns a HTML format string with the assembly version attributes
        /// </summary>
        /// <returns>HTML format string with the assembly version attributes</returns>
        public string ToHTML()
        {
            string str;
            str = "<b>Title: " + Title + "</b><P></P>";
            str += "<li><b>Assembly Name: </b>" + AssemblyName;
            str += "<li><b>Assembly Full Name: </b>" + AssemblyFullName;
            str += "<li><b>CodeBase: </b>" + CodeBase;
            str += "<li><b>Copyright: </b>" + Copyright;
            str += "<li><b>Company: </b>" + Company;
            str += "<li><b>Description: </b>" + Description;
            str += "<li><b>Product: </b>" + Product;
            str += "<li><b>Trademark: </b>" + Trademark;
            str += "<li><b>Version: </b>" + Version;
            str += "<li><b>Version: </b>" + VersionDetails;
            return str;
        }

        #endregion Display Functions...
    }
}