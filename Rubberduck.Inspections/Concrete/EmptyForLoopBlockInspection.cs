﻿using Antlr4.Runtime.Misc;
using Rubberduck.Inspections.Abstract;
using Rubberduck.Inspections.Results;
using Rubberduck.Parsing.Grammar;
using Rubberduck.Parsing.Inspections.Abstract;
using Rubberduck.Parsing.Inspections.Resources;
using Rubberduck.Parsing.VBA;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rubberduck.Inspections.Concrete
{
    internal class EmptyForLoopBlockInspection : ParseTreeInspectionBase
    {
        public EmptyForLoopBlockInspection(RubberduckParserState state)
            : base(state, CodeInspectionSeverity.Suggestion) { }

        public override Type Type => typeof(EmptyForLoopBlockInspection);

        public override CodeInspectionType InspectionType => CodeInspectionType.CodeQualityIssues;

        public override IEnumerable<IInspectionResult> GetInspectionResults()
        {
            return Listener.Contexts
                .Where(result => !IsIgnoringInspectionResultFor(result.ModuleName, result.Context.Start.Line))
                .Select(result => new QualifiedContextInspectionResult(this,
                                                        InspectionsUI.EmptyForLoopBlockInspectionResultFormat,
                                                        result));
        }

        public override IInspectionListener Listener { get; } =
            new EmptyForloopBlockListener();

        public class EmptyForloopBlockListener : EmptyBlockInspectionListenerBase
        {
            public override void EnterForNextStmt([NotNull] VBAParser.ForNextStmtContext context)
            {
                InspectBlockForExecutableStatements(context.block(), context);
            }
        }
    }
}
