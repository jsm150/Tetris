using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tetris
{
    public class Weight
    {
        public enum Property
        {
            BlockHeightValue,
            BlockedValue,
            HoleValue,
            LineClearValue,
            SideValue,
            BlockValue,
            FloorValue
        }

        private static readonly
            IReadOnlyDictionary<Property, (Func<Weight, float> Getter, Action<Weight, float> Setter)> _propertyList;

        public float BlockHeightValue { get; set; }
        public float BlockedValue { get; set; }
        public float HoleValue { get; set; }
        public float LineClearValue { get; set; }
        public float SideValue { get; set; }
        public float BlockValue { get; set; }
        public float FloorValue { get; set; }

        public float this[Property p]
        {
            get => _propertyList[p].Getter(this);
            set => _propertyList[p].Setter(this, value);
        }

        static Weight()
        {
            Type type = typeof(Weight);
            List<(string name, PropertyInfo)> propertyList = Enum.GetNames(typeof(Property))
                .Select(name => (name, type.GetProperty(name)))
                .ToList();

            _propertyList = propertyList.ToDictionary(
                t => (Property) Enum.Parse(typeof(Property), t.name),
                t => (
                    (Func<Weight, float>) Delegate.CreateDelegate(typeof(Func<Weight, float>), t.Item2.GetGetMethod()),
                    (Action<Weight, float>) Delegate.CreateDelegate(typeof(Action<Weight, float>),
                        t.Item2.GetSetMethod())
                )
            );
        }
    }
}