﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShaRide.Application.Localize {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resource_en {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource_en() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ShaRide.Application.Localize.Resource.en", typeof(Resource_en).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; already exists.
        /// </summary>
        public static string Exception_AlreadyExists {
            get {
                return ResourceManager.GetString("Exception.AlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid Credentials for &apos;{0}&apos;..
        /// </summary>
        public static string Exception_InvalidCredentials {
            get {
                return ResourceManager.GetString("Exception.InvalidCredentials", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; not found.
        /// </summary>
        public static string Exception_NotFound {
            get {
                return ResourceManager.GetString("Exception.NotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is already confirmed.
        /// </summary>
        public static string Exception_PhoneConfirmed {
            get {
                return ResourceManager.GetString("Exception.PhoneConfirmed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User not found with phone &apos;{0}&apos; .
        /// </summary>
        public static string Exception_PhoneNotFound {
            get {
                return ResourceManager.GetString("Exception.PhoneNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Phone &apos;{0}&apos; is already taken..
        /// </summary>
        public static string Exception_PhoneTaken {
            get {
                return ResourceManager.GetString("Exception.PhoneTaken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Restriction with &apos;{0}&apos; id was not found.
        /// </summary>
        public static string Exception_RestrictionNotFound {
            get {
                return ResourceManager.GetString("Exception.RestrictionNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ride with &apos;{0}&apos; id was not found..
        /// </summary>
        public static string Exception_RideNotFound {
            get {
                return ResourceManager.GetString("Exception.RideNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password is to weak.
        /// </summary>
        public static string Validation_Password {
            get {
                return ResourceManager.GetString("Validation.Password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Phone Format is invalid.
        /// </summary>
        public static string Validation_Phone {
            get {
                return ResourceManager.GetString("Validation.Phone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; field can only be between {1} - {2}..
        /// </summary>
        public static string Validation_Range {
            get {
                return ResourceManager.GetString("Validation.Range", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; field cannot be empty..
        /// </summary>
        public static string Validation_Required {
            get {
                return ResourceManager.GetString("Validation.Required", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One hour before the start time, the Ride cannot be canceled.
        /// </summary>
        public static string Validation_RideCannotCancelled {
            get {
                return ResourceManager.GetString("Validation.RideCannotCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User does not have enough balance for this operation.
        /// </summary>
        public static string Validation_UserDoesNotHaveEnoughBalance {
            get {
                return ResourceManager.GetString("Validation.UserDoesNotHaveEnoughBalance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This user does not have access to do this access..
        /// </summary>
        public static string Validation_UserHasNotAccessToOperation {
            get {
                return ResourceManager.GetString("Validation.UserHasNotAccessToOperation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your ShaRide code : {0}.
        /// </summary>
        public static string VerificationSms_Confirmation {
            get {
                return ResourceManager.GetString("VerificationSms.Confirmation", resourceCulture);
            }
        }
    }
}
