﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Super_Text_Adventure_Maker.Configuration {
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
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Super_Text_Adventure_Maker.Configuration.Strings", typeof(Strings).Assembly);
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
        ///   Looks up a localized string similar to action.
        /// </summary>
        internal static string General_Action {
            get {
                return ResourceManager.GetString("General_Action", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR.
        /// </summary>
        internal static string General_Error {
            get {
                return ResourceManager.GetString("General_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR in.
        /// </summary>
        internal static string General_ErrorIn {
            get {
                return ResourceManager.GetString("General_ErrorIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to file.
        /// </summary>
        internal static string General_File {
            get {
                return ResourceManager.GetString("General_File", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to scene.
        /// </summary>
        internal static string General_Scene {
            get {
                return ResourceManager.GetString("General_Scene", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome to SUPER TEXT ADVENTURE MAKER..
        /// </summary>
        internal static string Tools_Welcome {
            get {
                return ResourceManager.GetString("Tools_Welcome", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Press ENTER to continue.].
        /// </summary>
        internal static string UserInterface_PressEnterToContinue {
            get {
                return ResourceManager.GetString("UserInterface_PressEnterToContinue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to stam&gt;.
        /// </summary>
        internal static string UserInterface_StamPrompt {
            get {
                return ResourceManager.GetString("UserInterface_StamPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duplicate abbreviations:.
        /// </summary>
        internal static string Validation_DuplicateAbbreviations {
            get {
                return ResourceManager.GetString("Validation_DuplicateAbbreviations", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duplicate descriptions:.
        /// </summary>
        internal static string Validation_DuplicateActionDescriptions {
            get {
                return ResourceManager.GetString("Validation_DuplicateActionDescriptions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duplicate scene names:.
        /// </summary>
        internal static string Validation_DuplicateSceneNames {
            get {
                return ResourceManager.GetString("Validation_DuplicateSceneNames", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Entry scenes found in:.
        /// </summary>
        internal static string Validation_EntryScenes {
            get {
                return ResourceManager.GetString("Validation_EntryScenes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your adventure could not be built because of the above errors. Please fix them and try again..
        /// </summary>
        internal static string Validation_ErrorCannotBuild {
            get {
                return ResourceManager.GetString("Validation_ErrorCannotBuild", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A blank action abbreviation was found among other actions. If a blank action abbreviation is used, it must be the only action..
        /// </summary>
        internal static string Validation_ErrorDefaultActionNotIsolated {
            get {
                return ResourceManager.GetString("Validation_ErrorDefaultActionNotIsolated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duplicate action abbreviations were found in the scene. Each action should have a unique abbreviation..
        /// </summary>
        internal static string Validation_ErrorDuplicateAbbreviation {
            get {
                return ResourceManager.GetString("Validation_ErrorDuplicateAbbreviation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duplicate action descriptions were found in the scene. Each action should have a unique description..
        /// </summary>
        internal static string Validation_ErrorDuplicateActionDescriptions {
            get {
                return ResourceManager.GetString("Validation_ErrorDuplicateActionDescriptions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to More than one scene has the same name. Each scene should have a unique name..
        /// </summary>
        internal static string Validation_ErrorDuplicateSceneNames {
            get {
                return ResourceManager.GetString("Validation_ErrorDuplicateSceneNames", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The scene is an endless loop because all its actions use it as a next scene..
        /// </summary>
        internal static string Validation_ErrorInfiniteSceneLoop {
            get {
                return ResourceManager.GetString("Validation_ErrorInfiniteSceneLoop", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to More than one blank action abbreviation was found. If a blank action abbreviation is used, it must be the only action..
        /// </summary>
        internal static string Validation_ErrorMoreThanOneDefaultAction {
            get {
                return ResourceManager.GetString("Validation_ErrorMoreThanOneDefaultAction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have more than one entry scene. STAM doesn&apos;t know where to begin the adventure. An entry scene is a scene without a name..
        /// </summary>
        internal static string Validation_ErrorMultipleEntryScenes {
            get {
                return ResourceManager.GetString("Validation_ErrorMultipleEntryScenes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The action does not have a description..
        /// </summary>
        internal static string Validation_ErrorNoActionDescription {
            get {
                return ResourceManager.GetString("Validation_ErrorNoActionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The action does not have a result or a next scene..
        /// </summary>
        internal static string Validation_ErrorNoActionResultOrScene {
            get {
                return ResourceManager.GetString("Validation_ErrorNoActionResultOrScene", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You haven&apos;t created any entry scenes. STAM doesn&apos;t know where to begin the adventure. An entry scene is a scene without a name..
        /// </summary>
        internal static string Validation_ErrorNoEntryScene {
            get {
                return ResourceManager.GetString("Validation_ErrorNoEntryScene", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Some of the next scenes used in actions do not exist..
        /// </summary>
        internal static string Validation_ErrorNonExistentScenes {
            get {
                return ResourceManager.GetString("Validation_ErrorNonExistentScenes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Actions were found, but none of them has a next scene. If a scene has actions, at least one of them must have a next scene..
        /// </summary>
        internal static string Validation_ErrorNoNextScene {
            get {
                return ResourceManager.GetString("Validation_ErrorNoNextScene", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No scene description found..
        /// </summary>
        internal static string Validation_ErrorNoSceneDescription {
            get {
                return ResourceManager.GetString("Validation_ErrorNoSceneDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Scenes not found:.
        /// </summary>
        internal static string Validation_ScenesNotFound {
            get {
                return ResourceManager.GetString("Validation_ScenesNotFound", resourceCulture);
            }
        }
    }
}