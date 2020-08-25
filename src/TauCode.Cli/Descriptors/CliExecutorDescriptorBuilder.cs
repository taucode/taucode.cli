using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli.Descriptors
{
    public class CliExecutorDescriptorBuilder
    {
        private readonly PseudoList _form;

        public CliExecutorDescriptorBuilder(string workerGrammar)
        {
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(workerGrammar);
            ITinyLispPseudoReader reader = new TinyLispPseudoReader();
            _form = reader.Read(tokens);
        }

        public CliExecutorDescriptor Build()
        {
            var topDefblock = _form.Single(x => x.GetSingleArgumentAsBool(":is-top") ?? false);

            var supposedCommandForm = topDefblock.GetFreeArguments().First();

            if (supposedCommandForm.GetCarSymbolName().ToLowerInvariant() != "worker")
            {
                throw new CliException($"'worker' symbol was expected.");
            }

            var name = supposedCommandForm.GetSingleKeywordArgument<Symbol>(":worker-name", true)?.Name;
            var verb = supposedCommandForm.GetSingleKeywordArgument<StringAtom>(":verb", true)?.Value;
            var description = supposedCommandForm.GetSingleKeywordArgument<StringAtom>(":description", true)?.Value;
            var usageSamples = supposedCommandForm.GetSingleKeywordArgument(":usage-samples", true)?
                .AsPseudoList()?
                .Cast<StringAtom>()
                .Select(x => x.Value)
                .ToList();

            var keyList = new List<CliExecutorKeyDescriptor>();
            var argumentList = new List<CliExecutorArgumentDescriptor>();
            var optionList = new List<CliExecutorOptionDescriptor>();

            this.CollectItems(
                topDefblock.AsPseudoList().GetFreeArguments(),
                keyList,
                argumentList,
                optionList);

            var descriptor = new CliExecutorDescriptor(
                name,
                verb,
                description,
                usageSamples,
                keyList,
                argumentList,
                optionList);

            return descriptor;
        }

        private void CollectItems(
            PseudoList list,
            List<CliExecutorKeyDescriptor> keyList,
            List<CliExecutorArgumentDescriptor> argList,
            List<CliExecutorOptionDescriptor> optionList)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var subForm = list[i].AsPseudoList();
                var subFormCar = subForm.GetCarSymbolName().ToLowerInvariant();

                string action;

                switch (subFormCar)
                {
                    case "multi-text":
                        action = subForm.GetSingleKeywordArgument<Symbol>(":action").Name.ToLowerInvariant();

                        switch (action)
                        {
                            case "key":
                                var keyDescriptor = this.ExtractKeyDescriptor(list, i);
                                keyList.Add(keyDescriptor);
                                i++;
                                break;

                            case "option":
                                var optionDescriptor = this.ExtractOptionDescriptor(subForm);
                                optionList.Add(optionDescriptor);
                                break;

                            case "argument":
                                var argumentDescriptor = this.ExtractArgumentDescriptor(subForm);
                                argList.Add(argumentDescriptor);
                                break;

                            default:
                                throw new CliException($"Unknown action: '{action}'.");
                        }

                        break;

                    case "some-text":
                        action = subForm.GetSingleKeywordArgument<Symbol>(":action").Name.ToLowerInvariant();
                        if (action == "argument")
                        {
                            var argumentDescriptor = this.ExtractArgumentDescriptor(subForm);
                            argList.Add(argumentDescriptor);
                        }
                        else
                        {
                            throw new CliException($"Action '{action}' cannot be applied to node type '{subFormCar}'.");
                        }

                        break;

                    case "worker":
                    case "idle":
                    case "fallback":
                    case "end":
                        continue;

                    case "alt":
                    case "seq":
                    case "opt":
                        CollectItems(
                            subForm.GetFreeArguments(),
                            keyList,
                            argList,
                            optionList);
                        break;

                    default:
                        throw new CliException($"Unsupported node type: '{subFormCar}'.");
                }
            }
        }

        private CliExecutorOptionDescriptor ExtractOptionDescriptor(PseudoList subForm)
        {
            var alias = subForm.GetSingleKeywordArgument<Symbol>(":alias").Name.ToLowerInvariant();
            var options = subForm
                .GetAllKeywordArguments(":values")
                .Cast<StringAtom>()
                .Select(x => x.Value)
                .ToList();
            var description = subForm.GetSingleKeywordArgument<StringAtom>(":description", true)?.Value;

            var optionDescriptor = new CliExecutorOptionDescriptor(alias, options, description);

            return optionDescriptor;
        }

        private CliExecutorArgumentDescriptor ExtractArgumentDescriptor(PseudoList subForm)
        {
            var alias = subForm.GetSingleKeywordArgument<Symbol>(":alias").Name.ToLowerInvariant();
            var description = subForm.GetSingleKeywordArgument<StringAtom>(":description", true)?.Value;
            var docSubstitution = subForm.GetSingleKeywordArgument<StringAtom>(":doc-subst", true)?.Value;

            IList<string> values = null;
            if (subForm.GetCarSymbolName().ToLowerInvariant() == "multi-text")
            {
                values = subForm
                    .GetAllKeywordArguments(":values")
                    .Cast<StringAtom>()
                    .Select(x => x.Value)
                    .ToList();
            }

            var isMandatory = subForm.GetSingleArgumentAsBool(":is-mandatory") ?? false;
            var allowsMultiple = subForm.GetSingleArgumentAsBool(":allows-multiple") ?? false;

            var argumentDescriptor = new CliExecutorArgumentDescriptor(
                alias,
                values,
                isMandatory,
                allowsMultiple,
                description,
                docSubstitution);
            
            return argumentDescriptor;
        }

        private CliExecutorKeyDescriptor ExtractKeyDescriptor(PseudoList list, int index)
        {
            var keySubForm = list[index];
            var alias = keySubForm.GetSingleKeywordArgument<Symbol>(":alias").Name.ToLowerInvariant();
            var keys = keySubForm
                .GetAllKeywordArguments(":values")
                .Cast<StringAtom>()
                .Select(x => x.Value)
                .ToList();
            var isMandatory = keySubForm.GetSingleArgumentAsBool(":is-mandatory") ?? false;
            var allowsMultiple = keySubForm.GetSingleArgumentAsBool(":allows-multiple") ?? false;

            var valueSubForm = list[index + 1];
            IList<string> values = null;
            if (valueSubForm.GetCarSymbolName().ToLowerInvariant() == "multi-text")
            {
                values = valueSubForm
                    .GetAllKeywordArguments(":values")
                    .Cast<StringAtom>()
                    .Select(x => x.Value)
                    .ToList();
            }

            var valueDescription = valueSubForm.GetSingleKeywordArgument<StringAtom>(":description", true)?.Value;
            var valueDocSubstitution = valueSubForm.GetSingleKeywordArgument<StringAtom>(":doc-subst", true)?.Value;

            var keyDescriptor = new CliExecutorKeyDescriptor(
                alias,
                keys,
                isMandatory,
                allowsMultiple,
                values,
                valueDescription,
                valueDocSubstitution);

            return keyDescriptor;
        }
    }
}
