using BootFX.Common.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class Simulator<T>
        where T : ISimulatorArgs
    {
        private T Args { get; }
        internal IEnumerable<ISimulatorStep> Steps { get; } = new List<ISimulatorStep>();

        internal Simulator()
        {
            this.Args = Activator.CreateInstance<T>();
        }

        internal void AddRange<TValue>(Expression<Func<T, TValue>> expression, TValue from, TValue to, TValue step) =>
            ((List<ISimulatorStep>)this.Steps).Add(new SimulatorSteps<T, TValue>(expression, from, to, step));

        internal Dictionary<T, IEnumerable<BankrollResults>> Run(Func<T, IEnumerable<BankrollResults>> callback)
        {
            var results = new Dictionary<T, IEnumerable<BankrollResults>>();

            // get all the runs...
            var args = this.ExpandArgs().ToList();
            foreach (var arg in args)
            {
                var result = callback(arg);
                results[arg] = result;
            }

            return results;
        }

        private IEnumerable<T> ExpandArgs()
        {
            var allValues = new Dictionary<ISimulatorStep, List<decimal>>();
            foreach (var step in this.Steps)
            {
                var values = step.ToList();
                allValues[step] = values.Select(v => ConversionHelper.ToDecimal(v)).ToList();
            }

            return GenerateCombinations(allValues, 0, new Dictionary<ISimulatorStep, decimal>());
        }

        private IEnumerable<T> GenerateCombinations(
            Dictionary<ISimulatorStep, List<decimal>> allValues,
            int currentStepIndex,
            Dictionary<ISimulatorStep, decimal> currentCombination)
        {
            if (currentStepIndex >= allValues.Count)
            {
                // We have a complete combination, create a new instance with these values
                var instance = Activator.CreateInstance<T>();
                foreach (var kvp in currentCombination)
                {
                    var step = kvp.Key;
                    var value = kvp.Value;

                    // Get the property info from the step
                    var property = step.GetProperty();
                    var convertedValue = ConversionHelper.ChangeType(value, property.PropertyType);
                    property.SetValue(instance, convertedValue);
                }
                yield return instance;
                yield break;
            }

            var currentStep = allValues.Keys.ElementAt(currentStepIndex);
            var values = allValues[currentStep];

            foreach (var value in values)
            {
                currentCombination[currentStep] = value;
                foreach (var combination in GenerateCombinations(allValues, currentStepIndex + 1, currentCombination))
                {
                    yield return combination;
                }
            }
        }
    }
}
