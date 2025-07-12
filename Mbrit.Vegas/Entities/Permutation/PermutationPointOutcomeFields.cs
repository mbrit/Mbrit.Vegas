using BootFX.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    internal class PermutationPointOutcomeFields
    {
        internal EntityField Seen { get; }
        internal EntityField Hand { get; }
        internal EntityField Profit { get; }
        internal EntityField Wagered { get; }
        internal EntityField Ev { get; }

        internal PermutationPointOutcomeFields(Expression<Func<Permutation, bool>> seen, Expression<Func<Permutation, int>> hand,
            Expression<Func<Permutation, decimal>> profit, Expression<Func<Permutation, decimal>> wagered,
            Expression<Func<Permutation, decimal>> ev)
        {
            var et = typeof(Permutation).ToEntityType();
            this.Seen = et.GetField(seen);
            this.Hand = et.GetField(hand);
            this.Profit = et.GetField(profit);
            this.Wagered = et.GetField(wagered);
            this.Ev = et.GetField(ev);
        }
    }
}
