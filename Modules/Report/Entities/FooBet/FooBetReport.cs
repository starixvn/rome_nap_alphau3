// --------------------------------------------------------------------------
// <copyright file="FooBetReport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.22">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Report.Entities.FooBet
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// FooBetReport class.
    /// </summary>
    [Serializable]
    public class FooBetReport
    {
        public string ReportTitle { get; set; }

        public IList<FooBetDTO> FooBets { get; set; }

        public FooBetDTO Total { get; set; }
    }
}