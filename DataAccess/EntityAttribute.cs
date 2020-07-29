using System;

namespace DataAccess
{
    public sealed class EntityAttribute : Attribute
    {
        public EntityAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
