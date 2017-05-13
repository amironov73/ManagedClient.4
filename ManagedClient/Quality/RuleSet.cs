// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RuleSet.cs --
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ManagedClient.Quality
{
    /// <summary>
    /// Набор правил.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [XmlRoot("ruleset")]
    [MoonSharpUserData]
    public sealed class RuleSet
    {
        #region Properties

        /// <summary>
        /// Правила, входящие в набор.
        /// </summary>
        [JsonProperty("rules")]
        public IrbisRule[] Rules { get; set; }

        #endregion

        #region Private members

        private static readonly Dictionary<string,Type> _registeredRules
            = new Dictionary<string, Type>();

        #endregion

        #region Public methods

        /// <summary>
        /// Merge two reports into the new one.
        /// </summary>
        [NotNull]
        public static RecordReport MergeReport
            (
                [NotNull] RecordReport first,
                [NotNull] RecordReport second
            )
        {
            RecordReport result = new RecordReport
            {
                Defects = first.Defects.Concat(second.Defects)
                    .ToList(),
                Description = first.Description,
                Gold = first.Gold + second.Gold - 1000,
                Mfn = first.Mfn,
                Index = first.Index
            };

            return result;
        }


        /// <summary>
        /// Проверка одной записи
        /// </summary>
        [NotNull]
        public RecordReport CheckRecord
            (
                [NotNull] RuleContext context
            )
        {
            RecordReport result = new RecordReport
            {
                Description = context.Client.FormatRecord
                (
                    context.BriefFormat,
                    context.Record.Mfn
                ),
                Index = context.Record.FM("903"),
                Mfn = context.Record.Mfn
            };

            RuleUtility.RenumberFields
                (
                    context.Record
                );

            result.Gold = 1000;
            int bonus = 0;

            foreach (IrbisRule rule in Rules)
            {
                RuleReport oneReport = rule.CheckRecord(context);
                result.Defects.AddRange(oneReport.Defects);
                result.Gold -= oneReport.Damage;
                bonus += oneReport.Bonus;
            }

            if (result.Gold >= 900)
            {
                result.Gold += bonus;
            }

            return result;
        }

        /// <summary>
        /// Получаем правило по его имени.
        /// </summary>
        [CanBeNull]
        public static IrbisRule GetRule
            (
                [NotNull] string name
            )
        {
            Type ruleType;
            if (!_registeredRules.TryGetValue
                (
                    name, 
                    out ruleType)
                )
            {
                return null;
            }

            IrbisRule result = (IrbisRule)Activator.CreateInstance
                (
                    ruleType
                );

            return result;
        }

        /// <summary>
        /// Load set of rules from the specified file.
        /// </summary>
        [NotNull]
        public static RuleSet LoadJson
            (
                [NotNull] string fileName
            )
        {
            string text = File.ReadAllText(fileName);
            JObject obj = JObject.Parse(text);
            
            RuleSet result = new RuleSet();
            List<IrbisRule> rules = new List<IrbisRule>();

            foreach (JToken o in obj["rules"])
            {
                string name = o.ToString();
                IrbisRule rule = GetRule(name);
                if (rule != null)
                {
                    rules.Add(rule);
                }
            }

            result.Rules = rules.ToArray();

            return result;
        }

        /// <summary>
        /// Регистрируем все правила из указанной сборки.
        /// </summary>
        public static void RegisterAssembly
            (
                [NotNull] Assembly assembly
            )
        {
            Type[] types = assembly
                .GetTypes()
                .Where(t => t.IsPublic)
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsSubclassOf(typeof(IrbisRule)))
                .ToArray();

            foreach (Type ruleType in types)
            {
                RegisterRule(ruleType);
            }
        }

        /// <summary>
        /// Регистрация встроенных правил.
        /// </summary>
        public static void RegisterBuiltinRules ()
        {
            RegisterAssembly(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Register the rule type.
        /// </summary>
        public static void RegisterRule
            (
                [NotNull] Type ruleType
            )
        {
            string ruleName = ruleType.Name;

            _registeredRules.Add
                (
                    ruleName,
                    ruleType
                );
        }

        /// <summary>
        /// Отменяем регистрацию правила с указанным именем.
        /// </summary>
        public static void UnregisterRule
            (
                [NotNull] string name
            )
        {
            if (_registeredRules.ContainsKey(name))
            {
                _registeredRules.Remove(name);
            }
        }

        #endregion
    }
}
