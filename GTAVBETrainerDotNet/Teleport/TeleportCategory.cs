﻿//////////////////////////////////////////////
//   BE Trainer.NET for Grand Theft Auto V
//             by BE.Tenner
//      Copyright (c) BE Group 2015
//               Thanks to
//    ScriptHookV & ScriptHookVDotNet
//  Native Trainer & Enhanced Native Trainer
//////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVBETrainerDotNet.Teleport
{
    /// <summary>
    /// Teleport category
    /// </summary>
    public class TeleportCategory
    {
        /// <summary>
        /// Name of this category
        /// </summary>
        public MString Name = null;

        /// <summary>
        /// Teleport targets
        /// </summary>
        public TeleportTarget[] Targets = null;

        /// <summary>
        /// Creates a teleport category
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="targets">Targets</param>
        public TeleportCategory(MString name, TeleportTarget[] targets)
        {
            Name = name;
            Targets = targets;
        }
    }
}
