// --------------------------------------------------------------------------
// <copyright file="CustomViewEngine.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>

namespace Fanex.Razorbill.Module.Core.Web.ViewEngine
{
    using System.Web.Mvc;

    /// <summary>
    /// Custom View Engine.
    /// </summary>
    public class CustomViewEngine : RazorViewEngine
    {
        public CustomViewEngine()
            : base()
        {
            PartialViewLocationFormats = new string[]
                {
                    "~/Areas/{0}.cshtml"

                    ////Other areas
                };
        }
    }
}
