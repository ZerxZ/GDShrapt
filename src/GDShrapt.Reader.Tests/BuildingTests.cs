﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GDShrapt.Reader.Tests
{
    [TestClass]
    public class BuildingTests
    {
        [TestMethod]
        public void ClassBuildingTest()
        {
            var declaration = GD.Declaration.Class(
                GD.List.Atributes(
                    GD.Atribute.Tool(),
                    GD.Atribute.ClassName("Generated"),
                    GD.Atribute.Extends("Node2D")),

                GD.Declaration.Const("my_constant", GD.Expression.String("Hello World")),
                GD.Declaration.Variable("parameter", GD.Expression.True()),

                GD.Declaration.Method("_start",
                    GD.Expression.Call(GD.Expression.Identifier("print"), GD.Expression.String("Hello world"))
                    )
                );

            declaration.UpdateIntendation();

            var code = declaration.ToString();

            var codeToCompare = "tool\nclass_name Generated\nextends Node2D\n\nconst my_constant = \"Hello World\"\n\nonready var parameter = true\n\nfunc _start():\n\tprint(\"Hello world\")";

            AssertHelper.CompareCodeStrings(codeToCompare, code);
        }

        [TestMethod]
        public void CustomStyleTest()
        {
            var declaration = GD.Declaration.Class()
                .AddAtributes(x => x
                    .AddToolAtribute()
                    .AddNewLine()
                    .AddClassNameAtribute("Generated")
                    .AddNewLine()
                    .AddExtendsAtribute("Node2D"))
                .AddNewLine()
                .AddNewLine()
                .AddMembers(x => x
                    .AddVariable("a")
                    .AddNewLine()
                    .AddConst("message", GD.Expression.String("Hello"))
                    .AddNewLine()
                    .AddNewLine()
                    .AddMethod(x => x
                        .AddFuncKeyword()
                        .AddSpace()
                        .Add("_start")
                        .AddOpenBracket()
                        .AddCloseBracket()
                        .AddStatements(x => x
                            .AddNewLine()
                            .AddNewLine()
                            .AddIntendation()
                            .AddCall(GD.Expression.Identifier("print"), GD.Expression.String("Hello world"))
                            .AddNewLine()
                            .AddNewLine()
                            .AddIntendation()
                            .AddPass())));

            declaration.UpdateIntendation();

            var code = declaration.ToString();

            var codeToCompare = "tool\nclass_name Generated\nextends Node2D\n\nvar a\nconst message = \"Hello\"\n\nfunc _start()\n\n\tprint(\"Hello world\")\n\n\tpass";

            AssertHelper.CompareCodeStrings(codeToCompare, code);
        }

        [TestMethod]
        public void CustomStyleTest2()
        {
            var declaration = GD.Declaration.Class(
                GD.List.Atributes(
                    GD.Atribute.Tool(),
                    GD.Syntax.NewLine,
                    GD.Atribute.ClassName("Generated"),
                    GD.Syntax.NewLine,
                    GD.Atribute.Extends("Node2D")),

                GD.Syntax.NewLine,
                GD.Syntax.NewLine,

                GD.Declaration.Variable(
                     GD.Keyword.Const,
                     GD.Syntax.OneSpace,
                     GD.Syntax.Identifier("my_constant"),
                     GD.Syntax.OneSpace,
                     GD.Syntax.Assign,
                     GD.Syntax.OneSpace,
                     GD.Syntax.String("Hello World")),

                GD.Syntax.NewLine,
                GD.Syntax.NewLine,

                GD.Declaration.Variable(
                    GD.Keyword.Onready,
                    GD.Syntax.OneSpace,
                    GD.Keyword.Var,
                    GD.Syntax.OneSpace,
                    GD.Syntax.Identifier("parameter"),
                    GD.Syntax.OneSpace,
                    GD.Syntax.Assign,
                    GD.Syntax.OneSpace,
                    GD.Expression.True()),

                GD.Syntax.NewLine,
                GD.Syntax.NewLine,

                GD.Declaration.Method(
                    GD.Keyword.Func,
                    GD.Syntax.OneSpace,
                    GD.Syntax.Identifier("_start"),
                    GD.Syntax.OpenBracket,
                    GD.Syntax.CloseBracket,
                    GD.Syntax.Colon,

                    GD.Syntax.NewLine,
                    GD.Syntax.Intendation(1),
                    GD.Expression.Call(
                        GD.Expression.Identifier("print"),
                        GD.Syntax.OpenBracket,
                        GD.List.Expressions(GD.Expression.String("Hello world")),
                        GD.Syntax.CloseBracket)));

            var code = declaration.ToString();

            var codeToCompare = "tool\nclass_name Generated\nextends Node2D\n\nconst my_constant = \"Hello World\"\n\nonready var parameter = true\n\nfunc _start():\n\tprint(\"Hello world\")";

            AssertHelper.CompareCodeStrings(codeToCompare, code);
        }
    }
}
