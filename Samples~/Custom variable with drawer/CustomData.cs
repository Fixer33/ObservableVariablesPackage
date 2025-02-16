using System;

namespace Sample
{
    [Serializable] // This attribute is only required if it will be used in a public or [SerializeField] field
    public struct CustomData
    {
        public string SomeValue;

        public override string ToString()
        {
            return SomeValue;
        }
    }
}