using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli.Data
{
    public class CliWorkerDescriptorBuilder
    {
        private readonly PseudoList _form;

        public CliWorkerDescriptorBuilder(string workerGrammar)
        {
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(workerGrammar);
            ITinyLispPseudoReader reader = new TinyLispPseudoReader();
            _form = reader.Read(tokens);
        }

        public CliWorkerDescriptor Build()
        {
            try
            {


                var topDefblock = _form.Single(x => x.GetSingleArgumentAsBool(":is-top") ?? false);
                var supposedCommandForm = topDefblock.GetFreeArguments().First();
                string name = null;
                if (supposedCommandForm.GetCarSymbolName() == "WORKER")
                {
                    name = supposedCommandForm.GetSingleKeywordArgument<Symbol>(":worker-name", true)?.Name;
                }

                var descriptor = new CliWorkerDescriptor
                {
                    Verb = supposedCommandForm
                        .GetAllKeywordArguments(":verbs")
                        .Cast<StringAtom>()
                        .First()
                        .Value,
                    Description = supposedCommandForm
                        .GetSingleKeywordArgument<StringAtom>(":description")
                        .Value
                };
                descriptor.UsageSamples.AddRange(supposedCommandForm
                    .GetSingleKeywordArgument(":usage-samples")
                    .AsPseudoList()
                    .Cast<StringAtom>()
                    .Select(x => x.Value));

                var keyList = new List<CliWorkerKeyDescriptor>();
                var argList = new List<CliWorkerArgumentDescriptor>();
                var optionList = new List<CliWorkerOptionDescriptor>();

                this.CollectItems(
                    topDefblock.AsPseudoList().GetFreeArguments(),
                    keyList,
                    argList,
                    optionList);

                descriptor.Keys = keyList;
                descriptor.Arguments = argList;
                descriptor.Options = optionList;

                return descriptor;
            }
            catch (Exception ex)
            {
                throw new CliException("Could not build worker descriptor.", ex);
            }
        }

        private void CollectItems(
            PseudoList list,
            List<CliWorkerKeyDescriptor> keyList,
            List<CliWorkerArgumentDescriptor> argList,
            List<CliWorkerOptionDescriptor> optionList)
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


                            default:
                                throw new NotImplementedException();
                        }

                        break;

                    case "some-text":
                        action = subForm.GetSingleKeywordArgument<Symbol>(":action").Name.ToLowerInvariant();
                        if (action == "argument")
                        {
                            var argumentDescriptor = this.ExtractArgumentDescriptor(subForm);
                            argList.Add(argumentDescriptor);
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
                        throw new NotImplementedException();
                }
            }
        }

        private CliWorkerOptionDescriptor ExtractOptionDescriptor(PseudoList subForm)
        {
            var optionDescriptor = new CliWorkerOptionDescriptor();
            optionDescriptor.Alias = subForm.GetSingleKeywordArgument<Symbol>(":alias").Name.ToLowerInvariant();
            optionDescriptor.Description = subForm.GetSingleKeywordArgument<StringAtom>(":description").Value;
            optionDescriptor.Options = subForm
                .GetAllKeywordArguments(":values")
                .Cast<StringAtom>()
                .Select(x => x.Value)
                .ToList();

            return optionDescriptor;
        }

        private CliWorkerArgumentDescriptor ExtractArgumentDescriptor(PseudoList subForm)
        {
            var argumentDescriptor = new CliWorkerArgumentDescriptor();
            argumentDescriptor.Alias = subForm.GetSingleKeywordArgument<Symbol>(":alias").Name.ToLowerInvariant();
            argumentDescriptor.Description = subForm.GetSingleKeywordArgument<StringAtom>(":description").Value;
            argumentDescriptor.DocSubstitution = subForm.GetSingleKeywordArgument<StringAtom>(":doc-subst").Value;

            return argumentDescriptor;
        }

        private CliWorkerKeyDescriptor ExtractKeyDescriptor(PseudoList list, int index)
        {
            var keyDescriptor = new CliWorkerKeyDescriptor();
            var keySubForm = list[index];
            var valueSubForm = list[index + 1];

            keyDescriptor.Alias = keySubForm.GetSingleKeywordArgument<Symbol>(":alias").Name.ToLowerInvariant();
            keyDescriptor.Keys = keySubForm
                .GetAllKeywordArguments(":values")
                .Cast<StringAtom>()
                .Select(x => x.Value)
                .ToList();

            var valueDescriptor = new CliWorkerValueDescriptor();
            valueDescriptor.Description = valueSubForm.GetSingleKeywordArgument<StringAtom>(":description").Value;
            valueDescriptor.DocSubstitution = valueSubForm.GetSingleKeywordArgument<StringAtom>(":doc-subst").Value;

            if (valueSubForm.GetCarSymbolName().ToLowerInvariant() == "multi-text")
            {
                valueDescriptor.Values = valueSubForm
                    .GetAllKeywordArguments(":values")
                    .Cast<StringAtom>()
                    .Select(x => x.Value)
                    .ToList();
            }

            keyDescriptor.ValueDescriptor = valueDescriptor;
            return keyDescriptor;
        }
    }
}
