﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DerivativeCalcFinal {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Rules {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Rules() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DerivativeCalcFinal.Rules", typeof(Rules).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro absolutní hodnotu platí: [|x|]&apos; = x/|x| * [x]&apos; :.
        /// </summary>
        internal static string abs {
            get {
                return ResourceManager.GetString("abs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro sčítání platí: f + g = f&apos; + g&apos;, tj.:.
        /// </summary>
        internal static string addition {
            get {
                return ResourceManager.GetString("addition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro arccos platí: [arccos(x)]&apos; = (-1)/(sqrt(1 - x^2)).
        /// </summary>
        internal static string arccos {
            get {
                return ResourceManager.GetString("arccos", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro arccotan platí: [arccotan(x)]&apos; = (-1)/(1 + x^2).
        /// </summary>
        internal static string arccotg {
            get {
                return ResourceManager.GetString("arccotg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro arcsin platí: [arcsin(x)]&apos; = 1/(sqrt(1 - x^2)).
        /// </summary>
        internal static string arcsin {
            get {
                return ResourceManager.GetString("arcsin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro arctan platí: [arctan(x)]&apos; = 1/(1 + x^2).
        /// </summary>
        internal static string arctg {
            get {
                return ResourceManager.GetString("arctg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Jde o konstantu, zderivuje se tedy na 0..
        /// </summary>
        internal static string constant {
            get {
                return ResourceManager.GetString("constant", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Jelikož [cos(x)]&apos; = -sin(x) * [x]&apos; :.
        /// </summary>
        internal static string cos {
            get {
                return ResourceManager.GetString("cos", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro cotan platí: [cotan(x)]&apos; = (-1)/sin(x)^2.
        /// </summary>
        internal static string cotg {
            get {
                return ResourceManager.GetString("cotg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Jde o derivační proměnnou, zderivuje se tedy na 1..
        /// </summary>
        internal static string diffvariable {
            get {
                return ResourceManager.GetString("diffvariable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro dělení platí: [f / g]&apos; = ( f&apos; * g - f * g&apos; ) / (g^2)  :.
        /// </summary>
        internal static string division {
            get {
                return ResourceManager.GetString("division", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro přirozený logaritmus platí: (1/x) * [x]&apos;.
        /// </summary>
        internal static string ln {
            get {
                return ResourceManager.GetString("ln", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro násobení platí: [f * g]&apos; = f&apos; * g + f * g&apos;  :.
        /// </summary>
        internal static string multiplication {
            get {
                return ResourceManager.GetString("multiplication", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro mocninu platí: [x^y]&apos; = x^y * [ln(x) * y]&apos;.
        /// </summary>
        internal static string power {
            get {
                return ResourceManager.GetString("power", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro sinus platí: [sin(x)]&apos; = cos(x) * [x]&apos;.
        /// </summary>
        internal static string sin {
            get {
                return ResourceManager.GetString("sin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro odčítání platí: [f - g]&apos; = f&apos; - g&apos;  :.
        /// </summary>
        internal static string substraction {
            get {
                return ResourceManager.GetString("substraction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro tangens platí: [tan(x)]&apos; = 1/cos(x)^2.
        /// </summary>
        internal static string tg {
            get {
                return ResourceManager.GetString("tg", resourceCulture);
            }
        }
    }
}
