/*
 * This file is used by Code Analysis to maintain SuppressMessage
 * attributes that are applied to this project.
 * Project-level suppressions either have no target or are given
 * a specific target and scoped to a namespace, type, member, etc.
 */

#pragma warning disable SA1118 // Parameter should not span multiple lines

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.ReadabilityRules",
    "SA1124:Do not use regions",
    Justification =
        "Regions provide useful groupings of all tests for each method being " +
        "tested inside the given test file. It makes sure that all tests for a " +
        "method are kept together and makes it easier to debug, or see what is " +
        "being tested and what tests are missing.")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.DocumentationRules",
    "SA1600:Elements should be documented",
    Justification =
        "Test should be self-documenting.")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1201:Elements should appear in the correct order",
    Justification =
        "Test classes are organised differently to regular classes.")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.MaintainabilityRules",
    "SA1402:File may only contain a single type",
    Justification =
        "Multiple types may appear in the same in the case of test classes and test implementations.")]

#pragma warning restore SA1118 // Parameter should not span multiple lines